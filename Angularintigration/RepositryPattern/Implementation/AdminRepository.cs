using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Dapper;

namespace Angularintigration.RepositryPattern.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DapperContext _context;

        public AdminRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> ChangeRole(int userId, int roleId)
        {
            using var connection = _context.Getconn();

            var query = @"
            UPDATE vinod.users
            SET roleid = @RoleId
            WHERE userid = @UserId;";

            return await connection.ExecuteAsync(query, new
            {
                UserId = userId,
                RoleId = roleId
            });

        }

        public async Task<int> DeleteAnyPost(int postId)
        {
            using var connection = _context.Getconn();

          
            await connection.ExecuteAsync(@"
                DELETE FROM vinod.postfiles
                WHERE postid = @PostId;", new
            {
                PostId = postId
            });

           
            var result = await connection.ExecuteAsync(@"
                DELETE FROM vinod.posts
                WHERE postid = @PostId;", new
            {
                PostId = postId
            });

            return result;
        }

        public async Task<IEnumerable<dynamic>> GetApprovedPosts()
        {
            using var connection = _context.Getconn();

                        var query = @"
                SELECT p.postid,
                       p.title,
                       p.username,
                       p.description,
                       p.createddate,
                       p.ispinned,
                       f.fileurl,
                       f.filetype
                FROM vinod.posts p
                LEFT JOIN vinod.postfiles f
                       ON p.postid = f.postid
                WHERE p.status = 'Approved'
                ORDER BY p.ispinned DESC,
                         p.createddate DESC;";

            return await connection.QueryAsync(query);
        }

        public async Task<ReportModel> GetReports()
        {
            using var connection = _context.Getconn();

            var query = @"
    SELECT
    (SELECT COUNT(*) FROM vinod.users WHERE roleid = 1) AS TotalUsers,
    (SELECT COUNT(*) FROM vinod.users WHERE roleid = 2) AS TotalModerators,
    (SELECT COUNT(*) FROM vinod.users WHERE roleid = 3) AS TotalAdmins,
    (SELECT COUNT(*) FROM vinod.posts) AS TotalPosts,
   (SELECT COUNT(*) FROM vinod.posts WHERE status='Approved') AS ApprovedPosts,
    (SELECT COUNT(*) FROM vinod.posts WHERE status='Pending') AS PendingPosts,
    (SELECT COUNT(*) FROM vinod.posts WHERE status='Rejected') AS RejectedPosts,
    (SELECT COUNT(*) FROM vinod.posts WHERE isanonymous=true) AS AnonymousPosts,
    (SELECT COUNT(*) FROM vinod.users WHERE isactive=true) AS ActiveUsers,
    (SELECT COUNT(*) FROM vinod.users WHERE isactive=false) AS BlockedUsers;";

            return await connection.QueryFirstOrDefaultAsync<ReportModel>(query);
        }

        public async Task<IEnumerable<dynamic>> GetUsers()
        {
            using var connection = _context.Getconn();

            var query = @"
            SELECT u.userid,
                   u.fullname,
                   u.email,
                   u.mobile,
                   u.isactive,
                   u.createdate,
                   r.rolename,
                   u.roleid
            FROM vinod.users u
            INNER JOIN vinod.roles r
            ON u.roleid = r.roleid
                WHERE u.roleid IN (1,2)
            ORDER BY u.userid DESC;";

            return await connection.QueryAsync(query);
        }

        public async Task<int> ToggleUsersStatus(int userId)
        {
            using var connection = _context.Getconn();
            var query = @"
            UPDATE vinod.users
            SET isactive = NOT isactive
            WHERE userid = @UserId;"; //using Toggle Condition

            return await connection.ExecuteAsync(query, new
            {
                UserId = userId
            });
        }
    }
}
