namespace Angularintigration.Models
{
    public class EditPostModel
    {
       
        
        public int PostId { get; set; }

        public int UserId { get; set; }

      
        public string Title { get; set; } = string.Empty;

       
        public string Description { get; set; } = string.Empty;

     
        public IFormFile? File { get; set; }
    }
}