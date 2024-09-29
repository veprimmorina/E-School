namespace Master.Core.DTO
{
    public class SchoolYearResponseDto
    {
        public int? id { get; set; }
        public string? SchoolYear { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? IsActive { get; set; }
    }
}
