using Master.Core.DTO;

namespace Master.Data.Repositories.Interfaces
{
    public interface IAdministrationRepository
    {
        public Task<int> CreateCategoryForSchoolYear(CreateNewSchoolYearDto schoolYearDto);
        public Task<List<int>> CreateClasses(List<CreateNewSchoolYearDto> classes);
        public Task<int> CreateSchoolYear(CreateNewSchoolYearDto schoolYear);
        public Task<CategoryOrderAndPath> GetLastCategorySortOrderAndId();
        public Task CreateFormPeriodsForClasses(List<int> classes);
        public Task<List<SchoolYearResponseDto>> GetSchoolYears();
        public Task<List<PeriodResponseDto>> GetPeriods();
        public Task<List<GetScheduleDto>> GetSchedule(int schoolYearId);
        public Task<List<GetFormTeacherDto>> GetFormTeachers(int schoolYearId);
        public Task<List<GetReportResponseDto>> GetReports(int? schoolYearId);
        public Task<string> EditSchoolYear(EditSchoolYearDto editSchoolYear);
        public Task<string> EditPeriod(EditPeriodDto editPeriod);
        public Task<List<GetReportResponseDto>> GetReportDetails(int reportId);
        Task<List<GetTeacherDto>> GetAllTeachers();
        Task<string> ChangePeriodStatus(ChangePeriodStatusDto changePeriodStatus);
        Task<int> CreatePeriod(CreatePeriodDto createPeriod);
        Task<string> DeletePeriod(int id);
        Task<string> ChangeSchoolYearStatus(ChangePeriodStatusDto changeSchoolYearStatus);
        public Task<List<UnsubmittedReport>> GetUnsubmittedReports();
        public Task<List<GetClassesDto>> GetAllClassesForSchoolYear(int categoryId);
        Task<List<CoursesDto>> GetAllCoursesForClasses(List<GetClassesDto> getClassesDtos);
        Task<int> GetTotalStudentsByMainCourses(List<CoursesDto> getMainCourse);
        Task<int> GetTotalTeachersByCourses(List<CoursesDto> courses);
        Task<int> GetTotalYears();
    }
}
