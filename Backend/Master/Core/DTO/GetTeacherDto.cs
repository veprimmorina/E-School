namespace Master.Core.DTO
{
    public class GetTeacherDto
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? isRecommended { get; set; }
        public string? CourseName { get; set; }
        public string? Email { get; set; }
    }
}
