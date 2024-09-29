namespace Master.Core.DTO
{
    public class GetReportResponseDto
    {
        public int id { get; set; }
        public DateTime Date { get; set; }
        public string Details { get; set; }
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public int? StudentId { get; set; }
        public int? RemarkId { get; set; }
        public string? StudentFirstName { get; set; }
        public string? StudentLastName { get; set; }
        public int? StudentRemarkId { get; set; }
        public string? StudentRemarkFirstName { get; set; }
        public string? StudentRemarkLastName { get; set; }
        public string? RemarkDescription { get; set; }
    }
}
