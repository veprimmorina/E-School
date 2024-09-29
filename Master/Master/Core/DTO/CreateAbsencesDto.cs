namespace Master.Core.DTO
{
    public class CreateAbsencesDto
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public bool? Reasonable { get; set; }
        public int? SchoolYear { get; set; }
        public int? Period { get; set; }
    }
}
