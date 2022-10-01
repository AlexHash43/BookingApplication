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

        //Should update apppointment status
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateAppointment(Guid appointmentId, [FromBody] ProcedureForUpdateDto procedure)
        //{
        //    try
        //    {
        //        if (procedure == null)
        //        {
        //            _logger.LogError("Procedure object sent from the client is null");
        //            return BadRequest("Procedure object is null");
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            _logger.LogError("Invalid procedure object sent from the client");
        //            return BadRequest("Invalid model object");
        //        }
        //        var procedureEntity = await _repository.Procedure.GetProcedureByIdAsync(id);
        //        if (procedureEntity is null)
        //        {
        //            _logger.LogError($"Procedure with id: {id}, hasn't been found in db.");
        //            return NotFound();
        //        }
        //        _mapper.Map(procedure, procedureEntity);
        //        _repository.Procedure.Update(procedureEntity);
        //        var result = await _repository.SaveAsync();
        //        if (result != 0)
        //        {
        //            var procedureResult = _mapper.Map<ProcedureDto>(procedureEntity);
        //            return Ok(procedureResult);
        //        }
        //        else
        //        {
        //            _logger.LogInfo($"Updating procedure {procedureEntity.ProcedureName} in the Database failed ");
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Update Procedure Failed: {ex.Message}");
        //        return StatusCode(500, $"Internal Server Error:{ex.Message}");
        //    }
        //}
    }
}
