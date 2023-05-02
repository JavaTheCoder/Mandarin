using AutoMapper;
using Elfie.Serialization;
using Mandarin.Web.Data;
using Mandarin.Web.Helpers;
using Mandarin.Web.Models;
using Mandarin.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

// TODO: user should have phone number
// TODO: add email sender to confirm email when registering
namespace Mandarin.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = MapperConfig.GetMapper();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public ActionResult<Product> Create()
        {
            var productVM = new ProductVM();
            productVM.CategoriesList = _context.Categories.Select(c =>
                new SelectListItem(c.Name, $"{c.Id}"));
            productVM.UserId = _userManager.GetUserId(User);

            return View(productVM);
        }

        //var product = new Product
        //{
        //    Name = productVM.Name,
        //    CategoryId = productVM.CategoryId,
        //    Description = productVM.Description,
        //    Image = productVM.Image,
        //    Price = productVM.Price,
        //    UserId = productVM.UserId
        //};
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            var product = _mapper.Map<ProductVM, Product>(productVM);
            _context.Products.Add(product);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult<Product> Update(int id)
        {
            var product = _context.Products.Find(id);
            if (product.UserId == _userManager.GetUserId(User))
            {
                return View(product);
            }

            return Unauthorized();
        }

        [Authorize]
        public async Task<IActionResult> Update(Product newProduct, int id)
        {
            var product = _context.Products.Find(id);
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.Image = newProduct.Image;
            product.Category = newProduct.Category;
            product.Description = newProduct.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            product.Category = _context.Categories.Find(product.CategoryId);
            return View(product);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}