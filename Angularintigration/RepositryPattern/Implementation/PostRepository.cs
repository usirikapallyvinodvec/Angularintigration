using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Angularintigration.Servicepattern.Interfaces;
using Dapper;

namespace Angularintigration.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DapperContext _context;
        private readonly ISftpService _sftpService;

        private const string BaseUrl = "http://172.17.32.216";

        public PostRepository(DapperContext context, ISftpService sftpService)
        {
            _context = context;
            _sftpService = sftpService;
        }

        public async Task<int> AddPost(PostModel model)
        {
            using var connection = _context.Getconn();

            string status = model.IsAnonymous ? "Pending" : "Approved";

            var postId = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO vinod.posts
                (userid,title,description,username,isanonymous,status)
                VALUES
                (@UserId,@Title,@Description,@UserName,@IsAnonymous,@Status)
                RETURNING postid;",
                new
                {
                    UserId = model.IsAnonymous ? (int?)null : model.UserId,
                    model.Title,
                    model.Description,
                    UserName = model.IsAnonymous ? "Anonymous" : model.UserName,
                    model.IsAnonymous,
                    Status = status
                });

            if (model.Files != null && model.Files.Count > 0)
            {
                foreach (var file in model.Files)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                    string remotePath = await _sftpService.UploadFile(file, fileName);

                    string fileUrl = $"{BaseUrl}/uploads/{fileName}";

                    await connection.ExecuteAsync(@"
                        INSERT INTO vinod.postfiles
                        (postid, filename, filetype, filepath, fileurl)
                        VALUES
                        (@PostId,@FileName,@FileType,@FilePath,@FileUrl);",
                        new
                        {
                            PostId = postId,
                            FileName = fileName,
                            FileType = file.ContentType,
                            FilePath = remotePath,
                            FileUrl = fileUrl
                        });
                }
            }

            return postId;
        }

        public async Task<IEnumerable<dynamic>> GetHomePosts(int page, int pageSize)
        {
            using var connection = _context.Getconn();

            var posts = (await connection.QueryAsync<dynamic>(@"
                SELECT postid,title,description,username,createddate,ispinned
                FROM vinod.posts
                WHERE status='Approved'
                ORDER BY ispinned DESC, createddate DESC
                LIMIT @PageSize OFFSET @Offset;",
                new { PageSize = pageSize, Offset = (page - 1) * pageSize })).ToList();

            foreach (var post in posts)
            {
                var files = await connection.QueryAsync(@"
                    SELECT fileurl,filetype
                    FROM vinod.postfiles
                    WHERE postid=@PostId",
                    new { PostId = post.postid });

                post.files = files.ToList();
            }

            return posts;
        }

        public async Task<IEnumerable<dynamic>> GetMyPosts(int userId)
        {
            using var connection = _context.Getconn();

            var postDict = new Dictionary<int, dynamic>();

            var result = await connection.QueryAsync(@"
                SELECT p.postid, p.title, p.description, p.status, p.createddate,
                       f.fileurl, f.filetype
                FROM vinod.posts p
                LEFT JOIN vinod.postfiles f ON p.postid = f.postid
                WHERE p.userid = @UserId
                ORDER BY p.createddate DESC;",
                new { UserId = userId });

            foreach (var row in result)
            {
                if (!postDict.TryGetValue((int)row.postid, out var post))
                {
                    post = new
                    {
                        postid = row.postid,
                        title = row.title,
                        description = row.description,
                        status = row.status,
                        createddate = row.createddate,
                        files = new List<dynamic>()
                    };

                    postDict.Add((int)row.postid, post);
                }

                if (row.fileurl != null)
                {
                    post.files.Add(new
                    {
                        fileurl = row.fileurl,
                        filetype = row.filetype
                    });
                }
            }

            return postDict.Values;
        }

        public async Task<dynamic> GetPostById(int id)
        {
            using var connection = _context.Getconn();

            return await connection.QueryFirstOrDefaultAsync(@"
                SELECT postid,title,description
                FROM vinod.posts
                WHERE postid=@Id", new { Id = id });
        }

        public async Task<int> UpdatePost(EditPostModel model)
        {
            using var connection = _context.Getconn();

            return await connection.ExecuteAsync(@"
                UPDATE vinod.posts
                SET title=@Title, description=@Description
                WHERE postid=@PostId AND userid=@UserId;", model);
        }

        public async Task<int> DeletePost(DeletePostModel model)
        {
            using var connection = _context.Getconn();

            await connection.ExecuteAsync(
                "DELETE FROM vinod.postfiles WHERE postid=@PostId", model);

            return await connection.ExecuteAsync(
                "DELETE FROM vinod.posts WHERE postid=@PostId AND userid=@UserId", model);
        }

        public async Task<IEnumerable<dynamic>> GetPendingPosts()
        {
            using var connection = _context.Getconn();

            var postDict = new Dictionary<int, dynamic>();

            var result = await connection.QueryAsync(@"
                SELECT p.postid, p.title, p.description, p.username, p.createddate,
                       f.fileurl, f.filetype
                FROM vinod.posts p
                LEFT JOIN vinod.postfiles f ON p.postid = f.postid
                WHERE p.status = 'Pending'
                ORDER BY p.createddate DESC;");

            foreach (var row in result)
            {
                if (!postDict.TryGetValue((int)row.postid, out var post))
                {
                    post = new
                    {
                        postid = row.postid,
                        title = row.title,
                        description = row.description,
                        username = row.username,
                        createddate = row.createddate,
                        files = new List<dynamic>()
                    };

                    postDict.Add((int)row.postid, post);
                }

                if (row.fileurl != null)
                {
                    post.files.Add(new
                    {
                        fileurl = row.fileurl,
                        filetype = row.filetype
                    });
                }
            }

            return postDict.Values;
        }

        public async Task<int> ApprovedPost(int postId, int moderatorId)
        {
            using var connection = _context.Getconn();

            return await connection.ExecuteAsync(@"
                UPDATE vinod.posts
                SET status='Approved', approvedby=@ModeratorId
                WHERE postid=@PostId;",
                new { PostId = postId, ModeratorId = moderatorId });
        }

        public async Task<int> RejectedPost(int postId, int moderatorId, string reason)
        {
            using var connection = _context.Getconn();

            return await connection.ExecuteAsync(@"
                UPDATE vinod.posts
                SET status='Rejected', rejectedby=@ModeratorId, rejectedreason=@Reason
                WHERE postid=@PostId;",
                new { PostId = postId, ModeratorId = moderatorId, Reason = reason });
        }

        public async Task<IEnumerable<dynamic>> GetPostList()
        {
            using var connection = _context.Getconn();

            var posts = (await connection.QueryAsync<dynamic>(@"
                SELECT p.postid,p.title,p.username,p.status,
                       p.rejectedreason,p.createddate,
                       au.fullname AS approvedbyname,
                       ru.fullname AS rejectedbyname
                FROM vinod.posts p
                LEFT JOIN vinod.users au ON p.approvedby = au.userid
                LEFT JOIN vinod.users ru ON p.rejectedby = ru.userid
                ORDER BY p.createddate DESC;")).ToList();

            foreach (var post in posts)
            {
                var files = await connection.QueryAsync(@"
                    SELECT fileurl,filetype
                    FROM vinod.postfiles
                    WHERE postid=@PostId",
                    new { PostId = post.postid });

                post.files = files.ToList();
            }

            return posts;
        }
    }
}