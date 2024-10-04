using Master.Core.DTO;

namespace Master.Data.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        public Task<List<GetTeacherCoursesDto>> GetMyCourses(int id);
        Task<List<object>> GetCategories();
        Task<List<object>> GetCoursesOfCategory(int categoryId);
        Task PutGradeForStudent(PutGradeDto grade);
        Task PutGradeFromExcel(List<PutGradeDto> gradeDto);
        Task<List<GetGradeDto>> GetGradesByStudentCourse(PutGradeDto gradeDto);
        Task PutFinalGradeForPeriod(PutGradeDto gradeDto, int grade);
        Task PutFinalGradeForCourse(PutGradeDto gradeDto, int finalGradeForCourse);
        Task<List<UnsubmittedReport>> GetUnsubmittedReportForTeacher(int v);
        Task<List<GetReportResponseDto>> GetMyReports(int teacherId, int schoolYearId);
        Task<List<GetTeacherWithCourseDto>> GetCoursesForTeacherByYear(List<int> categoryId, int currentTeacherId);
        Task<List<FormClassesDto>> GetMyFormClasses(int v);
        Task DeleteGrades(List<int?> gradeIdsToDelete);
        Task<List<int>> GetFinalGrades(List<GetFinalGrades> gradeIdsToDelete);
        public Task<GetTeacherDto> GetTeacherByCourse(int courseId);
    }
}
