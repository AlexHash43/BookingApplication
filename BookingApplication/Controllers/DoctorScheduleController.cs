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
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<User> _roleManager;
        private readonly AppointmentContext _context;

        public DoctorScheduleController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, UserManager<User> userManager, RoleManager<User> roleManager, AppointmentContext context )
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
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
        [Authorize(Roles = "Admin")]
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
                        _context.DoctorSchedule.Add(slot);
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

        //[Authorize(Roles = "Admin")]
        //[HttpPut]
        //public async Task<ActionResult> UpdateSlot (TimeFrameDto range)
        //{
        //    return NotImplementedException();
        //}


    }
}
