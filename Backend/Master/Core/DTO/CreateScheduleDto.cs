namespace Master.Core.DTO
{
    public class CreateScheduleDto
    {
        public string Day { get; set; }
        public int Hour { get; set; }
        public int CourseId { get; set; }
        public int CategoryId { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public int? SchoolYearId { get; set; }
        public int? Id { get; set; }

    }
}
