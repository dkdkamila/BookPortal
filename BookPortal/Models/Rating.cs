// Rating modell
using System.ComponentModel.DataAnnotations;

namespace BookPortal.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? UserId { get; set; }

        [Range(1, 5, ErrorMessage = "Värdet för betyget måste vara mellan 1 och 5.")]
        public int? Value { get; set; }

        // Navigation properties
        public Book? Book { get; set; }
        public ApplicationUser? User { get; set; }
    }
}