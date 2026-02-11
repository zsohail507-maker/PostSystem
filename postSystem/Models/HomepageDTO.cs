using postSystem.Models.Entities;

namespace postSystem.Models
{
    public class HomepageDTO
    {
        public List<Post> Posts { get; set; } = new();
        public Dictionary<string, int> Categories { get; set; } = new();
    }
}
