using Microsoft.ML.Data;

namespace Master.Core.DTO.ML.Model2
{
    public class InputModel2
    {
        [LoadColumn(0)]
        public int Id { get; set; }
        [LoadColumn(1)]
        public int Nationality { get; set; }
        [LoadColumn(2)]
        public int Age { get; set; }
        [LoadColumn(3)]
        public int TaskSubmitted { get; set; }
        [LoadColumn(4)]
        public int ORLInteraction { get; set; }
        [LoadColumn(5)]
        public int QuizResult { get; set; }
        [LoadColumn(6)]
        public bool WillDrop { get; set; }
    }
}
