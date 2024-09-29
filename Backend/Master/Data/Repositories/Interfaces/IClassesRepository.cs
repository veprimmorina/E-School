using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Data.Repositories.Interfaces
{
    public interface IClassesRepository
    {
        Task<BaseResponse<string>> CreateFormTeacher(CreateFormTeacher createFormTeacher);
        Task<BaseResponse<string>> CreateSchedule(CreateScheduleDto createSchedule);
        Task<BaseResponse<string>> DeleteSchedules(List<int?> scheduleIds);
        Task<BaseResponse<string>> EditFormTeacher(CreateFormTeacher formTeacherDto);
        Task<BaseResponse<string>> EditStudentsAbsences(List<EditAbsenceDto> absenceDto);
        Task<BaseResponse<ClassDetailsDto>> GetClassDetails(int id);
        Task<BaseResponse<List<GetClassesWithoutFormTeacherDto>>> GetClassesWithoutFormTeacher();
        Task<List<Object>> GetClassScheduleByClassId(int classId);
        Task<BaseResponse<List<CourseDto>>> GetCoursesByClassId(int categoryId);
        Task<List<Object>> GetCurrentCoursesBeingHold(TimeSpan localTime, String day);
        Task<BaseResponse<List<GetClassesDto>>> GetMyClasses(int teacherId);
        Task<BaseResponse<List<StudentAbsenceDto>>> GetNewAbsencesForStudents(List<int> studentIds);
        Task<BaseResponse<List<StudentsClassGrades>>> GetStudentsOfClass(int classId);
        Task<BaseResponse<List<StudentsGradeResponseDto>>> ImportStudentsOfClass(int classId);
    }
}
