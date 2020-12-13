using System.Collections.Generic;
using System.Threading.Tasks;
using ChatRoomWeb.Models;

namespace ChatRoomWeb.Repository
{
    public interface IChatRepository
    {
        Chat GetChat(int id);

        Task CreateRoom(string name, string userId);

        IEnumerable<Chat> GetChats(string userId);

        Task<int> CreatePrivateRoom(string rootId, string targetId);

        IEnumerable<Chat> GetPrivateChats(string userId);

        Task JoinRoom(int chatId, string userId);

        Task<Message> CreateMessage(int chatId, string message, string userId);
    }
}