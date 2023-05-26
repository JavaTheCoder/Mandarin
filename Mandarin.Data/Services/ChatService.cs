using Mandarin.Data.Data;
using Mandarin.Data.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Mandarin.Data.Services
{
    public class ChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddChat(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
        }

        public void AddMessage(Message message, Chat chat, string username)
        {
            message.From = username;
            message.To = chat.CustomerName == username
                ? chat.OwnerName
                : chat.CustomerName;
            message.Chat = chat;
            message.Date = DateTime.Now;

            _context.Messages.Add(message);

            _context.SaveChanges();
        }

        public Chat GetChatById(int id)
        {
            return _context.Chats.Find(id);
        }

        public async Task<Chat> GetChatByProductId(int id, string customerName,
            string ownerName)
        {
            var chat = _context.Chats.FirstOrDefault(c => c.ProductId == id
                && c.CustomerName == customerName
                && c.OwnerName == ownerName);

            if (chat == null)
            {
                return null;
            }

            return await GetChatWithMessagesById(chat.Id);
        }

        public async Task<Chat> GetChatByUsername(string username)
        {
            return await _context.Chats.FirstOrDefaultAsync(
                c => c.OwnerName == username || c.CustomerName == username);
        }

        public async Task<List<Chat>> GetAllUserChats(string username)
        {
            var chats = await _context.Chats
                 .Where(c => c.OwnerName == username
                 || c.CustomerName == username)
                 .ToListAsync();

            foreach (var chat in chats)
            {
                chat.Product = _context.Products.Find(chat.ProductId);
                chat.Messages = _context.Messages
                    .Where(m => m.ChatId == chat.Id).ToList();
            }

            return chats;
        }

        public async Task<Chat> GetChatWithMessagesById(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            chat.Messages = await _context.Messages
                .Where(m => m.ChatId == id)
                .ToListAsync();
            chat.Product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(c => c.Id == chat.ProductId);
            return chat;
        }
    }
}
