namespace postSystem.Models.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public string UserName { get; set; }
        public string Content { get; set; }

        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
