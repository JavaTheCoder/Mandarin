using System.ComponentModel.DataAnnotations.Schema;

namespace Mandarin.Data.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public string OwnerName { get; set; }

        public string CustomerName { get; set; }

        public int ProductId { get; set; }

        [NotMapped]
        public Product Product { get; set; }

        public List<Message> Messages = new List<Message>();
    }
}
