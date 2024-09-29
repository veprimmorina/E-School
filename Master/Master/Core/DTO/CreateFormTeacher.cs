namespace Master.Core.DTO
{
    public class CreateFormTeacher
    {
        public int? Id { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public int? SchoolYearId { get; set; }
    }
}
