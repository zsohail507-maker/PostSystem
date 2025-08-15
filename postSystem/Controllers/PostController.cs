using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using postSystem.Models;
using postSystem.Models.Data;
using postSystem.Models.Entities;

namespace postSystem.Controllers
{
    public class PostController : Controller
    {
        private readonly MasterDBContext _masterDBContext;

        public PostController(MasterDBContext context)
        {
            _masterDBContext = context;
        }

        // Get by id
        public IActionResult ViewPost(Guid id)
        {
            Posts? post = _masterDBContext.Posts
        .Include(p => p.Comments)
        .Include(p => p.User)
        .FirstOrDefault(p => p.Id == id);

            return View(post);
        }

        // Get all posts
        public IActionResult Dashboard()
        {
            Users? user = _masterDBContext.Users.FirstOrDefault(u => u.Email == HttpContext.Session.GetString("email"));
            ViewBag.Username = user.Name;

            List<Posts> posts = _masterDBContext.Posts.ToList<Posts>();

            return View(posts);
        }

        // Add a new post
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPost(AddPost post)
        {
            if (post.name == null)
            {
                ModelState.AddModelError(nameof(post.name), "Title cannot be null");
                return View(post);
            }
            if (post.description == null)
            {
                ModelState.AddModelError(nameof(post.description), "Content cannot be null");
                return View(post);
            }

            Posts newPost = new Posts();

            newPost.name = post.name;
            newPost.description = post.description;
            newPost.createdAt = DateTime.Now;

            var UserEmail = HttpContext.Session.GetString("email");
            Users? user = _masterDBContext.Users.FirstOrDefault(p => p.Email == UserEmail);

            newPost.UserId = user.Id;
            newPost.Owner = user.Name;

            _masterDBContext.Posts.Add(newPost);
            _masterDBContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // Edit a post
        
        public IActionResult EditPost(Guid id)
        {
            Posts? post = _masterDBContext.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
           
            return View(post);
        }

        [HttpPost]
        public IActionResult EditPost(Posts post)
        {
            if (post.name == null)
            {
                ModelState.AddModelError(nameof(post.name), "Title cannot be null");
                return View(post);
            }
            if (post.description == null)
            {
                ModelState.AddModelError(nameof(post.description), "Content cannot be null");
                return View(post);
            }

            Posts? existingPost = _masterDBContext.Posts.FirstOrDefault(p => p.Id == post.Id);

            


            existingPost.updatedAt = DateTime.Now;

            if (post.name != existingPost.name || post.description != existingPost.description)
            {
                existingPost.name = post.name;
                existingPost.description = post.description;
                _masterDBContext.Posts.Update(existingPost);
                _masterDBContext.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }



        public IActionResult logout()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
