namespace Master.Core.DTO
{
    public class GetFinalGrades
    {
        public int? CourseId { get; set; }
        public int ClassId { get; set; }
        public int UserId { get; set; }
        public int? PeriodId { get; set; }
    }
}
