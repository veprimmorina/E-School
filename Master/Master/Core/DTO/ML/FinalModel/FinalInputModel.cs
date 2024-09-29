using Microsoft.ML.Data;

namespace Master.Core.DTO.ML.FinalModel
{
    public class FinalInputModel
    {
        [LoadColumn(0)]
        public int AverageLastYear { get; set; }
        [LoadColumn(1)]
        public int AverageNow { get; set; }
        [LoadColumn(2)]
        public int AbsencesLastYear { get; set; }
        [LoadColumn(3)]
        public int AbsencesNow { get; set; }
        [LoadColumn(4)]
        public int ConductGradeLastYear { get; set; }
        [LoadColumn(5)]
        public int ConductGradeNow { get; set; }
        [LoadColumn(6)]
        public int TaskSubmitted { get; set; }
        [LoadColumn(7)]
        public int ORLInteraction { get; set; }
        [LoadColumn(8)]
        public bool WillDrop { get; set; }
    }
}
