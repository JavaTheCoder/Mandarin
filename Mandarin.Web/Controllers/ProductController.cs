using AutoMapper;
using Mandarin.Data.Helpers;
using Mandarin.Data.Models;
using Mandarin.Data.Services;
using Mandarin.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

// TODO: LAST ONE | add email sender to confirm email when registering
namespace Mandarin.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ProductService _productService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FavoriteProductService _favoritesService;

        public ProductController(UserManager<ApplicationUser> userManager,
            ProductService productService,
            FavoriteProductService favoritesService)
        {
            _userManager = userManager;
            _productService = productService;
            _favoritesService = favoritesService;
            _mapper = MapperConfig.GetMapper();
        }

        public async Task<ActionResult<List<Product>>> Index()
        {
            string? username = _userManager.GetUserName(User);
            var products = await _favoritesService.GetFavoriteProducts(username);

            TempData["FavoriteProductsIds"] = products.Select(p => p.Id).ToList();
            ViewBag.CategoriesList = _productService.GetCategoriesList();

            var tuple = (await _productService.GetAllProductsWithCategories(), new ProductVM());
            return View(tuple);
        }

        public async Task<ActionResult<List<Product>>> ShowByCategory(int id)
        {
            string? username = _userManager.GetUserName(User);
            var products = await _favoritesService.GetFavoriteProducts(username);

            TempData["FavoriteProductsIds"] = products.Select(p => p.Id).ToList();
            ViewBag.CategoriesList = _productService.GetCategoriesList();

            var tuple = (await _productService.GetProductsByCategoryId(id), new ProductVM());
            return View("Index", tuple);
        }

        public async Task<IActionResult> SearchByName(ProductVM result)
        {
            if (string.IsNullOrEmpty(result.Name))
            {
                return RedirectToAction("Index");
            }

            string? username = _userManager.GetUserName(User);
            var products = await _favoritesService.GetFavoriteProducts(username);

            TempData["FavoriteProductsIds"] = products.Select(p => p.Id).ToList();
            ViewBag.CategoriesList = _productService.GetCategoriesList();

            var tuple = (await _productService.GetFilteredProducts(result.Name), result);
            return View("Index", tuple);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<ProductVM> Create()
        {
            var productVM = new ProductVM
            {
                UserId = _userManager.GetUserId(User),
                CategoriesList = _productService.GetCategoriesList()
            };

            return View(productVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            var product = _mapper.Map<ProductVM, Product>(productVM);
            await _productService.AddProduct(product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult<Product> Update(int id)
        {
            var product = _productService.GetProductById(id);
            if (product?.UserId == _userManager.GetUserId(User))
            {
                return View(product);
            }

            return Unauthorized();
        }

        [Authorize]
        public async Task<IActionResult> Update(Product newProduct, int id)
        {
            await _productService.UpdateProduct(id, newProduct);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult<Product> Details(int id)
        {
            var product = _productService.GetProductWithCategoryById(id);
            return View(product);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductById(id);
            _productService.DeleteProduct(product);

            return RedirectToAction("Index");
        }

        public IActionResult AddOrRemoveFavorites(int id)
        {
            string? username = _userManager.GetUserName(User);
            _favoritesService.AddOrRemoveFavorites(id, username);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult<List<Product>>> GetFavoriteProducts()
        {
            string? username = _userManager.GetUserName(User);
            var products = await _favoritesService.GetFavoriteProducts(username);

            return View("Favorites", products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
            });
        }
    }
}