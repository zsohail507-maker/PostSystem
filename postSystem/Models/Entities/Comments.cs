using Microsoft.Extensions.Hosting;

namespace postSystem.Models.Entities
{
    public class Comments
    {
        public Guid Id { get; set; }
        public string description { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public string Owner { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        public Posts Post { get; set; } = null!;
        public Users User { get; set; } = null!;
    }
}
