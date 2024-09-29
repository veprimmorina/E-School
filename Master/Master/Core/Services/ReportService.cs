using Master.Core.DTO;
using Master.Core.Interfaces;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;

namespace Master.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IHelperService _helperService;
        private readonly EncryptionHelper _encryption;

        public ReportService(IReportRepository reportRepository, IHelperService helperService)
        {
            _reportRepository = reportRepository;
            _helperService = helperService;
            _encryption = new EncryptionHelper(EncryptionKeyGenerator.GenerateKey(32));
        }

        public async Task<BaseResponse<string>> CreateReport(ReportCreateDTO reportDto)
        {
            reportDto.Description = _encryption.Encrypt(reportDto.Description);
            var result = await _reportRepository.CreateReport(reportDto);
            var currentSchoolPeriod = await _helperService.GetCurrentActivePeriod();
            var currentSchoolYear = await _helperService.GetCurrentActiveYearAndCategory();

            if (!result.IsSuccess)
            {
                return BaseResponse<string>.BadRequest("An error occurred!");
            }

            if (reportDto.Absences != null && reportDto.Absences.Any())
            {
                foreach (var absence in reportDto.Absences)
                {
                    absence.ReportId = result.Data;
                }

                var createAbsences = await _reportRepository.CreateAbsences(reportDto.Absences);

                if (!createAbsences.IsSuccess)
                {
                    return createAbsences;
                }
            }

            if (reportDto.Remark != null || reportDto.Remark.UserIds.Any())
            {
                reportDto.Remark.ReportId = result.Data;
                reportDto.Remark.SchoolYearId = currentSchoolYear.Data.id.Value;
                reportDto.Remark.PeriodId = currentSchoolPeriod.Data.id;

                var createRemark = await _reportRepository.CreateRemark(reportDto.Remark);

                if (!createRemark.IsSuccess)
                {
                    return BaseResponse<string>.BadRequest(createRemark.Message);
                }

                var studentRemarks = new List<StudentRemark>();
                foreach (var userId in reportDto.Remark.UserIds)
                {
                    var studentRemarkDto = new StudentRemark
                    {
                        StudentId = userId.Value,
                        RemarkId = createRemark.Data
                    };

                    studentRemarks.Add(studentRemarkDto);
                }

                await _reportRepository.CreateStudentsRemarks(studentRemarks);
            }

            return BaseResponse<string>.Success("Successfully saved!");
        }

        public async Task<BaseResponse<string>> EditStudentAbsence(EditAbsenceDto absenceDto)
        {
            var result = await _reportRepository.EditStudentAbsence(absenceDto);
            return result;
        }

        public async Task<BaseResponse<AbsenceResponseDTO>> GetAbsencesForStudent(int id)
        {
            var result = await _reportRepository.GetAbsencesForStudent(id);
            return result;
        }
    }
}