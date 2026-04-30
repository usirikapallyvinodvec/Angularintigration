using Angularintigration.Models;

namespace Angularintigration.RepositryPattern.Interfaces
{
    public interface IRegistrationRepositry
    {
        Task<int> RegisterUser(RegistrationPage registrationPage);
    }
}
