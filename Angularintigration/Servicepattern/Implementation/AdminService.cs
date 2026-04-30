using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Angularintigration.Servicepattern.Interfaces;

namespace Angularintigration.Servicepattern.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _context;

        public AdminService(IAdminRepository context)
        {
            _context = context;
        }

        public Task<int> ChangeRole(int userId, int roleId)
        {
            return _context.ChangeRole(userId, roleId);
        }

        public Task<int> DeleteAnyPost(int postId)
        {
            return _context.DeleteAnyPost(postId);
        }

        public Task<IEnumerable<dynamic>> GetApprovedPosts()
        {
            return _context.GetApprovedPosts();
        }

        public Task<ReportModel> GetReports()
        {
            return _context.GetReports();
        }

        public Task<IEnumerable<dynamic>> GetUsers()
        {
            return _context.GetUsers();
        }

        public Task<int> ToggleUserStatus(int userId)
        {
            return _context.ToggleUsersStatus(userId);
        }
    }
}
