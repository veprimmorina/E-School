namespace Master.Core.DTO
{
    public class StudentDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<CourseDto> Courses { get; set; }
    }
}
