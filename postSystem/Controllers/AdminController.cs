using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using postSystem.Models;
using postSystem.Models.Data;
using postSystem.Models.Entities;
using System;
using System.Reflection;

namespace postSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly MasterDBContext _db;

        public AdminController(MasterDBContext db)
        {
            _db = db;
        }

        // ========================
        // AUTH
        // ========================

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var admin = _db.AdminUsers.FirstOrDefault(a => a.Username == username);

            if (admin == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            var hasher = new PasswordHasher<AdminUser>();
            var result = hasher.VerifyHashedPassword(admin, admin.Password, password);

            if (result == PasswordVerificationResult.Success)
            {
                HttpContext.Session.SetString("Admin", admin.Username);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetString("Admin") != null;
        }

        // ========================
        // DASHBOARD
        // ========================

        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var posts = await _db.Posts
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(posts);
        }


        // GET: Admin/CreatePost
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(AddPost model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string imagePath = null;

            if (model.Image != null && model.Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/posts"
                );

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName =
                    Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }

                imagePath = "/uploads/posts/" + uniqueFileName;
            }

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Content = model.Content,
                Category = model.Category,
                ImageUrl = imagePath,
                IsPublished = model.IsPublished,
                CreatedAt = DateTime.UtcNow,
                LikesCount = 0,
                ViewsCount = 0,
                Slug = model.Title
                    .ToLower()
                    .Replace(" ", "-")
                    .Replace(".", "")
                    .Replace(",", "")
            };

            _db.Posts.Add(post);
            _db.SaveChanges();

            return RedirectToAction("Dashboard");
        }


        // ========================
        // COMMENT MODERATION
        // ========================

        public async Task<IActionResult> Comments()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var comments = await _db.Comments
                .Include(c => c.Post)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(comments);
        }

        public async Task<IActionResult> ApproveComment(Guid id)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var comment = await _db.Comments.FindAsync(id);
            if (comment != null)
            {
                comment.IsApproved = true;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Comments");
        }

        public async Task<IActionResult> DeleteComment(Guid id)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var comment = await _db.Comments.FindAsync(id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Comments");
        }
    }
}
