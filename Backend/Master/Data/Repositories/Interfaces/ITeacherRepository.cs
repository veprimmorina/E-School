using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Data.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        public Task<BaseResponse<List<GetTeacherCoursesDto>>> GetMyCourses(int id);
        Task<List<object>> GetCategories();
        Task<List<object>> GetCoursesOfCategory(int categoryId);
        Task<BaseResponse<string>> PutGradeForStudent(PutGradeDto grade);
        Task<BaseResponse<string>> PutGradeFromExcel(List<PutGradeDto> gradeDto);
        Task<BaseResponse<List<GetGradeDto>>> GetGradesByStudentCourse(PutGradeDto gradeDto);
        Task<BaseResponse<string>> PutFinalGradeForPeriod(PutGradeDto gradeDto, int grade);
        Task<BaseResponse<string>> PutFinalGradeForCourse(PutGradeDto gradeDto, int finalGradeForCourse);
        Task<BaseResponse<List<UnsubmittedReport>>> GetUnsubmittedReportForTeacher(int v);
        Task<BaseResponse<List<GetReportResponseDto>>> GetMyReports(int teacherId, int schoolYearId);
        Task<BaseResponse<List<GetTeacherWithCourseDto>>> GetCoursesForTeacherByYear(List<int> categoryId, int currentTeacherId);
        Task<BaseResponse<List<FormClassesDto>>> GetMyFormClasses(int v);
        Task<BaseResponse<string>> DeleteGrades(List<int?> gradeIdsToDelete);
        Task<BaseResponse<List<int>>> GetFinalGrades(List<GetFinalGrades> gradeIdsToDelete);
        public Task<BaseResponse<GetTeacherDto>> GetTeacherByCourse(int courseId);
    }
}
