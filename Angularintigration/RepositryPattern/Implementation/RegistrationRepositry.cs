using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Dapper;

namespace Angularintigration.Repositories
{
    public class RegisterRepository : IRegistrationRepositry
    {
        private readonly DapperContext _context;

        public RegisterRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> RegisterUser(RegistrationPage model)
        {
            using var connection = _context.Getconn();

         
            var checkQuery = @"SELECT COUNT(1)
                               FROM vinod.users
                               WHERE email = @Email";

            var exists = await connection.ExecuteScalarAsync<int>(
                checkQuery,
                new { Email = model.Email }
            );

            if (exists > 0)
            {
                return -1;
            }

            var query = @"insert into vinod.users
                          (fullname,email,mobile,password,roleid)
                          values
                          (@FullName,@Email,@Mobile,@Password,@RoleId)";

            return await connection.ExecuteAsync(query, model);
        }
    }
}