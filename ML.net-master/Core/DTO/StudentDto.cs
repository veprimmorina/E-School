using Microsoft.ML.Data;

namespace Core.DTO
{
    public class StudentDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int MesatarjaENotaveVitinParaprak { get; set; }
        public int MesatarjaTani { get; set; }
        public int MungesatVitinEKaluar { get; set; }
        public int MungesatTani { get; set; }
        public int NotaESjelljesVitinEKaluar { get; set; }
        public int NotaESjelljesTani { get; set; }
        public int NderveprimiMeOER { get; set; }
        public int DorezimiIDetyrave { get; set; }
    }
}
