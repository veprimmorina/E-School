using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Core.Interfaces
{
    public interface IAdministrationService
    {
        Task<BaseResponse<string>> CreateSchoolYear(CreateNewSchoolYearDto schoolYearDto);
        Task<BaseResponse<List<PeriodResponseDto>>> GetPeriods();
        Task<BaseResponse<List<SchoolYearResponseDto>>> GetSchoolYears();
        public Task<BaseResponse<List<TeacherScheduleDto>>> GetSchedule(int? schoolYearId);
        public Task<BaseResponse<List<GetFormTeacherDto>>> GetFormTeachers();
        public Task<BaseResponse<List<GetReportResponseDto>>> GetReports(int? schoolYearId);
        public Task<BaseResponse<List<GetReportDetailsResponseDto>>> GetReportDetailsById(int reportId);
        public Task<BaseResponse<string>> EditSchoolYear(EditSchoolYearDto editSchoolYear);
        public Task<BaseResponse<string>> EditPeriod(EditPeriodDto editPeriod);
        Task<BaseResponse<int>> GetActivePeriod();
        Task<BaseResponse<List<GetTeacherDto>>> GetAllTeachers();
        Task<BaseResponse<List<GetTeacherDto>>> GetRecommendedFormTeachers();
        Task<BaseResponse<string>> ChangePeriodStatus(ChangePeriodStatusDto changePeriodStatusDto);
        Task<BaseResponse<int>> CreatePeriod(CreatePeriodDto createPeriod);
        Task<BaseResponse<string>> DeletePeriod(int id);
        Task<BaseResponse<string>> ChangeSchoolYearStatus(ChangePeriodStatusDto changeSchoolYearStatus);
        Task<BaseResponse<List<UnsubmittedReport>>> GetUnsubmittedReports();
        Task<BaseResponse<string>> SendEmailToTeacherForUnsubmittedReport(int courseId);
        Task<BaseResponse<List<CoursesDto>>> GetAllCoursesOfSchoolYear();
        Task<BaseResponse<StatisticsDto>> GetStats();
    }
}
