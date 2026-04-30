using Angularintigration.Models;

namespace Angularintigration.Servicepattern.Interfaces
{
    public interface ILoginServices
    {
        Task<dynamic> LoginDetails(Login login);
    }
}
