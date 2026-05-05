using Angularintigration.Servicepattern.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Angularintigration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ISftpService _sftpService;

        public FileController(ISftpService sftpService)
        {
            _sftpService = sftpService;
        }

        [HttpPost("uploading")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected");
            }

            string fileName = Guid.NewGuid().ToString() +
                              Path.GetExtension(file.FileName);

            var remotePath = await _sftpService.UploadFile(file, fileName);

            string baseUrl = "http://172.17.32.216"; 

            string fileUrl = $"{baseUrl}/uploads/{fileName}";

            return Ok(new
            {
                Message = "File Uploaded Successfully",
                FilePath = fileUrl 
            });
        }
    }
}