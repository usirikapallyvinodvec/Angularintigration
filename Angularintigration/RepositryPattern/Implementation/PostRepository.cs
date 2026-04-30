using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Dapper;

namespace Angularintigration.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DapperContext _context;
        private readonly IWebHostEnvironment _env;

        public PostRepository(DapperContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IEnumerable<dynamic>> GetHomePosts(int page, int pageSize)
        {
            using var connection = _context.Getconn();

            int offset = (page - 1) * pageSize;

            var postQuery = @"
                    SELECT postid,
                           title,
                           description,
                           username,
                           createddate,
                           ispinned
                    FROM vinod.posts
                    WHERE status='Approved'
                    ORDER BY ispinned DESC, createddate DESC
                    LIMIT @PageSize OFFSET @Offset;";

            var posts = (await connection.QueryAsync(postQuery, new
            {
                PageSize = pageSize,
                Offset = offset
            })).ToList();

            foreach (var post in posts)
            {
                var files = await connection.QueryAsync(@"
                    SELECT fileurl,filetype
                    FROM vinod.postfiles
                    WHERE postid=@PostId", new
                {
                    PostId = post.postid
                });

                post.files = files.ToList();
            }

            return posts;
        }


        public async Task<IEnumerable<dynamic>> GetMyPosts(int userId)
        {
            using var connection = _context.Getconn();

            var query = @"
            SELECT p.postid,
                   p.title,
                   p.description,
                   p.status,
                   p.createddate,
                   f.fileurl,
                   f.filetype
            FROM vinod.posts p
            LEFT JOIN vinod.postfiles f
                   ON p.postid = f.postid
            WHERE p.userid = @UserId
            ORDER BY p.createddate DESC;";

            return await connection.QueryAsync(query, new
            {
                UserId = userId
            });
        }

     
        public async Task<int> AddPost(PostModel model)
        {
            using var connection = _context.Getconn();

            string status = model.IsAnonymous ? "Pending" : "Approved";

            object postData;

           
            if (model.IsAnonymous)
            {
                postData = new
                {
                    UserId = (int?)null,
                    Title = model.Title,
                    Description = model.Description,
                    UserName = "Anonymous",
                    IsAnonymous = true,
                    Status = status
                };
            }
            else
            {
           
                postData = new
                {
                    UserId = model.UserId,
                    Title = model.Title,
                    Description = model.Description,
                    UserName = model.UserName,
                    IsAnonymous = false,
                    Status = status
                };
            }

            var postQuery = @"
            INSERT INTO vinod.posts
            (userid, title, description, username, isanonymous, status)
            VALUES
            (@UserId, @Title, @Description, @UserName, @IsAnonymous, @Status)
            RETURNING postid;";

            int postId = await connection.ExecuteScalarAsync<int>(postQuery, postData);

            
            if (model.Files != null && model.Files.Count > 0)
            {
                string rootPath = _env.WebRootPath;

              
                if (string.IsNullOrEmpty(rootPath))
                {
                    rootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
                }

                string folderPath = Path.Combine(rootPath, "Uploads");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (var file in model.Files)
                {
                    string fileName =
                        Guid.NewGuid().ToString() +
                        Path.GetExtension(file.FileName);

                    string fullPath =
                        Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileQuery = @"
                    INSERT INTO vinod.postfiles
                    (postid, filename, filetype, filepath, fileurl)
                    VALUES
                    (@PostId, @FileName, @FileType, @FilePath, @FileUrl);";

                    await connection.ExecuteAsync(fileQuery, new
                    {
                        PostId = postId,
                        FileName = fileName,
                        FileType = file.ContentType,
                        FilePath = fullPath,
                        FileUrl = "/Uploads/" + fileName
                    });
                }
            }

            return postId;
        }

        public async Task<dynamic> GetPostById(int id)
        {
            using var connection = _context.Getconn();

            var query = @"
            SELECT postid,
                   title,
                   description
            FROM vinod.posts
            WHERE postid = @Id;";

            return await connection.QueryFirstOrDefaultAsync(query, new
            {
                Id = id
            });
        }

       
        public async Task<int> UpdatePost(EditPostModel model)
        {
            using var connection = _context.Getconn();

            var query = @"
            UPDATE vinod.posts
            SET title = @Title,
                description = @Description
            WHERE postid = @PostId
            AND userid = @UserId;";

            return await connection.ExecuteAsync(query, model);
        }

        public async Task<int> DeletePost(DeletePostModel model)
        {
            using var connection = _context.Getconn();

            await connection.ExecuteAsync(
            "delete from vinod.postfiles where postid=@PostId",
            model);

            var query = @"
                    delete from vinod.posts
                    where postid=@PostId
                    and userid=@UserId";

            return await connection.ExecuteAsync(query, model);
        }

        public async Task<IEnumerable<dynamic>> GetPendingPosts()
        {
            using var connection = _context.Getconn();

            var query = @"
                SELECT p.postid,p.title, p.description,p.username, p.createddate, f.fileurl,f.filetype FROM vinod.posts p LEFT JOIN vinod.postfiles f
                                   ON p.postid = f.postid
                            WHERE p.status = 'Pending'
                            ORDER BY p.createddate DESC;";

            return await connection.QueryAsync(query);
        }

        public async Task<int> ApprovedPost(int postId, int moderatorId)
        {
            using var connection = _context.Getconn();

                        var query = @"
                                    UPDATE vinod.posts
                                    SET status='Approved',
                                        approvedby=@ModeratorId
                                    WHERE postid=@PostId;";

            return await connection.ExecuteAsync(query, new
            {
                PostId = postId,
                ModeratorId = moderatorId
            });

        }

        public async Task<int> RejectedPost(int postId, int moderatorId, string reason)
        {
            using var connection = _context.Getconn();
            var query = @"
                    UPDATE vinod.posts
                    SET status='Rejected',
                        rejectedby=@ModeratorId,
                        rejectedreason=@Reason
                    WHERE postid=@PostId;";
            return await connection.ExecuteAsync(query, new
            {
                PostId = postId,
                ModeratorId = moderatorId,
                Reason = reason
            });

        }

        public async Task<IEnumerable<dynamic>> GetPostList()
        {
            using var connection = _context.Getconn();
            var query = @"
                    SELECT p.postid,
                           p.title,
                           p.username,
                           p.status,
                           p.rejectedreason,
                           p.createddate,

                           au.fullname AS approvedbyname,
                           ru.fullname AS rejectedbyname

                    FROM vinod.posts p

                    LEFT JOIN vinod.users au
                           ON p.approvedby = au.userid

                    LEFT JOIN vinod.users ru
                           ON p.rejectedby = ru.userid

                    ORDER BY p.createddate DESC;";

            return await connection.QueryAsync(query);
        }
    }
    }
