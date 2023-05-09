using Mandarin.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mandarin.Data.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public IEnumerable<SelectListItem> CategoriesList { get; set; }
    }
}
