using Microsoft.AspNetCore.Mvc;
using postSystem.Models.Data;
using postSystem.Models.Entities;
using System;

namespace postSystem.Controllers
{
    public class CommentController : Controller
    {
        private readonly MasterDBContext _db;

        public CommentController(MasterDBContext db)
        {
            _db = db;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid postId, string userName, string content)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(content))
                return RedirectToAction("Index", "Home");

            var post = await _db.Posts.FindAsync(postId);
            if (post == null)
                return NotFound();

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                UserName = userName.Trim(),
                Content = content.Trim(),
                IsApproved = true, 
                CreatedAt = DateTime.UtcNow
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Post", new { slug = post.Slug });
        }
    }
}
