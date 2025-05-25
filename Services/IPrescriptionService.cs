using System.Threading.Tasks;
using Tutorial11.DTOs;

namespace Tutorial11.Services
{
    public interface IPrescriptionService
    {
        Task AddPrescriptionAsync(PrescriptionRequestDto request);
        Task<PatientResponseDto> GetPatientDataAsync(int id);
    }
}