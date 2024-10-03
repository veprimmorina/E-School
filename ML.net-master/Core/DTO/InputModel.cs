using Microsoft.ML.Data;

namespace Core.DTO
{
    public class InputModel
    {
        [LoadColumn(0)]
        public float MesatarjaENotaveVitinParaprak { get; set; }
        [LoadColumn(1)]
        public float MesatarjaTani { get; set; }
        [LoadColumn(2)]
        public float MungesatVitinEKaluar { get; set; }
        [LoadColumn(3)]
        public float MungesatTani { get; set; }
        [LoadColumn(4)]
        public float NotaESjelljesVitinEKaluar { get;set; }
        [LoadColumn(5)]
        public float NotaESjelljesTani { get; set; }
        [LoadColumn(6)]
        public float NderveprimiMeOER { get; set; }
        [LoadColumn(7)]
        public float DetyratEPerfunduara {  get; set; }
        [LoadColumn(8)]
        public bool KaRenie { get; set; }
    }
}