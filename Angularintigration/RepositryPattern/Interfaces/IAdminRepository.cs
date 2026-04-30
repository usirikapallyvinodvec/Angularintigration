using Angularintigration.Models;

namespace Angularintigration.RepositryPattern.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<dynamic>> GetUsers();
        Task<int> ChangeRole(int userId, int roleId);
        Task<int> ToggleUsersStatus(int userId);
        Task<IEnumerable<dynamic>> GetApprovedPosts();
        Task<int> DeleteAnyPost(int postId);
        Task<ReportModel> GetReports();
    }
}
