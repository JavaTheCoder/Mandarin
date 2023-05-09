namespace Mandarin.Data.Models
{
    public class FavoriteProduct
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public Product Product { get; set; }
    }
}
