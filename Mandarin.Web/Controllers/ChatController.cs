using Mandarin.Data.Models;
using Mandarin.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

/*
    1. Product Categorization: Organize tech products into categories such as computers, smartphones, audio devices, etc. This allows users to easily find what they're looking for.
    2. Advanced Search: Implement a robust search functionality with filters for brand, price range, specifications, and other relevant attributes to help users quickly find specific products.
    3. Detailed Product Listings: Include comprehensive product descriptions, specifications, high-quality images, and customer reviews to provide users with all the necessary information to make informed purchasing decisions.
    4. User Reviews and Ratings: Allow customers to leave reviews and ratings for products they have purchased. This helps build trust and assists other users in making purchasing decisions.
 */

#nullable disable
namespace Mandarin.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;
        private readonly ProductService _productService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ChatService chatService,
            ProductService productService,
            UserManager<ApplicationUser> userManager)
        {
            _chatService = chatService;
            _productService = productService;
            _userManager = userManager;
        }

        public async Task<ActionResult<List<Chat>>> ShowAllChats()
        {
            string username = _userManager.GetUserName(User);
            var chats = await _chatService.GetAllUserChats(username);
            return View("Chats", chats);
        }

        public async Task<IActionResult> OpenChat(int id)
        {
            var chat = await _chatService.GetChatWithMessagesById(id);

            var tuple = (chat, new Message());
            return View("Chat", tuple);
        }

        public async Task<IActionResult> OpenOrCreateChat(int id)
        {
            TempData["ProductId"] = id;
            string username = _userManager.GetUserName(User);

            var product = _productService.GetProductWithCategory(id);
            var user = await _userManager.FindByIdAsync(product.UserId);
            string ownerName = user.UserName;

            var chat = await _chatService.GetChatByProductId(id, username, ownerName);

            if (chat is null)
            {
                chat = new Chat
                {
                    Product = product,
                    ProductId = product.Id,
                    OwnerName = ownerName,
                    CustomerName = username
                };

                await _chatService.AddChat(chat);
                return View("Chat", (chat, new Message()));
            }

            chat.Product = product;
            return View("Chat", (chat, new Message()));
        }

        public IActionResult AddMessage(string msg)
        {
            TempData.TryGetValue("ChatId", out var id);
            var chat = _chatService.GetChatById((int)id);

            string username = _userManager.GetUserName(User);

            var message = new Message { Text = msg, ChatId = (int)id };
            _chatService.AddMessage(message, chat, username);

            return RedirectToAction("OpenChat", new { id = message.ChatId });
        }
    }
}
