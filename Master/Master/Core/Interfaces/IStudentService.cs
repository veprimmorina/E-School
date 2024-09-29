using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Core.Interfaces
{
    public interface IStudentService
    {
        public Task<BaseResponse<List<StudentAbsenceDto>>> GetMyAbsences();
        public Task<BaseResponse<GetFormTeacherDto>> GetMyFormTeacher();
        public Task<BaseResponse<List<StudentDto>>> GetMyGrades();
        public Task<BaseResponse<List<TeacherScheduleDto>>> GetMySchedule();
        Task<BaseResponse<StudentStats>> GetStats();
    }
}
