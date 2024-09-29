namespace Master.Core.DTO
{
    public class UnsubmittedReport
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Date { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
    }
}
