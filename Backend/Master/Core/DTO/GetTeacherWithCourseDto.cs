namespace Master.Core.DTO
{
    public class GetTeacherWithCourseDto
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherLastName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
