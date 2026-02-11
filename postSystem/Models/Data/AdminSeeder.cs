using Microsoft.AspNetCore.Identity;
using postSystem.Models.Entities;
using System;
using System.Linq;

namespace postSystem.Models.Data
{
    public static class AdminSeeder
    {
        // ✅ Accept DbContext directly
        public static void Seed(MasterDBContext db)
        {
            // Prevent duplicate seeding
            if (db.AdminUsers.Any())
                return;

            var hasher = new PasswordHasher<AdminUser>();

            db.AdminUsers.Add(new AdminUser
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Password = hasher.HashPassword(null, "Admin@123")
            });

            db.SaveChanges();
        }
    }
}
