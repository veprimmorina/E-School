using Master.Core.DTO;

namespace Master.Data.Repositories.Interfaces
{
    public interface IClassesRepository
    {
        Task CreateFormTeacher(CreateFormTeacher createFormTeacher);
        Task CreateSchedule(CreateScheduleDto createSchedule);
        Task DeleteSchedules(List<int?> scheduleIds);
        Task EditFormTeacher(CreateFormTeacher formTeacherDto);
        Task EditStudentsAbsences(List<EditAbsenceDto> absenceDto);
        Task<ClassDetailsDto> GetClassDetails(int id);
        Task<List<GetClassesWithoutFormTeacherDto>> GetClassesWithoutFormTeacher();
        Task<List<Object>> GetClassScheduleByClassId(int classId);
        Task<List<CourseDto>> GetCoursesByClassId(int categoryId);
        Task<List<Object>> GetCurrentCoursesBeingHold(TimeSpan localTime, String day);
        Task<List<GetClassesDto>> GetMyClasses(int teacherId);
        Task<List<StudentAbsenceDto>> GetNewAbsencesForStudents(List<int> studentIds);
        Task<List<StudentsClassGrades>> GetStudentsOfClass(int classId);
        Task<List<StudentsGradeResponseDto>> ImportStudentsOfClass(int classId);
    }
}
