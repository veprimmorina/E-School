namespace Master.Core.DTO
{
    public class GetClassesDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int? FormTeacherId { get; set; }
        public string? FormTeacherFirstName { get; set; }
        public string? FormTeacherLastName { get; set; }
        public string? UserDetails { get; set; }
    }
}
