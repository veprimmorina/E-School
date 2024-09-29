namespace Master.Core.DTO
{
    public class ReportCreateDTO
    {
        public int ReportId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public List<CreateAbsencesDto?>? Absences { get; set; }
        public CreateRemarkDto? Remark { get; set; }
    }
}
