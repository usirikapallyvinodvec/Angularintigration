using Angularintigration.Models;

namespace Angularintigration.RepositryPattern.Interfaces
{
    public interface ILoginRepositry
    {
        Task<dynamic> LoginDetails(Login login);
    }
}
