using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Core.Interfaces
{
    public interface IClassesService
    {
        Task<BaseResponse<string>> CreateFormTeacher(CreateFormTeacher createFormTeacher);
        Task<BaseResponse<string>> CreateSchedule(List<CreateScheduleDto> createSchedule);
        Task<BaseResponse<ClassDetailsDto>> GetClassDetails(int id);
        Task<BaseResponse<List<object>>> GetClassScheduleByClassId(int classId);
        Task<BaseResponse<List<object>>> GetCurrentCoursesBeingHold();
        Task<BaseResponse<List<GetClassesDto>>> GetMyClasses();
        Task<BaseResponse<List<StudentAbsenceDto>>> GetNewAbsencesForStudents(int classId);
        Task<BaseResponse<List<StudentsGradeResponseDto>>> GetStudentsByCourse(int courseId);
        Task<BaseResponse<List<StudentDto>>> GetStudentsOfClass(int classId);
        Task<BaseResponse<string>> EditStudentsAbsences(List<EditAbsenceDto> absenceDto);
        Task<BaseResponse<List<GetClassesWithoutFormTeacherDto>>> GetClassesWithoutFormTeacher();
        Task<BaseResponse<string>> EditFormTeacher(CreateFormTeacher formTeacherDto);
        // Task<BaseResponse<List<StudentsGradeResponseDto>>> ImportStudentsOfClass(int classId);
    }
}
