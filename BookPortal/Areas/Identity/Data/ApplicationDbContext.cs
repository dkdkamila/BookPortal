using Microsoft.EntityFrameworkCore;
using BookPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        public DbSet<Book>? Books { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<Rating>? Ratings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Aktivera detaljerad loggning för SQL-förfrågningar
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurera relationer här
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(rt => rt.Book)
                .WithMany(b => b.Ratings)
                .HasForeignKey(rt => rt.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Konfigurera ApplicationUser-relationer här
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Ratings)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
