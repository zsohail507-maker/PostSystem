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
        // When user selects "Other", this field contains the new category name
        public string? NewCategory { get; set; }
        // removed IsPublished from the CreatePost form; posts are published by default
        // kept for backward compatibility but ignored in server logic
        public bool IsPublished { get; set; }

        // Required for model binding
        public AddPost() { }
    }
}
