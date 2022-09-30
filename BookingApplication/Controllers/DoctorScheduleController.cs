using AutoMapper;
using BookingApplication.Service;
using Contracts;
using Entities;
using Entities.DataTransferObjects.DoctorScheduleDtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public DoctorScheduleController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSchedulesAsync(TimeFrameDto timeFrame)
        {
            if (timeFrame != null)
            {
                var schedule = await _repository.DoctorSchedule.GetAllSchedulesAsync(timeFrame);
                if (schedule.Any()) return Ok(schedule);
                else return NotFound();
            }
            else return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleSlotById(Guid slotId)
        {
            if (slotId != null)
            {
                var slot = await _repository.DoctorSchedule.GetScheduleSlotById(slotId);
                if (slot != null)
                    return Ok(slot);
                else BadRequest();
            }
            return BadRequest();

        }
        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorSchedule(GetScheduleByDoctorIdDto doctor)
        {
            if (doctor != null)
            {
                var doctorSchedule = await _repository.DoctorSchedule.GetScheduleSlotsByDoctorAsync(doctor);
                if (doctorSchedule.Any()) return Ok(doctorSchedule);
                else return NotFound();

            }
            return BadRequest();
        }
        //[Authorize(Policy = "admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateDoctorSchedule([FromBody] ScheduleSlotRangeDto doctorSchedule)
        {
            try
            {
                if (doctorSchedule == null)
                {
                    _logger.LogError("ScheduleSlotRange object sent from the client is null");
                    return BadRequest("ScheduleSlotRange object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid ScheduleSlotRange object sent from the client");
                    return BadRequest("Invalid model object");
                }
                //var procedureEntity = _mapper.Map <DoctorSchedule>(doctorSchedule);
                //procedureEntity.Id = Guid.NewGuid();
                var doctorIds = await _repository.DoctorSchedule.GetDoctorIdsAsync();
                if (doctorIds == null)
                {
                    return BadRequest();
                }
                var slots = new List<DoctorSchedule>();
                doctorIds.ForEach(doctorId =>
                {
                    var slots = Timeline.GenerateSlots(doctorId, doctorSchedule.Start, doctorSchedule.End, doctorSchedule.Weekends);
                });
                if( !slots.Any())
                    return BadRequest();
                slots.ForEach(slot =>
                    {
                        _repository.DoctorSchedule.Create(slot);
                    });
                var result = await _repository.SaveAsync();
                if (result != 0)
                {
                    //var createdProcedure = _mapper.Map<DoctorSchedule>(procedureEntity);
                    //return CreatedAtRoute("ProcedureCreated", procedure);
                    return Ok($"Doctor Schedules created ");
                }
                else
                {
                    _logger.LogInfo($"Creating Doctors Schedule in range {doctorSchedule.Start} - {doctorSchedule.End} in the Database failed ");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Creating Doctors Schedule Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
        //Update should not be made in case there is an appointment made using DoctorSchedule Id
        //[Authorize(Policy = "admin_doctor")]
        [HttpPut]
        public async Task<ActionResult> UpdateSlot(SlotUpdateDto slotUpdate)
        {
            try
            {
                if (slotUpdate == null)
                {
                    _logger.LogError("SlotUpdate object sent from the client is null");
                    return BadRequest("SlotUpdate object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid SlotUpdate object sent from the client");
                    return BadRequest("Invalid model object");
                }
                var appointment = _repository.Appointment.GetByCondition(a => a.DoctorScheduleId == slotUpdate.Id);
                if (appointment != null)
                {
                    return BadRequest("There is an appointment set on this slot");
                }
                var slot = await _repository.DoctorSchedule.GetScheduleSlotById(slotUpdate.Id);
                if (slot is null)
                {
                    _logger.LogError($"SlotUpdate with id: {slotUpdate.Id}, hasn't been found in db.");
                    return NotFound();
                }
                _mapper.Map(slotUpdate, slot);
                _repository.DoctorSchedule.Update(slot);
                var result = await _repository.SaveAsync();
                if (result != 0)
                {
                    var procedureResult = _mapper.Map<ScheduleSlotDto>(slot);
                    return Ok(procedureResult);
                }
                else
                {
                    _logger.LogInfo($"Updating Schedule slot {slot.Id} in the Database failed ");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update Slot Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedure(Guid id)
        {
            try
            {
                var scheduleSlotEntity = await _repository.DoctorSchedule.GetScheduleSlotById(id);
                if (scheduleSlotEntity == null)
                {
                    _logger.LogError($"Procedure with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                //Check if there's any appointment with this procedure used

                var appointments = _repository.Appointment.GetByCondition(a => a.DoctorScheduleId == id);
                if (appointments.Any())
                {
                    _logger.LogError($"Cannot delete DoctorSchedule slot with id: {id}. It has related accounts. Delete those accounts first");
                    return BadRequest("Cannot delete DoctorSchedule slot. It has related appointments. Delete those appointments first");
                }
                _repository.DoctorSchedule.Delete(scheduleSlotEntity);
                var result = await _repository.SaveAsync();
                if (result != 0)
                {

                    return Ok($"Schedule slot: start - '{scheduleSlotEntity.ConsultationStart}' end - '{scheduleSlotEntity.ConsultationEnd}', successfully deleted from the database");
                }
                else
                {
                    _logger.LogInfo($"Deleting DoctorSchedule slot {scheduleSlotEntity.Id} in the Database failed ");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete DoctorSchedule slot Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }



    }
}
