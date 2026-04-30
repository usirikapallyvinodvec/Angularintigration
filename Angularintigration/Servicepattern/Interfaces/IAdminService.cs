using Angularintigration.Models;

namespace Angularintigration.Servicepattern.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<dynamic>> GetUsers();

        Task<int> ChangeRole(int userId, int roleId);

        Task<int> ToggleUserStatus(int userId);
        Task<IEnumerable<dynamic>> GetApprovedPosts();

        Task<int> DeleteAnyPost(int postId);
        Task<ReportModel> GetReports();
    }
}
