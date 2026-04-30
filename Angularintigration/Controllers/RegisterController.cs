using Angularintigration.Models;
using Angularintigration.Servicepattern.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Angularintigration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegistrationService _service;

        public RegisterController(IRegistrationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationPost([FromBody] RegistrationPage registrationPage)
        {
          
            if (registrationPage.Password != registrationPage.ConfirmPassword)
            {
                return BadRequest(new
                {
                    message = "Password mismatch"
                });
            }


            var result = await _service.RegisterUser(registrationPage);

      
            if (result == -1)
            {
                return BadRequest(new
                {
                    message = "This email already exists"
                });
            }

            return Ok(new
            {
                message = "Registration Success"
            });
        }
    }
}