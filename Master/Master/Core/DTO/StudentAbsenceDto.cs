namespace Master.Core.DTO
{
    public class StudentAbsenceDto
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public bool? Reasonable { get; set; }
        public int SchoolYearId { get; set; }
        public string SchoolYear { get; set; }
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public string? StudentFirstName { get; set; }
        public string? StudentLastName { get; set; }
    }
}
