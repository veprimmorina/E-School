using Microsoft.ML.Data;

namespace Core.DTO
{
    public class ResultModel
    {
        [ColumnName("PredictedLabel")]
        public bool WillDrop { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public float Score { get; set; }
    }
}
