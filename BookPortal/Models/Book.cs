using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookPortal.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Titel är obligatoriskt.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Författare är obligatoriskt.")]
        public string? Author { get; set; }

        public string? Genre { get; set; }
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
    }
}