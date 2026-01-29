using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteCloud_api.Categories.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        public List<Notes.Models.Note> Notes { get; set; } = new();
    }
}
