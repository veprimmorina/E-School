using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Data.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        public Task<BaseResponse<List<GetScheduleDto>>> GetScheduleByClass(int classId);
        public Task<BaseResponse<ClassDto>> GetClassForStudent(int studentId);
        public Task<BaseResponse<List<StudentAbsenceDto>>> GetStudentAbsences(int schoolYearId, int periodId, int studentId);
        public Task<BaseResponse<List<StudentsClassGrades>>> GetGradesForStudentAndCourses(int studentId, List<int> courseIds);
    }
}
