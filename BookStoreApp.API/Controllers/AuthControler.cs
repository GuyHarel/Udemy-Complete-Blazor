using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControler : ControllerBase
    {
        private readonly ILogger<AuthControler> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;

        public AuthControler( ILogger<AuthControler> logger, IMapper mapper, UserManager<ApiUser> userManager)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var user = mapper.Map<ApiUser>(userDto);
            var result = await userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                    return BadRequest(ModelState);
                }
            }

            await userManager.AddToRoleAsync(user, "User");

            return Accepted();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
                var user = await userManager.FindByEmailAsync(userDto.Email.ToUpperInvariant());
                if (user == null)
                {
                    return Unauthorized();
                }

                var result = await userManager.CheckPasswordAsync(user, userDto.Password);
                if (!result)
                {
                    return Unauthorized();
                }

                return Accepted();

        }

    }
}
