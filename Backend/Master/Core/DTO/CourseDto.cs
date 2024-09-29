using System.Diagnostics;

namespace Master.Core.DTO
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<PeriodResponseDto> Periods { get; set; }
        public int? FinalGrade { get; set; }
        public bool? IsMyCourse { get; set; }
    }
}
