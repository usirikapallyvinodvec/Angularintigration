using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Dapper;

namespace Angularintigration.RepositryPattern.Implementation
{
    public class ChatRepository : IChatRepository
    {
        private readonly DapperContext _context;

        public ChatRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<dynamic>> getChatHistory(int senderId, int receiverId)
        {
            using var connection = _context.Getconn();

            var query = @"
            SELECT *
            FROM vinod.chatmessages
            WHERE
            (senderuserid=@SenderId
            AND receiveruserid=@ReceiverId)

            OR

            (senderuserid=@ReceiverId
            AND receiveruserid=@SenderId)

            ORDER BY sentdate;";
            return await connection.QueryAsync(
               query,
               new
               {
                   SenderId = senderId,
                   ReceiverId = receiverId
               });
        }


        public async Task<IEnumerable<dynamic>> GetUsers(int userId)
        {
            using var connection =
                _context.Getconn();

            var query = @"
            SELECT userid,
                   fullname,
                   roleid
            FROM vinod.users
            WHERE userid <> @UserId
            AND isactive = true;";

            return await connection.QueryAsync(
                query,
                new { UserId = userId });
        }

        public async Task<int> SaveMessage(ChatMessageModel chatIR)
        {
            using var connection =
                _context.Getconn();

            var query = @"
            INSERT INTO vinod.chatmessages
            (senderuserid,
             receiveruserid,
             message)
            VALUES
            (@SenderUserId,
             @ReceiverUserId,
             @Message);";

            return await connection.ExecuteAsync(
                query, chatIR);
        }
    }
}
