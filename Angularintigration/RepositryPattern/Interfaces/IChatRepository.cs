using Angularintigration.Models;

namespace Angularintigration.RepositryPattern.Interfaces
{
    public interface IChatRepository
    {
        Task<int> SaveMessage(ChatMessageModel chatIR);
        Task<IEnumerable<dynamic>> GetUsers(int userId);
        Task<IEnumerable<dynamic>> getChatHistory(int senderId,int receiverId);
    }
}
