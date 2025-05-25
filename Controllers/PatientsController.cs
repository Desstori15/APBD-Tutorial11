using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial11.Data;

namespace Tutorial11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .FirstOrDefaultAsync(p => p.IdPatient == id);

            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            var result = new
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthdate = patient.Birthdate,
                Prescriptions = patient.Prescriptions.OrderBy(p => p.DueDate).Select(prescription => new
                {
                    IdPrescription = prescription.IdPrescription,
                    Date = prescription.Date,
                    DueDate = prescription.DueDate,
                    Medicaments = prescription.PrescriptionMedicaments.Select(pm => new
                    {
                        pm.Medicament.IdMedicament,
                        pm.Medicament.Name,
                        pm.Dose,
                        pm.Details
                    }),
                    Doctor = new
                    {
                        prescription.Doctor.IdDoctor,
                        prescription.Doctor.FirstName,
                        prescription.Doctor.LastName
                    }
                })
            };

            return Ok(result);
        }
    }
}
