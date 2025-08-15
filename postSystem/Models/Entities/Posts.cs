namespace postSystem.Models.Entities
{
    public class Posts
    {
        public Guid Id { get; set; }
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public string Owner { get; set; } = null!; 
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public Guid UserId { get; set; }

        public Users User { get; set; } = null!;
        public ICollection<Comments> Comments { get; set; } = new List<Comments>();
    }
}
