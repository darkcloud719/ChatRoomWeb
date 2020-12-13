using System.Threading.Tasks;
using ChatRoomWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomWeb.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _singInManager;
        private UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _singInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user!=null)
            {
                var result = await _singInManager.PasswordSignInAsync(user, password, false, false);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
            }

            return RedirectToAction("Login","Account");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new User
            {
                UserName = username
            };

            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                await _singInManager.SignInAsync(user,false);
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Register","Account");
        }

        public async Task<IActionResult> Logout()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
    }
}