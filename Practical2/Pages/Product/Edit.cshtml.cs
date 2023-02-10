using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practical2.Data;
using Practical2.Models;

namespace Practical2
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditModel(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _db.Products == null)
            {
                return NotFound();
            }
            var product =  await _db.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
           ViewData["CategoryId"] = new SelectList(_db.Set<Category>(), "CategoryId", "Name");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id,IFormFile? file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (id != null)
            {
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    if (Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    Product.ImageUrl = @"\images\products\" + filename + extension;
                }
                _db.Products.Any(e => e.ProductId == id);
                _db.Products.Update(Product);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
