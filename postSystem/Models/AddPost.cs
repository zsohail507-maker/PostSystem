using Microsoft.AspNetCore.Http;

namespace postSystem.Models
{
    public class AddPost
    {
        public string Title { get; set; }
        public string Content { get; set; }

        // Image upload
        public IFormFile Image { get; set; }
        public string Category { get; set; }
        public bool IsPublished { get; set; }

        // Required for model binding
        public AddPost() { }
    }
}
