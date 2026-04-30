namespace Angularintigration.Models
{
    public class PostModel
    {
        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public bool IsAnonymous { get; set; }

        public List<IFormFile>? Files { get; set; }
    }
}