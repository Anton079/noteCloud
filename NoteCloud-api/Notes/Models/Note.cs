using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column("isFavorite")]
        public bool isFavorite { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

    }
}
