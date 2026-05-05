using Angularintigration.Servicepattern.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;

public class SftpService : ISftpService
{
    private readonly IConfiguration _config;

    public SftpService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string> UploadFile(IFormFile file, string fileName)
    {
        var host = _config["Sftp:Host"];
        var port = int.Parse(_config["Sftp:Port"]);
        var username = _config["Sftp:Username"];
        var password = _config["Sftp:Password"];
        var remoteFolder = _config["Sftp:RemotePath"]; 

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(remoteFolder))
            throw new Exception("SFTP configuration missing");

        using var client = new SftpClient(host, port, username, password);

        try
        {
            client.Connect();

           
            if (!client.Exists(remoteFolder))
            {
                client.CreateDirectory(remoteFolder);
            }

            string remotePath = $"{remoteFolder}/{fileName}";

            using (var stream = file.OpenReadStream())
            {
                client.UploadFile(stream, remotePath, true);
            }

            return remotePath;
        }
        catch (Exception ex)
        {
            throw new Exception("SFTP Upload Failed: " + ex.Message);
        }
        finally
        {
            if (client.IsConnected)
                client.Disconnect();
        }
    }
}