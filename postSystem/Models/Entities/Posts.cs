namespace postSystem.Models.Entities
{
    public class Post
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public int LikesCount { get; set; }
        public int ViewsCount { get; set; }

        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
