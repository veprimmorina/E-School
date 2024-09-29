using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Core.Interfaces
{
    public interface ITeacherService
    {
        Task<BaseResponse<List<GetTeacherCoursesDto>>> GetMyCourses();
        Task<BaseResponse<List<object>>> GetCategories();
        Task<BaseResponse<List<object>>> GetCoursesOfCategory(int categoryId);
        Task<BaseResponse<string>> PutGradeForStudent(List<PutGradeDto> gradeDto);
        Task<BaseResponse<string>> PutGradesFromExcel(List<PutGradeDto> grades);
        Task<BaseResponse<string>> TrackReports();
        Task<BaseResponse<List<UnsubmittedReport>>> GetUnsubmittedReports();
        Task<BaseResponse<List<GetReportResponseDto>>> GetMyReports(int schoolYearId);
        Task<BaseResponse<List<TeacherScheduleDto>>> GetMySchedule(int? schoolYearId);
        Task<BaseResponse<List<FormClassesDto>>> GetMyFormClasses();
    }
}
