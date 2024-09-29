namespace Master.Core.DTO
{
    public class GetReportDetailsResponseDto
    {
        public int Id;
        public DateTime Date { get; set; }
        public string Details { get; set; }
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public List<ReportAbsenceDto>? Absences { get; set; }
        public List<ReportRemarkDto>? Remarks { get; set; }
    }
}
