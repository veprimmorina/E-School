using Core.DTO;
using Core.Wrappers;

namespace Core.Services
{
    public interface IModelService
    {
        BaseResponse<string> SaveModel();
        List<ResultModel> Predict(List<StudentDto> students);
        void GenerateAndExportPredictions(List<StudentDto> students, string excelFilePath);
    }
}
