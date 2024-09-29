using Master.Core.DTO;
using Master.Core.Wrappers;

namespace Master.Core.Interfaces
{
    public interface IHelperService
    {
        public string GetCurrentDayInWords();
        public Task<BaseResponse<SchoolYearResponseDto>> GetCurrentActiveYearAndCategory();
        public Task<BaseResponse<List<GetScheduleDto>>> GetScheduleByYear(int schoolYear);
        public Task<BaseResponse<List<int>>> GetChldrenCategoriesId(int categoryId);
        public Task<BaseResponse<PeriodResponseDto>> GetCurrentActivePeriod();
        public Task<BaseResponse<GetFormTeacherDto>> GetFormTeacherByClassId(int classId);
        public Task<BaseResponse<List<PeriodResponseDto>>> GetPeriods();
        public BaseResponse<string> SendEmail(string email, string? firstName, string? lastName, string? context);
        public Task<BaseResponse<GetHourDto>> GetHours(int hourId);
    }
}
