using ChatRoomWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomWeb.Database
{
    public class ChatRoomDbContext : IdentityDbContext<User>
    {
        public ChatRoomDbContext(DbContextOptions<ChatRoomDbContext> options):base(options)
        {

        }

        public DbSet<Chat> Chats {get;set;}
        public DbSet<Message> Messages {get;set;}
        public DbSet<ChatUser> ChatUsers{get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ChatUser>()
                .HasKey(x => new{ x.ChatId, x.UserId});
        }
    }

    
}