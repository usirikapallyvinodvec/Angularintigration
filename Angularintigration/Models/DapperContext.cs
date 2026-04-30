using Npgsql;
using System.Data;

namespace Angularintigration.Models
{
    public class DapperContext
    {
        private readonly IConfiguration _Config;
        private readonly string _str;
        public DapperContext(IConfiguration Config)
        {
            _Config = Config;
            _str = _Config.GetConnectionString("DefaultConnection");
        }
        public IDbConnection Getconn()
            {
           return new NpgsqlConnection(_str);
        }
    }
}
