using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApplication.Controllers
{
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public AppointmentsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments(DateTime start, DateTime end, Guid doctorId)
        {
            try
            {
                var appointments = await _repository.Appointment.GetAllAppointmentsAsync(start, end, doctorId);
                if (appointments.Any())
                {
                    _logger.LogInfo("Returned all procedures from Database");
                    var appointmentsResult = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
                    return Ok(appointmentsResult);
                }
                else
                {
                    _logger.LogInfo("No procedures in the Database");
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllProcedures Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("free")]
        public async Task<IActionResult> GetPatientAppointments([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] Guid patient)
        {
            var patientAppointments = await _repository.Appointment.GetPatientAppointments(patient);
                return Ok(patientAppointments);

        }


    }
}
