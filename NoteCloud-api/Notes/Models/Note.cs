using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NoteCloud_api.Users.Models;

namespace NoteCloud_api.Notes.Models
{
    [Table("notes")]
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; }

        [Required]
        [Column("category")]
        public string Category { get; set; }

        [Required]
        [Column("userId")]
        [MaxLength(100)]
        public string UserId { get; set; }

        public User? User { get; set; }

        [Required]
        [Column("isFavorite")]
        public bool isFavorite { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

    }
}
