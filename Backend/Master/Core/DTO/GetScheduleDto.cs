namespace Master.Core.DTO
{
    public class GetScheduleDto
    {
        public int Id { get; set; }
        public int Hour { get; set; }
        public string? Day { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public int school_year_id { get; set; }

    }
}
