using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Angularintigration.Servicepattern.Interfaces;

namespace Angularintigration.Servicepattern.Implementation
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _context;

        public PostService(IPostRepository context)
        {
            _context = context;
        }

        public Task<int> AddPost(PostModel model)
        {
            return _context.AddPost(model);
        }

        public Task<int> ApprovePost(int postId, int moderatorId)
        {
            return _context.ApprovedPost(postId, moderatorId);
        }

        public Task<int> DeletePost(DeletePostModel model)
        {

            return _context.DeletePost(model);
        
        }

        public Task<IEnumerable<dynamic>> GetHomePosts(int page, int pageSize)
        {
            return _context.GetHomePosts(page, pageSize);
        }

        public Task<IEnumerable<dynamic>> GetMyPosts(int userId)
        {
            return _context.GetMyPosts(userId);
        }

        public Task<IEnumerable<dynamic>> GetPendingPosts()
        {
            return _context.GetPendingPosts();
        }

        public Task<dynamic> GetPostById(int id)
        {
            return _context.GetPostById(id);
        }

        public Task<IEnumerable<dynamic>> GetPostList()
        {
            return _context.GetPostList();
        }

        public Task<int> RejectPost(int postId, int moderatorId, string reason)
        {
            return _context.RejectedPost(postId, moderatorId, reason);
        }

        public Task<int> UpdatePost(EditPostModel model)
        {
            return _context.UpdatePost(model);
        }

    }
}