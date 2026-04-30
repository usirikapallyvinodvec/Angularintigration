using Angularintigration.Servicepattern.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Angularintigration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _service.GetUsers();
            return Ok(result);
        }

        [HttpPut("changerole")]
        public async Task<IActionResult> ChangeRole(
           int userId,
           int roleId)
        {
            await _service.ChangeRole(userId, roleId);

            return Ok(new
            {
                message = "Role Updated"
            });
        }
        [HttpPut("toggleuser")]
        public async Task<IActionResult> ToggleUser(int userId)
        {
            await _service.ToggleUserStatus(userId);

            return Ok(new
            {
                message = "User Status Updated"
            });
        }
        [HttpGet("posts")]
        public async Task<IActionResult> GetPosts()
        {
            var result = await _service.GetApprovedPosts();

            return Ok(result);
        }

        [HttpDelete("deletepost")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            await _service.DeleteAnyPost(postId);

            return Ok(new
            {
                message = "Post Deleted Successfully"
            });
        }
        [HttpGet("reports")]
        public async Task<IActionResult> GetReports()
        {
            var result = await _service.GetReports();

            return Ok(result);
        }

            
    }
}
