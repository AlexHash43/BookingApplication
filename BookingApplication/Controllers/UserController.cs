using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, UserManager<User> userManager)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }


    }
}
