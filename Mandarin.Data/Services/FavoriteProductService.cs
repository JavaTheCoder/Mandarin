using Mandarin.Data.Data;
using Mandarin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Mandarin.Data.Services
{
    public class FavoriteProductService
    {
        private readonly ApplicationDbContext _context;

        public FavoriteProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetFavoriteProducts(string username)
        {
            var favorites = await _context.Favorites
                .Where(f => f.UserName == username)
                .Select(f => f.Product.Id)
                .ToListAsync();

            var products = await _context.Products
                .Where(p => favorites.Contains(p.Id))
                .ToListAsync();

            return products;
        }

        public void AddOrRemoveFavorites(int id, string username)
        {
            var product = _context.Products.Find(id);

            if (!string.IsNullOrEmpty(username))
            {
                var favorite = _context.Favorites
                    .FirstOrDefault(f => f.Product.Id == product.Id);

                if (favorite == null)
                {
                    favorite = new FavoriteProduct
                    {
                        UserName = username,
                        Product = product
                    };

                    _context.Favorites.Add(favorite);
                }
                else
                {
                    _context.Favorites.Remove(favorite);
                }

                _context.SaveChanges();
            }
        }
    }
}
