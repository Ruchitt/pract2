using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Practical2.Data;
using Practical2.Models;

namespace Practical2.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Models.Category> Categories { get; set; }
        public IEnumerable<Models.Product> Products { get; set; }
        public void OnGet(string[]? categoryNames, string? prodname)
        {
            if (categoryNames != null && categoryNames.Length > 0)
            {
                Categories = _db.Categories.Where(c => categoryNames.Contains(c.Name)).ToList();
                Products = _db.Products;
            }
            else
            {
                if (!string.IsNullOrEmpty(prodname))
                {
                    ViewData["productName"] = prodname;
                    Categories = _db.Products
                        .Include(p => p.Category)
                        .Where(p => p.Name == prodname)
                        .Select(p => p.Category)
                        .ToList();
                    Products = _db.Products.ToList();
                }
                else
                {
                    Categories = _db.Categories;
                    Products = _db.Products.ToList();
                }
            }
        }
        public async Task<IActionResult> OnPost(string searchText)
        {
            var prodstring = searchText.Split(',');
            return RedirectToPage("/Product/Index", new { prodNames = prodstring });
        }
    }
}
