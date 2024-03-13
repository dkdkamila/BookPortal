// User modell (ASP.NET Core Identity-modell)
using Microsoft.AspNetCore.Identity;

namespace BookPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Book>? Books { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
    }
}