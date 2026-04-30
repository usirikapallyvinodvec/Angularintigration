using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Angularintigration.Servicepattern.Interfaces;
using System.Diagnostics.Metrics;

namespace Angularintigration.Servicepattern.Implementation
{
    public class RegistrationServices : IRegistrationService
    {
        private readonly IRegistrationRepositry _Repo;

        public RegistrationServices(IRegistrationRepositry Repo)
        {
            _Repo = Repo;
        }

        public Task<int> RegisterUser(RegistrationPage registrationPage)
        {
            return _Repo.RegisterUser(registrationPage);
        }
    }
}
