using Microsoft.Extensions.Hosting;

namespace postSystem.Models.Entities
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Posts> Posts { get; set; } = new List<Posts>();
        public ICollection<Comments> Comments { get; set; } = new List<Comments>();
    }
}
