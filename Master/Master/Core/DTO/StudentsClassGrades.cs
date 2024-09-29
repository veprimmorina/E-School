namespace Master.Core.DTO
{
    public class StudentsClassGrades
    {
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GradeId { get; set; }
        public bool IsFinal { get; set; }
        public int Grade { get; set; }
    }
}
