using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using postSystem.Models;
using postSystem.Models.Data;
using postSystem.Models.Entities;
using postSystem.Views.Home;
using System;

namespace postSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly MasterDBContext _db;

        public HomeController(MasterDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts
                        .Where(p => p.IsPublished)
                        .OrderByDescending(p => p.CreatedAt)
                        .ToListAsync();

            var categories = await _db.Posts
                .Where(p => p.IsPublished)
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(
                    x => x.Category,
                    x => x.Count
                );

            var dto = new HomepageDTO
            {
                Posts = posts,
                Categories = categories
            };

            return View(dto);

        }


        [HttpGet]
        public async Task<IActionResult> CategorizedPosts(string category, string? search)
        {
            if (string.IsNullOrWhiteSpace(category))
                category.Trim();

            IQueryable<Post> query = _db.Posts
                .Where(p => p.IsPublished && p.Category == category);

            // 🔍 SEARCH WITHIN CATEGORY
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Title.Contains(search) ||
                    p.Content.Contains(search));
            }

            var posts = await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            // You are already using IndexModel in the view, so we keep it
            IndexModel model = new(_db);
            model.Posts = posts;
            model.Search = search;

            return View(model);
        }


        [HttpPost]
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                searchTerm.Trim();
            return RedirectToAction("CategorizedPosts", new { category = searchTerm });
        }


    }
}
