namespace Master.Core.DTO
{
    public class PutGradeDto
    {
        public int? CourseId { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public int? PeriodId { get; set; }
        public int Grade { get; set; }
        public bool IsFinal { get; set; }
        public int? GradeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
