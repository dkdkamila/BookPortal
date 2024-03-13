// Review modell
using System.ComponentModel.DataAnnotations;

namespace BookPortal.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? UserId { get; set; }

        public string? Text { get; set; }

        // Navigation properties
        public Book? Book { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
