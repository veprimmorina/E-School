using Master.Core.DTO;

namespace Master.Data.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        public Task<List<GetScheduleDto>> GetScheduleByClass(int classId);
        public Task<ClassDto> GetClassForStudent(int studentId);
        public Task<List<StudentAbsenceDto>> GetStudentAbsences(int schoolYearId, int periodId, int studentId);
        public Task<List<StudentsClassGrades>> GetGradesForStudentAndCourses(int studentId, List<int> courseIds);
    }
}
