using Contracts;
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

        public ProcedureController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;

        }
        [HttpGet]
        public IActionResult GetAllProcedures()
        {
            try
            {
                var procedures = _repository.Procedure.GetAllProcedures();
                _logger.LogInfo("Returned all owners from Database");
                return Ok(procedures);
            }
            catch(Exception ex)
            {
                _logger.LogError("GetAllProcedures Failed");
                return StatusCode(500, "Internal Sertver Error");
            }
        }
    }
}
