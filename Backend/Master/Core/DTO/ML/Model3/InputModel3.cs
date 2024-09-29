using Microsoft.ML.Data;

namespace Master.Core.DTO.ML.Model3
{
    public class InputModel3
    {
        [LoadColumn(2)]
        public int Attendance { get; set; }
        [LoadColumn(3)]
        public int PreviousQualification { get; set; }
        [LoadColumn(6)]
        public int Remarks { get; set; }
        [LoadColumn(26)]
        public bool WillDrop { get; set; }
    }
}
