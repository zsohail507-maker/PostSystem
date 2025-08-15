using Microsoft.EntityFrameworkCore;
using postSystem.Models.Entities;

namespace postSystem.Models.Data
{
    public class MasterDBContext : DbContext
    {
      
        public MasterDBContext(DbContextOptions<MasterDBContext> options) : base(options)  {
        
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Comments> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Posts (allow cascade)
            modelBuilder.Entity<Users>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict); // keep this

            // User → Comments (disable cascade to avoid cycle)
            modelBuilder.Entity<Users>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // Post → Comments (disable cascade to avoid cycle)
            modelBuilder.Entity<Posts>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction
        }

    }
}
