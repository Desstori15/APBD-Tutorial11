using Microsoft.EntityFrameworkCore;
using Tutorial11.Data;
using Tutorial11.DTOs;
using Tutorial11.Models;

namespace Tutorial11.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly AppDbContext _context;

        public PrescriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPrescriptionAsync(PrescriptionRequestDto request)
        {
            if (request.Medicaments.Count > 10)
                throw new Exception("Cannot add more than 10 medicaments.");

            if (request.DueDate < request.Date)
                throw new Exception("DueDate must be greater than or equal to Date.");

            var doctor = await _context.Doctors.FindAsync(request.IdDoctor)
                         ?? throw new Exception("Doctor not found.");

            var patient = await _context.Patients.FindAsync(request.IdPatient);
            if (patient == null)
            {
                patient = new Patient
                {
                    IdPatient = request.IdPatient,
                    // Добавь другие свойства пациента при необходимости
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            foreach (var m in request.Medicaments)
            {
                var medicament = await _context.Medicaments.FindAsync(m.IdMedicament);
                if (medicament == null)
                    throw new Exception($"Medicament with ID {m.IdMedicament} not found.");
            }

            var prescription = new Prescription
            {
                Date = request.Date,
                DueDate = request.DueDate,
                IdDoctor = request.IdDoctor,
                IdPatient = request.IdPatient
            };
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            foreach (var m in request.Medicaments)
            {
                _context.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = m.IdMedicament,
                    Dose = m.Dose,
                    Details = m.Details
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<PatientResponseDto> GetPatientDataAsync(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(p => p.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Prescriptions)
                    .ThenInclude(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.IdPatient == id);

            if (patient == null)
                throw new Exception("Patient not found.");

            return new PatientResponseDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Prescriptions = patient.Prescriptions
                    .OrderBy(p => p.DueDate)
                    .Select(p => new PrescriptionInfoDto
                    {
                        IdPrescription = p.IdPrescription,
                        Date = p.Date,
                        DueDate = p.DueDate,
                        Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentInfoDto
                        {
                            IdMedicament = pm.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Medicament.Description
                        }).ToList(),
                        Doctor = new DoctorInfoDto
                        {
                            IdDoctor = p.Doctor.IdDoctor,
                            FirstName = p.Doctor.FirstName,
                            LastName = p.Doctor.LastName
                        }
                    }).ToList()
            };
        }
    }
}
