using Angularintigration.Models;
using Angularintigration.Servicepattern.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Angularintigration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _service;

        public PostController(IPostService service)
        {
            _service = service;
        }

        [HttpGet("editpost/{id}")]
        public async Task<IActionResult> EditPost(int id)
        {
            var result = await _service.GetPostById(id);
            return Ok(result);
        }

        [HttpPut("updatepost")]
        public async Task<IActionResult> UpdatePost([FromForm] EditPostModel model)
        {
            var result = await _service.UpdatePost(model);

            return Ok(new
            {
                message = "Post Updated Successfully"
            });
        }
        [HttpGet("homeposts")]
        public async Task<IActionResult> GetHomePosts(int page = 1, int pageSize = 10)
        {
            var result = await _service.GetHomePosts(page, pageSize);
            return Ok(result);
        }
        [HttpGet("myposts/{userId}")]
        public async Task<IActionResult> GetMyPosts(int userId)
        {
            var result = await _service.GetMyPosts(userId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddPost([FromForm] PostModel model)
        {
            try
            {
                var result = await _service.AddPost(model);

                return Ok(new
                {
                    message = "Post Added Successfully",
                    postId = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpDelete("deletepost")]
        public async Task<IActionResult> DeletePost(
      int PostId,
  int UserId)
        {
            var model = new DeletePostModel
            {
                PostId = PostId,
                UserId = UserId
            };

            var result = await _service.DeletePost(model);

            if (result > 0)
            {
                return Ok(new
                {
                    message = "Post Deleted Successfully"
                });
            }

            return BadRequest(new
            {
                message = "Delete Failed"
            });
        }
        [HttpGet("pendingposts")]
        public async Task<IActionResult> PendingPosts()
        {
            var result = await _service.GetPendingPosts();
            return Ok(result);
        }

        [HttpPut("approvepost")]
        public async Task<IActionResult> ApprovePost(int postId, int moderatorId)
        {
            await _service.ApprovePost(postId, moderatorId);

            return Ok(new { message = "Post Approved" });
        }

        [HttpPut("rejectpost")]
        public async Task<IActionResult> RejectPost(
        int postId,
        int moderatorId,
        string reason)
        {
            await _service.RejectPost(postId, moderatorId, reason);

            return Ok(new { message = "Post Rejected" });
        }

        [HttpGet("postlist")]
        public async Task<IActionResult> GetPostList()
        {
            var result = await _service.GetPostList();

            return Ok(result);
        }
    }
}