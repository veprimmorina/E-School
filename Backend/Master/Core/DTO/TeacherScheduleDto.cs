namespace Master.Core.DTO
{
    public class TeacherScheduleDto
    {
        public string Day { get; set; }
        public List<GetScheduleDto> Schedules { get; set; }
    }
}
