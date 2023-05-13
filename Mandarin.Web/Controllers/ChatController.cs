using Mandarin.Data.Data;
using Mandarin.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mandarin.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> ShowAllChats()
        {
            string? username = _userManager.GetUserName(User);
            var chats = await _context.Chats
                .Where(c => c.OwnerName == username
                || c.CustomerName == username)
                .ToListAsync();
            return View("Chat", chats);
        }
    }
}
