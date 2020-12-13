using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChatRoomWeb.Models
{
    public class User : IdentityUser
    {
        public ICollection<ChatUser> Chats{get;set;}
    }
}