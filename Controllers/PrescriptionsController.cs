using Microsoft.AspNetCore.Mvc;
using Tutorial11.DTOs;
using Tutorial11.Services;

namespace Tutorial11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _service;

        public PrescriptionsController(IPrescriptionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDto request)
        {
            try
            {
                await _service.AddPrescriptionAsync(request);
                return Ok(new { message = "Prescription added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientData(int id)
        {
            try
            {
                var result = await _service.GetPatientDataAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}