
using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Dapper;
using System.Reflection;

namespace Angularintigration.Repositories
{
    public class LoginRepository : ILoginRepositry
    {
        private readonly DapperContext _context;

        public LoginRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<dynamic> LoginDetails(Login login)
        {
            var query = @"select userid,fullname,email,roleid,isactive from vinod.users where email = @Email and password = @Password";
            using var connection = _context.Getconn();  
            return await connection.QueryFirstOrDefaultAsync(query, login);
        }
    }
}