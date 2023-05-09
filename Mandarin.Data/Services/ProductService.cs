using Mandarin.Data.Data;
using Mandarin.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Mandarin.Data.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public Task<List<Product>> GetAllProductsWithCategories()
        {
            return _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public IEnumerable<SelectListItem> GetCategoriesList()
        {
            return _context.Categories.Select(
                c => new SelectListItem(c.Name, $"{c.Id}"));
        }

        public async Task<List<Product>> GetFilteredProducts(string name)
        {
            var productsList = await GetAllProductsWithCategories();
            return productsList
                .Where(p => p.Name.Contains(name,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Find(id);
        }

        public async Task<List<Product>> GetProductsByCategoryId(int id)
        {
            return await _context.Products
                .Where(p => p.CategoryId == id)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public object GetProductWithCategoryById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
        }

        public async Task UpdateProduct(int id, Product newProduct)
        {
            var product = GetProductById(id);
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.Image = newProduct.Image;
            product.Category = newProduct.Category;
            product.Description = newProduct.Description;

            await _context.SaveChangesAsync();
        }
    }
}
