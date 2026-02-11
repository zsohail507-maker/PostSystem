using Microsoft.EntityFrameworkCore;
using postSystem.Models.Entities;

namespace postSystem.Models.Data
{
    public class MasterDBContext : DbContext
    {
        public MasterDBContext(DbContextOptions<MasterDBContext> options)
            : base(options)
        {
        }

        // =========================
        // DbSets
        // =========================

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        // =========================
        // Fluent API Configuration
        // =========================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------
            // Post
            // -------------------------
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(p => p.Slug)
                      .IsRequired()
                      .HasMaxLength(250);

                entity.HasIndex(p => p.Slug)
                      .IsUnique();

                entity.Property(p => p.Content)
                      .IsRequired();

                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasMany(p => p.Comments)
                      .WithOne(c => c.Post)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------------
            // Comment
            // -------------------------
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.UserName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Content)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(c => c.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            // -------------------------
            // PostLike
            // -------------------------
            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.UserFingerprint)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasIndex(l => new { l.PostId, l.UserFingerprint })
                      .IsUnique(); // Prevent duplicate likes
            });

            // -------------------------
            // AdminUser
            // -------------------------
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.Password)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.HasIndex(a => a.Username)
                      .IsUnique();
            });
        }
    }
}
