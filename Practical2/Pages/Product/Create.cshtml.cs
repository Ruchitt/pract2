using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Practical2.Data;
using Practical2.Models;

namespace Practical2
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Name");
            return Page();
        }
        [BindProperty]
        public Product Product { get; set; }
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string filename = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"images\products");
            var extension = Path.GetExtension(file.FileName);
            using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
            {
                file.CopyTo(fileStreams);
            }
            Product.ImageUrl = @"\images\products\" + filename + extension;
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
