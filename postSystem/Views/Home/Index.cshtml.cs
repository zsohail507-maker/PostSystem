using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using postSystem.Models.Data;
using postSystem.Models.Entities;

namespace postSystem.Views.Home
{
    public class IndexModel : PageModel
    {
        private readonly MasterDBContext _context;

        public IndexModel(MasterDBContext context)
        {
            _context = context;
        }

        public List<Post> Posts { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Post> query = _context.Posts
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.CreatedAt);

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(p =>
                    p.Title.Contains(Search) ||
                    p.Category.Contains(Search));
            }

            Posts = await query.ToListAsync();
        }
    }
}
