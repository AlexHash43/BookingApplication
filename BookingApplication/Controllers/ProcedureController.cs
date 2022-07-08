using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllProcedures()
        {
            try
            {
                var procedures = _repository.Procedure.GetAllProcedures();
                _logger.LogInfo("Returned all owners from Database");
                var proceduresResult = _mapper.Map<IEnumerable<ProcedureDto>>(procedures);
                return Ok(proceduresResult);
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetAllProcedures Failed: {ex.Message}");
                return StatusCode(500, $"Internal Sertver Error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetProcedureById(Guid Id)
        {
            try
            {
                var procedure = _repository.Procedure.GetByCondition(x => x.Id == Id);
                var procedureResult = _mapper.Map<IEnumerable<ProcedureDto>>(procedure);
                return Ok(procedureResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProcedureById Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error");
            }
        }
    }
}
