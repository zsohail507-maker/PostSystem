using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using postSystem.Models;
using postSystem.Models.Data;
using postSystem.Models.Entities;
using System;

namespace postSystem.Controllers
{
    public class PostController : Controller
    {
        private readonly MasterDBContext _db;

        public PostController(MasterDBContext db)
        {
            _db = db;
        }

        // SEO URL: /post/{slug}
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            var post = await _db.Posts
                .Include(p => p.Comments.Where(c => c.IsApproved))
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

            if (post == null)
                return NotFound();

            post.ViewsCount++;
            await _db.SaveChangesAsync();

            return View(post);
        }

        // AJAX Like
        [HttpPost]
        [Route("post/like")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Like([FromBody] LikeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Fingerprint))
                return BadRequest();

            var post = await _db.Posts.FindAsync(request.PostId);
            if (post == null)
                return NotFound();

            bool alreadyLiked = await _db.PostLikes.AnyAsync(l =>
                l.PostId == request.PostId &&
                l.UserFingerprint == request.Fingerprint);

            if (!alreadyLiked)
            {
                _db.PostLikes.Add(new PostLike
                {
                    Id = Guid.NewGuid(),
                    PostId = request.PostId,
                    UserFingerprint = request.Fingerprint
                });

                post.LikesCount++;
                await _db.SaveChangesAsync();
            }

            return Ok(new { likes = post.LikesCount });
        }


       

    }


    // DTO for AJAX Like
    public class LikeRequest
    {
        public Guid PostId { get; set; }
        public string Fingerprint { get; set; }
    }



    
}
