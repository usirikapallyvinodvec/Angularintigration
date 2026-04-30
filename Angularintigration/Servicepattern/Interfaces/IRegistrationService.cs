using Angularintigration.Models;

namespace Angularintigration.Servicepattern.Interfaces
{
    public interface IRegistrationService
    {
        Task<int> RegisterUser(RegistrationPage model);
    }
}
