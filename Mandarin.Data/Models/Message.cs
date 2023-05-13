namespace Mandarin.Data.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public DateTime Date { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }
    }
}