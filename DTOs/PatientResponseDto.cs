namespace Tutorial11.DTOs
{
    public class PatientResponseDto
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PrescriptionInfoDto> Prescriptions { get; set; }
    }

    public class PrescriptionInfoDto
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public List<MedicamentInfoDto> Medicaments { get; set; }
        public DoctorInfoDto Doctor { get; set; }
    }

    public class MedicamentInfoDto
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public int Dose { get; set; }
        public string Description { get; set; }
    }

    public class DoctorInfoDto
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}