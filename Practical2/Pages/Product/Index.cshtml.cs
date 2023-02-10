using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Practical2.Data;
using Practical2.Models;

namespace Practical2
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<Product> Product { get;set; }

        public async Task OnGetAsync(string[]? prodNames, string? categoryname)
        {

            if (prodNames != null && prodNames.Length > 0)
            {
                Product = _db.Products.Where(c => prodNames.Contains(c.Name)).Include(p=>p.Category).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(categoryname))
                {
                    ViewData["catName"] = categoryname;
                    Product = _db.Products.Where(c => categoryname.Contains(c.Category.Name)).Include(p => p.Category).ToList(); ;
                }
                else 
                {
                    Product = await _db.Products
                    .Include(p => p.Category).ToListAsync();
                }
            }
        }
        public async Task<IActionResult> OnPost(string searchText)
        {
            var categories = searchText.Split(',');
            return RedirectToPage("/Category/Index", new { categoryNames = categories });
        }
    }
}
