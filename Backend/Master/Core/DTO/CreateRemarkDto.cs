namespace Master.Core.DTO
{
    public class CreateRemarkDto
    {
        public int? ReportId { get; set; }
        public string? Description { get; set; }
        public int? PeriodId { get; set; }
        public int? SchoolYearId { get; set; }
        public List<int?>? UserIds { get; set; }
    }
}
