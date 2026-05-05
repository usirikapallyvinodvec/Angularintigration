namespace Angularintigration.Servicepattern.Interfaces
{
    public interface ISftpService
    {
        Task<string> UploadFile(IFormFile file, string fileName);
    }
}
