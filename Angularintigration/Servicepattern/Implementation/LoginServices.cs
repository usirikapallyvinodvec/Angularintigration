using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Angularintigration.Servicepattern.Interfaces;

namespace Angularintigration.Servicepattern.Implementation
{
    public class LoginServices : ILoginServices
    {
        private readonly ILoginRepositry _Repo;

        public LoginServices(ILoginRepositry Repo)
        {
            _Repo = Repo;
        }

        public Task<dynamic> LoginDetails(Login login)
        {
            return _Repo.LoginDetails(login);
        }
    }
}
