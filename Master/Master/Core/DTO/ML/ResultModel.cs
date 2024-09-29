using Microsoft.ML.Data;
using Newtonsoft.Json;

namespace Master.Core.DTO.ML
{
    public class ResultModel
    {
        [ColumnName("PredictedLabel")]
        public bool WillDrop { get; set; }
        [JsonIgnore]
        public float Score { get; set; }
    }
}