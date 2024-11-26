using Euroleague.DTO;
using Euroleague.Repository;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Euroleague.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorize _authorizeService;

        public AuthController(IAuthorize authorizeService)
        {
            _authorizeService = authorizeService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _authorizeService.LogIn(model.Username, model.Password);
            var token = await _authorizeService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest model)
        {
            var user = await _authorizeService.SignIn(model.Username, model.Password, model.Email);
            var token = await _authorizeService.GenerateToken(user);
            return Ok(new { Token = token });
        }


    }
}
