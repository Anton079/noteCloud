using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteCloud_api.Users.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        [MaxLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Column("firstName")]
        public string FirstName { get; set; }

        [Required]
        [Column("lastName")]
        public string LastName { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

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


    }
}
