using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Data.Repositories.Interfaces
{
    public interface IAdministrationRepository
    {
        public Task<BaseResponse<int>> CreateCategoryForSchoolYear(CreateNewSchoolYearDto schoolYearDto);
        public Task<BaseResponse<List<int>>> CreateClasses(List<CreateNewSchoolYearDto> classes);
        public Task<BaseResponse<int>> CreateSchoolYear(CreateNewSchoolYearDto schoolYear);
        public Task<BaseResponse<CategoryOrderAndPath>> GetLastCategorySortOrderAndId();
        public Task<BaseResponse<string>> CreateFormPeriodsForClasses(List<int> classes);
        public Task<BaseResponse<List<SchoolYearResponseDto>>> GetSchoolYears();
        public Task<BaseResponse<List<PeriodResponseDto>>> GetPeriods();
        public Task<BaseResponse<List<GetScheduleDto>>> GetSchedule(int schoolYearId);
        public Task<BaseResponse<List<GetFormTeacherDto>>> GetFormTeachers(int schoolYearId);
        public Task<BaseResponse<List<GetReportResponseDto>>> GetReports(int? schoolYearId);
        public Task<BaseResponse<string>> EditSchoolYear(EditSchoolYearDto editSchoolYear);
        public Task<BaseResponse<string>> EditPeriod(EditPeriodDto editPeriod);
        public Task<BaseResponse<List<GetReportResponseDto>>> GetReportDetails(int reportId);
        Task<BaseResponse<List<GetTeacherDto>>> GetAllTeachers();
        Task<BaseResponse<string>> ChangePeriodStatus(ChangePeriodStatusDto changePeriodStatus);
        Task<BaseResponse<int>> CreatePeriod(CreatePeriodDto createPeriod);
        Task<BaseResponse<string>> DeletePeriod(int id);
        Task<BaseResponse<string>> ChangeSchoolYearStatus(ChangePeriodStatusDto changeSchoolYearStatus);
        public Task<BaseResponse<List<UnsubmittedReport>>> GetUnsubmittedReports();
        public Task<BaseResponse<List<GetClassesDto>>> GetAllClassesForSchoolYear(int categoryId);
        Task<BaseResponse<List<CoursesDto>>> GetAllCoursesForClasses(List<GetClassesDto> getClassesDtos);
        Task<BaseResponse<int>> GetTotalStudentsByMainCourses(List<CoursesDto> getMainCourse);
        Task<BaseResponse<int>> GetTotalTeachersByCourses(List<CoursesDto> courses);
        Task<BaseResponse<int>> GetTotalYears();
    }
}
