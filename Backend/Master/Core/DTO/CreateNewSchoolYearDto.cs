using System.Collections.Generic;

namespace Master.Core.DTO
{
    public class CreateNewSchoolYearDto
    {

        public int? Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; } = "";
        public int? Parent { get; set; } = 0;
        public int SortOrder { get; set; } = 1;
        public List<CreateNewSchoolYearDto> Classes { get; set; } = new List<CreateNewSchoolYearDto>();
        public int? SchoolYearId { get; set; }
        public bool? IsActive { get; set; }
        public int? FormTeacherId { get; set; }
        public string? Path { get; set; }
    }
}
