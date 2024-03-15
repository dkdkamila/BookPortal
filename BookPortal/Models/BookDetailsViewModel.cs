
namespace BookPortal.Models
{
    public class BookDetailsViewModel
    {
        public Book? Book { get; set; }
        public IEnumerable<Review>? Reviews { get; set; } = new List<Review>();
        public IEnumerable<Rating>? Ratings { get; set; } = new List<Rating>();
    }
}