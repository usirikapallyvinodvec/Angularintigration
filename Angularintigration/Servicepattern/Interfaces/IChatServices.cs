using Angularintigration.Models;

namespace Angularintigration.Servicepattern.Interfaces
{
    public interface IChatServices
    {
        Task<int> SaveMessage(ChatMessageModel chatIR);

        Task<IEnumerable<dynamic>> GetUsers(int userId);

        Task<IEnumerable<dynamic>> GetChatHistory(int senderId,int receiverId);
    }
}