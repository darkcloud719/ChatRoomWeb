using System.Linq;
using System.Threading.Tasks;
using ChatRoomWeb.Database;
using ChatRoomWeb.Hubs;
using ChatRoomWeb.Infrastructure;
using ChatRoomWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomWeb.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IChatRepository _repo;
        private ChatRoomDbContext _ctx;

        public HomeController(IChatRepository repo, ChatRoomDbContext ctx)
        {
            _repo = repo;
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            var chats = _repo.GetChats(GetUserId());

            return View(chats);
        }


        public IActionResult Find([FromServices] ChatRoomDbContext ctx)
        {
            var users = ctx.Users
                .Where(x => x.Id!=User.GetUserId())
                .ToList();

            return View(users);
        }

        public IActionResult Private()
        {
            var chats = _repo.GetPrivateChats(GetUserId());

            return View(chats);
        }

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            return View(_repo.GetChat(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            await _repo.CreateRoom(name, GetUserId());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            await _repo.JoinRoom(id, GetUserId());
            return RedirectToAction("Chat", "Home" , new { id = id});
        }

        public async Task<IActionResult> SendMessage(int roomId, string message, [FromServices] IHubContext<ChatHub> chat)
        {
            var Message = await _repo.CreateMessage(roomId, message, User.Identity.Name);

            await chat.Clients.Group(roomId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.Name,
                    TimeStamp = Message.Timestamp.ToString("yyyy/MM/d tt hh:mm:ss")
                });

            return Ok();
        }

        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            var id = await _repo.CreatePrivateRoom(GetUserId(), userId);
            return RedirectToAction("Chat", new{id});
        }

    }
}