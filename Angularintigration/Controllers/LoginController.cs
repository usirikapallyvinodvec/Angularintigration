using Angularintigration.Models;
using Angularintigration.Servicepattern.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Angularintigration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginServices _service;

        public LoginController(ILoginServices service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> LoginPost(Login model)
        {
            var result = await _service.LoginDetails(model);

            if (result == null)
            {
                return BadRequest(new
                {
                    message = "Invalid Email or Password"
                });
            }
            if (result.isactive == false)
            {
                return BadRequest(new
                {
                    message = "Your Account is Blocked"
                });
            }
            return Ok(new
            {
                message = "Login Success",
                data = result
            });
        }
    }
}
