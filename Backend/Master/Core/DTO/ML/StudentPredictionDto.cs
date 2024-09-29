namespace Master.Core.DTO.ML
{
    public class StudentPredictionDto
    {

        public int AverageLastYear { get; set; }
        public int AverageNow { get; set; }
        public int AbsencesLastYear { get; set; }
        public int AbsencesNow { get; set; }
        public int ConductGradeLastYear { get; set; }
        public int ConductGradeNow { get; set; }
        public int TaskSubmitted { get; set; }
        public int ORLInteraction { get; set; }
        public bool WillDrop { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
