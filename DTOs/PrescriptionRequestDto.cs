namespace Tutorial11.DTOs
{
    public class PrescriptionRequestDto
    {
        public int IdDoctor { get; set; }
        public int IdPatient { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public List<PrescriptionMedicamentDto> Medicaments { get; set; }
    }
}