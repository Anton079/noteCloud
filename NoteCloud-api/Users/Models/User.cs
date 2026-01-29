using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NoteCloud_api.Notes.Models;

namespace NoteCloud_api.Users.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("firstName")]
        public string FirstName { get; set; } = default!;

        [Required]
        [Column("lastName")]
        public string LastName { get; set; } = default!;

        [Required]
        [Column("email")]
        public string Email { get; set; } = default!;

        [Required]
        [Column("createdAt", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [Column("updatedAt", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }


        public string? Role { get; set; }
        public string? PasswordHash { get; set; }

        [NotMapped]
        public string? Password { get; set; }

        public string? PasswordSalt { get; set; }


        [Column("lastLoginAt", TypeName = "datetime")]
        public DateTime? LastLoginAt { get; set; }


        [MaxLength(500)]
        [Column("refreshToken")]
        public string? RefreshToken { get; set; }


        [Column("refreshTokenExpiryTime", TypeName = "datetime")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public List<Note> Notes { get; set; } = new();

    }
}
