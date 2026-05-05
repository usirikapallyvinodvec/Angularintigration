using Angularintigration.Models;

namespace Angularintigration.RepositryPattern.Interfaces
{
    public interface IPostRepository
    {
        Task<int> AddPost(PostModel postModel);
        Task<IEnumerable<dynamic>> GetHomePosts(int page, int pageSize);
        Task<IEnumerable<dynamic>> GetMyPosts(int userId);
        Task<dynamic> GetPostById(int id);
        Task<int> UpdatePost(EditPostModel model);
        Task<int> DeletePost(DeletePostModel model);
        Task<IEnumerable<dynamic>> GetPendingPosts();
        Task<int> ApprovedPost(int postId, int moderatorId);
        Task<int> RejectedPost(int postId, int moderatorId,string reason);
        Task<IEnumerable<dynamic>> GetPostList();
    }
}