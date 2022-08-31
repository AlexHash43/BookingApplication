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
                if (procedures.Any())
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
            catch (Exception ex)
            {
                _logger.LogError($"GetAllProcedures Failed: {ex.Message}");
                return StatusCode(500, $"Internal Sertver Error: {ex.Message}");
            }
        }
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetProcedureById(Guid id)
        {
            try
            {
                var procedure = await _repository.Procedure.GetProcedureByIdAsync(id);
                if (procedure != null)
                {
                    _logger.LogInfo($"Returned procedure by Id {id} from Database");
                    var procedureResult = _mapper.Map<ProcedureDto>(procedure);
                    return Ok(procedureResult);
                }
                else
                {
                    _logger.LogInfo($"No procedure with Id: {id} was found in the Database");
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProcedureById Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
        [HttpGet("{procedureName}")]
        public async Task<IActionResult> GetProcedureByName(string procedureName)
        {
            try
            {
                var procedure = await _repository.Procedure.GetProcedureByNameAsync(procedureName);
                if (procedure != null)
                {
                    _logger.LogInfo($"Returned procedure by Name {procedureName} from Database");
                    var procedureResult = _mapper.Map<ProcedureDto>(procedure);
                    return Ok(procedureResult);
                }
                else
                {
                    _logger.LogInfo($"No procedure with Name: {procedureName} was found in the Database");
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProcedureById Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProcedure([FromBody]ProcedureCreationDto procedure)
        {
            try
            {
                if(procedure == null)
                {
                    _logger.LogError("Procedure object sent from the client is null");
                    return BadRequest("Procedure object is null");
                }
                if(!ModelState.IsValid)
                {
                    _logger.LogError("Invalid procedure object sent from the client");
                    return BadRequest("Invalid model object");
                }
                var procedureEntity = _mapper.Map<Procedure>(procedure);
                //procedureEntity.Id = Guid.NewGuid();
                _repository.Procedure.CreateProcedure(procedureEntity);

                   var result =  await _repository.SaveAsync();
                 if (result != 0)
                {
                    var createdProcedure = _mapper.Map<ProcedureDto>(procedureEntity);
                    //return CreatedAtRoute("ProcedureCreated", procedure);
                    return Ok($"Procedure created: {procedure.ProcedureName}");
                }
                else
                {
                    _logger.LogInfo($"Creating procedure {procedure.ProcedureName} in the Database failed ");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Creating Procedure Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProcedure(Guid id, [FromBody] ProcedureForUpdateDto procedure )
        {
            try
            {
                if (procedure == null)
                {
                    _logger.LogError("Procedure object sent from the client is null");
                    return BadRequest("Procedure object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid procedure object sent from the client");
                    return BadRequest("Invalid model object");
                }
                var procedureEntity = await _repository.Procedure.GetProcedureByIdAsync(id);
                if (procedureEntity is null)
                {
                    _logger.LogError($"Procedure with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _mapper.Map(procedure, procedureEntity);
                _repository.Procedure.Update(procedureEntity);
                var result = await _repository.SaveAsync();
                if (result != 0)
                {
                    var procedureResult = _mapper.Map<ProcedureDto>(procedureEntity);
                    return Ok(procedureResult);
                }
                else
                {
                    _logger.LogInfo($"Updating procedure {procedureEntity.ProcedureName} in the Database failed ");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update Procedure Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedure(Guid id )
        {
            try
            {
                var procedureEntity = await _repository.Procedure.GetProcedureByIdAsync(id);
                if (procedureEntity == null)
                {
                    _logger.LogError($"Procedure with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                //Check if there's any appointment with this procedure used

                //if (_repository.Appointment.AccountsByOwner(id).Any())
                //{
                //    _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                //}
                _repository.Procedure.Delete(procedureEntity);
                var result = await _repository.SaveAsync();
                if (result != 0)
                {
                    
                    return Ok($"Procedure: '{procedureEntity.ProcedureName}' successfully deleted from the database");
                }
                else
                {
                    _logger.LogInfo($"Deleting procedure {procedureEntity.ProcedureName} in the Database failed ");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update Procedure Failed: {ex.Message}");
                return StatusCode(500, $"Internal Server Error:{ex.Message}");
            }
        }
    }
}
