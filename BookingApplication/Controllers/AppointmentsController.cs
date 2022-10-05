using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.AppointmentDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApplication.Controllers
{
    //Check Mapping!!!
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
        public async Task<IActionResult> GetAllAppointments(DateTime start, DateTime end)
        {
            try
            {
                var appointments = await _repository.Appointment.GetAllAppointmentsAsync(start, end);
                if (appointments.Any())
                {
                    _logger.LogInfo("Returned all Appointments from Database");
                    var appointmentsResult = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
                    return Ok(appointmentsResult);
                }
                else
                {
                    _logger.LogInfo("No Appointments in the Database");
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllAppointments Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("patient")]
        public async Task<IActionResult> GetPatientAppointments(AppointmentRange appointmentRange)
        {
            //var patientAppointments = await _repository.Appointment.GetPatientAppointments(start, end, patientId);
            //return Ok(patientAppointments);
            try
            {
                var patientAppointments = await _repository.Appointment.GetPatientAppointments(appointmentRange);
                if (patientAppointments.Any())
                {
                    _logger.LogInfo("Returned all patient appointments from Database");
                    var appointmentsResult = _mapper.Map<IEnumerable<AppointmentDto>>(patientAppointments);
                    return Ok(appointmentsResult);
                }
                else
                {
                    _logger.LogInfo("No patient appointments in the Database");
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetPatientAppointments Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorAppointments(AppointmentRange appointmentRange)
        {
            try
            {
                var patientAppointments = await _repository.Appointment.GetDoctorAppointments(appointmentRange);
                if (patientAppointments.Any())
                {
                    _logger.LogInfo("Returned all doctor appointments from Database");
                    var appointmentsResult = _mapper.Map<IEnumerable<AppointmentDto>>(patientAppointments);
                    return Ok(appointmentsResult);
                }
                else
                {
                    _logger.LogInfo("No doctor appointments in the Database");
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDoctorAppointments Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreationDto appointmentToCreate)
        {
            try
            {
                if (appointmentToCreate == null)
                {
                    _logger.LogError("AppointmentCreationDto object sent from the client is null");
                    return BadRequest("AppointmentCreationDto object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid AppointmentCreationDto object sent from the client");
                    return BadRequest("Invalid model object");
                }
                var newAppointment = _repository.Appointment.CreateAppointmentAsync(appointmentToCreate);
                //procedureEntity.Id = Guid.NewGuid();
                _repository.Appointment.Create(newAppointment);

                var result = await _repository.SaveAsync();
                if (result != 0)
                {
                    var createdAppointment = _mapper.Map<AppointmentDto>(newAppointment);
                    //return CreatedAtRoute("ProcedureCreated", procedure);
                    return Ok($"Appointment created : patientId -  {newAppointment.PatientId}, Appointment start -  {newAppointment.AppointmentStart}");
                }
                else
                {
                    _logger.LogInfo($"Creating Appointment schedule Id:{newAppointment.DoctorScheduleId} starting at {newAppointment.AppointmentStart}  in the Database failed ");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Creating Appointment Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            try
            {
                var appointmentEntity = await _repository.Appointment.GetAppointmentByIdAsync(id);
                if (appointmentEntity == null)
                {
                    _logger.LogError($"Appointment with id: '{id}' not found in the db.");
                    return NotFound("Appointment not found");
                }
                //Check if there's any appointment with this procedure used

                //if (_repository.Appointment.AccountsByOwner(id).Any())
                //{
                //    _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                //}
                _repository.Appointment.Delete(appointmentEntity);
                var result = await _repository.SaveAsync();
                if (result != 0)
                {
                    _logger.LogInfo($"Appointment: '{appointmentEntity.Id}' successfully deleted from the database");
                    return Ok($"Appointment: '{appointmentEntity.Id}' successfully deleted from the database");
                }
                else
                {
                    _logger.LogInfo($"Deleting appointment {appointmentEntity.Id} in the Database failed ");
                    return BadRequest($"Deleting appointment {appointmentEntity.Id} in the Database failed ");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Deleting appoitnment Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }

        //Should update apppointment status
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Guid appointmentId, [FromBody] AppointmentForUpdateDto appointmentUpd)
        {
            try
            {
                if (appointmentUpd == null)
                {
                    _logger.LogError("Appointment object sent from the client is null");
                    return BadRequest("Appointment object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Appointment object sent from the client");
                    return BadRequest("Invalid Appointment model object");
                }
                var appointmentEntity = await _repository.Appointment.GetAppointmentByIdAsync(appointmentId);
                if (appointmentEntity is null)
                {
                    _logger.LogError($"Appointment with id: {appointmentId}, hasn't been found in db.");
                    return NotFound();
                }
                _mapper.Map(appointmentUpd, appointmentEntity);
                _repository.Appointment.Update(appointmentEntity);
                var result = await _repository.SaveAsync();
                if (result != 0)
                {
                    var procedureResult = _mapper.Map<AppointmentDto>(appointmentEntity);
                    return Ok(procedureResult);
                }
                else
                {
                    _logger.LogInfo($"Updating Appointment {appointmentEntity.Id} in the Database failed ");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update Appointment Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
    }
}
