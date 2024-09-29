using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Core.Interfaces
{
    public interface IReportService
    {
        Task<BaseResponse<string>> CreateReport(ReportCreateDTO reportDto);
        Task<BaseResponse<string>> EditStudentAbsence(EditAbsenceDto absenceDto);
        Task<BaseResponse<AbsenceResponseDTO>> GetAbsencesForStudent(int id);
    }
}
