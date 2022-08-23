using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Controllers
{
    [Route("api/procedure")]
    [ApiController]
    public class ProcedureController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public ProcedureController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProcedures()
        {
            try
            {
                var procedures = await _repository.Procedure.GetAllProceduresAsync();
                if(procedures.Any())
                {
                    _logger.LogInfo("Returned all procedures from Database");
                    var proceduresResult = _mapper.Map<IEnumerable<ProcedureDto>>(procedures);
                    return Ok(proceduresResult);
                }
                else
                {
                    _logger.LogInfo("No procedures in the Database");
                    return BadRequest();
                }

            }
            catch(Exception ex)
            {
                _logger.LogError($"GetAllProcedures Failed: {ex.Message}");
                return StatusCode(500, $"Internal Sertver Error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProcedureById(Guid id)
        {
            try
            { 
                var procedure = await _repository.Procedure.GetProcedureByIdAsync(id);
                if (procedure != null)
                {
                    _logger.LogInfo($"Returned procedure by Id {id} from Database");
                    var procedureResult = _mapper.Map<IEnumerable<ProcedureDto>>(procedure);
                    return Ok(procedureResult);
                }
                else 
                {
                    _logger.LogInfo($"No procedure by Id {id} in the Database");
                    return BadRequest(); 
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProcedureById Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProcedure(Procedure procedure)
        {
            try
            {
                _repository.Procedure.Update(procedure);
                var result = await _repository.SaveAsync();
                if (result != 0)
                {
                    var procedureResult = _mapper.Map<IEnumerable<ProcedureDto>>(procedure);
                    return Ok(procedureResult);
                }
                else { return BadRequest(); }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Update Procedure Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
    }
}
