using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoomWeb.Database;
using ChatRoomWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomWeb.Repository
{
    public class ChatRepository : IChatRepository
    {
        private ChatRoomDbContext _ctx;
        
        public ChatRepository(ChatRoomDbContext ctx) => _ctx = ctx;

        public async Task<int> CreatePrivateRoom(string rootId, string targetId)
        {
            var A_ChatId_List = _ctx.ChatUsers
                .Include(x => x.Chat)
                .Where(x => x.UserId == targetId && x.Chat.Type == ChatType.Private)
                .Select(x => x.ChatId);
            
            var B_ChatId_List = _ctx.ChatUsers
                .Include(x => x.Chat)
                .Where(x => x.UserId == rootId && x.Chat.Type == ChatType.Private)
                .Select(x => x.ChatId);

            bool havingPrivateRoomAlready = A_ChatId_List.Intersect(B_ChatId_List).Count() > 0 ? true : false;

            if(havingPrivateRoomAlready)
            {
                int chatId = A_ChatId_List.Intersect(B_ChatId_List).FirstOrDefault();

                return chatId;
            }
            else
            {
                var chat = new Chat
                {
                    Type = ChatType.Private
                };

                chat.Users.Add(new ChatUser
                {
                    UserId = targetId
                });

                chat.Users.Add(new ChatUser
                {
                    UserId = rootId
                });

                _ctx.Chats.Add(chat);
                await _ctx.SaveChangesAsync();

                return chat.Id;
                
            }
        }
        public async Task<Message> CreateMessage(int chatId, string message, string userId)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = userId,
                Timestamp = DateTime.Now
            };

            _ctx.Messages.Add(Message);
            await _ctx.SaveChangesAsync();
            
            return Message;
        }

        public async Task CreateRoom(string name, string userId)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
                Role = UserRole.Admin
            });

            _ctx.Chats.Add(chat);
            await _ctx.SaveChangesAsync();
        }

        public Chat GetChat(int id)
        {
            return _ctx.Chats
                    .Include(x => x.Message)
                    .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.User)
                .Where(x => x.Users.All(y => y.UserId != userId && x.Type == Models.ChatType.Room))
                .ToList();
        }

        public IEnumerable<Chat> GetPrivateChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.User)
                .Where(x => x.Type == ChatType.Private && x.Users.Any(y => y.UserId == userId))
                .ToList();
        }

        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };

            _ctx.ChatUsers.Add(chatUser);
            await _ctx.SaveChangesAsync();
        }
    }
}