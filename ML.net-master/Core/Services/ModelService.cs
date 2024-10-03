using Core.Wrappers;
using AutoMapper;
using Microsoft.ML;
using Core.DTO;
using OfficeOpenXml;
using System.IO;
using Core.Other.Testing;

namespace Core.Services
{
    public class ModelService : IModelService
    {
        protected readonly IMapper _mapper;
        protected readonly MLContext _context;
        protected readonly metricsEvaluator metricEvaluator = new metricsEvaluator();

        public ModelService(IMapper mapper) { 
            _mapper= mapper;
            _context = new MLContext();
        }

        public BaseResponse<string> SaveModel()
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();

                var dataView = _context.Data.LoadFromTextFile<InputModel>(Path.Combine(currentDirectory, "Core\\Data", "StudentsDataset.csv"), hasHeader: true, separatorChar: ',');

                var pipeline = _context.Transforms.Concatenate("Features", "MesatarjaENotaveVitinParaprak", "MesatarjaTani", "MungesatVitinEKaluar", "MungesatTani", "NotaESjelljesVitinEKaluar", "DetyratEPerfunduara", "NderveprimiMeOER")
                .Append(_context.Transforms.NormalizeMinMax("Features"))
                .Append(_context.Transforms.Conversion.ConvertType("Label", nameof(InputModel.KaRenie), Microsoft.ML.Data.DataKind.Boolean))
                .Append(_context.BinaryClassification.Trainers.SdcaLogisticRegression());

                var crossValidationResults = _context.BinaryClassification.CrossValidate(dataView, pipeline, numberOfFolds: 10);

                var model = pipeline.Fit(dataView);

                _context.Model.Save(model, dataView.Schema, $"{currentDirectory}/Model.zip");

                var predictions = model.Transform(dataView);

                return BaseResponse<string>.Success("Model saved sucesfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.BadRequest(ex.Message);
            }
        }

        public List<ResultModel> Predict(List<StudentDto> students)
        {
            var areStudentsValid = ValidateStudents(students);

            if (!areStudentsValid)
            {
                Console.WriteLine("Can not make prediction with current data!");
                return new List<ResultModel> { new ResultModel() };
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var loadedModel = _context.Model.Load($"{currentDirectory}/Model.zip", out DataViewSchema schema);
            var predictionEngine = _context.Model.CreatePredictionEngine<InputModel, ResultModel>(loadedModel);

            var results = new List<ResultModel>();

            foreach (var submission in students)
            {
                var inputModel = _mapper.Map<InputModel>(submission);
                var result = predictionEngine.Predict(inputModel);

                results.Add(new ResultModel
                {
                    Name = submission.Name,
                    LastName = submission.LastName,
                    WillDrop = result.WillDrop
                });
            }

            return results;
        }

        private bool ValidateStudents(List<StudentDto> students)
        {
            foreach(var student in students)
            {
                if(student.DorezimiIDetyrave == 0 || student.NotaESjelljesVitinEKaluar == 0|| student.MesatarjaENotaveVitinParaprak == 0 || student.NderveprimiMeOER == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void ExportPredictionsToExcel(List<ResultModel> predictions, string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets.Add("Predictions");

                // Add header row
                worksheet.Cells[1, 1].Value = "Emri";
                worksheet.Cells[1, 2].Value = "Mbiemri";
                worksheet.Cells[1, 3].Value = "Ka Renie";

                // Add data rows
                for (int i = 0; i < predictions.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = predictions[i].Name;
                    worksheet.Cells[i + 2, 2].Value = predictions[i].LastName;
                    worksheet.Cells[i + 2, 3].Value = predictions[i].WillDrop ? "Po" : "Jo";
                }

                package.Save();
            }
        }

        public void GenerateAndExportPredictions(List<StudentDto> submissions, string excelFilePath)
        {
            var predictions = Predict(submissions);
            ExportPredictionsToExcel(predictions, excelFilePath);
        }
    }
}
