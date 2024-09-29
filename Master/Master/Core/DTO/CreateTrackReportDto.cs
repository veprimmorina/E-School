namespace Master.Core.DTO
{
    public class CreateTrackReportDto
    {
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public string Date { get; set; }
        public bool IsSubmited { get; set; } = false;
        public int SchoolYearId { get; set; }
    }
}
