using Angularintigration.Models;

namespace Angularintigration.Servicepattern.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<dynamic>> GetHomePosts(int page, int pageSize);

        Task<int> AddPost(PostModel model);

        Task<IEnumerable<dynamic>> GetMyPosts(int userId);

        Task<dynamic> GetPostById(int id);

        Task<int> UpdatePost(EditPostModel model);
        Task<int> DeletePost(DeletePostModel model);
        Task<IEnumerable<dynamic>> GetPendingPosts();

        Task<int> ApprovePost(int postId, int moderatorId);

        Task<int> RejectPost(int postId, int moderatorId, string reason);
        Task<IEnumerable<dynamic>> GetPostList();
    }
}