using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Data.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<BaseResponse<string>> CreateAbsences(List<CreateAbsencesDto> absences);
        Task <BaseResponse<int>> CreateRemark(CreateRemarkDto remark);
        Task<BaseResponse<int>> CreateReport(ReportCreateDTO reportDto);
        Task<BaseResponse<string>> CreateReportTrack(List<CreateTrackReportDto> reportsToCreate);
        Task<BaseResponse<string>> CreateStudentsRemarks(List<StudentRemark> studentRemarks);
        Task<BaseResponse<string>> EditStudentAbsence(EditAbsenceDto absenceDto);
        Task<BaseResponse<AbsenceResponseDTO>> GetAbsencesForStudent(int id);
        Task<BaseResponse<List<CourseTeacherDto>>> GetHoursByDay(string currentDay);
    }
}
