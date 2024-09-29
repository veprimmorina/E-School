namespace Master.Core.DTO
{
    public class PeriodResponseDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool? is_active { get; set; }
        public List<GradeDto>? Grades { get; set; }
    }
}
