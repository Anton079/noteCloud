using Microsoft.EntityFrameworkCore;
using NoteCloud_api.Notes.Models;
using NoteCloud_api.Users.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace NoteCloud_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id).HasColumnName("id").HasMaxLength(100);
                entity.Property(u => u.FirstName).HasColumnName("firstName").HasMaxLength(50);
                entity.Property(u => u.LastName).HasColumnName("lastName").HasMaxLength(50);
                entity.Property(u => u.Email).HasColumnName("email").HasMaxLength(100);
                entity.Property(u => u.Role).HasColumnName("role").HasMaxLength(50);
                entity.Property(u => u.PasswordHash).HasColumnName("passwordHash").HasMaxLength(255);
                entity.Property(u => u.PasswordSalt).HasColumnName("passwordSalt").HasMaxLength(255);
                entity.Property(u => u.CreatedAt).HasColumnName("createdAt");
                entity.Property(u => u.UpdatedAt).HasColumnName("updatedAt");
                entity.Property(u => u.LastLoginAt).HasColumnName("lastLoginAt");
                entity.Property(u => u.RefreshToken).HasColumnName("refreshToken").HasMaxLength(500);
                entity.Property(u => u.RefreshTokenExpiryTime).HasColumnName("refreshTokenExpiryTime");

                entity.Ignore(u => u.Password);
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("notes");
                entity.HasKey(n => n.Id);

                entity.Property(n => n.Id).HasColumnName("id");
                entity.Property(n => n.Title).HasColumnName("title").HasMaxLength(255);
                entity.Property(n => n.Content).HasColumnName("content").HasMaxLength(5000);
                entity.Property(n => n.Category).HasColumnName("category").HasMaxLength(100);
                entity.Property(n => n.isFavorite).HasColumnName("isFavorite");
                entity.Property(n => n.Date).HasColumnName("date");
            });
        }
    }
}
