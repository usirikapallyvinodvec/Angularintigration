using Angularintigration.Models;

namespace Angularintigration.RepositryPattern.Interfaces
{
    public interface IChatRepository
    {
        Task<int> SaveMessage(ChatMessageModel chatIR);

        Task<IEnumerable<dynamic>> GetUsers(int userId);

        Task<IEnumerable<dynamic>> GetChatHistory(int senderId,int receiverId);
    }
}