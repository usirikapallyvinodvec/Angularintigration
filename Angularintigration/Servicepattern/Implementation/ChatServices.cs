using Angularintigration.Models;
using Angularintigration.RepositryPattern.Interfaces;
using Angularintigration.Servicepattern.Interfaces;

namespace Angularintigration.Servicepattern.Implementation
{
    public class ChatServices : IChatServices
    {
        private readonly IChatRepository _context;

        public ChatServices(
            IChatRepository context)
        {
            _context = context;
        }
        public Task<IEnumerable<dynamic>>
        GetChatHistory(
            int senderId,
            int receiverId)
        {
            return _context.GetChatHistory(
                senderId,
                receiverId);
        }
        public Task<IEnumerable<dynamic>>
        GetUsers(int userId)
        {
            return _context.GetUsers(
                userId);
        }

        public Task<int>
        SaveMessage(
            ChatMessageModel chatIR)
        {
            return _context.SaveMessage(
                chatIR);
        }
    }
}