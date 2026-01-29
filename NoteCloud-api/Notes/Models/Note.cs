using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NoteCloud_api.Categories.Models;
using NoteCloud_api.Users.Models;

namespace NoteCloud_api.Notes.Models
{
    [Table("notes")]
    public class Note
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; } = default!;

        [Required]
        [Column("content")]
        public string Content { get; set; } = default!;

        [Required]
        [Column("categoryId")]
        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }

        [Required]
        [Column("userId")]
        public Guid UserId { get; set; }

        public User? User { get; set; }

        [Required]
        [Column("isFavorite")]
        public bool isFavorite { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

    }
}
