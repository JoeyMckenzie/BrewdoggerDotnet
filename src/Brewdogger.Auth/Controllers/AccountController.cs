using AutoMapper;
using Brewdogger.Auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brewdogger.Auth.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;

        public AccountController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            return Ok();
        }
    }
}