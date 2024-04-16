using FilmsToWatch.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FilmsToWatch.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmWatcher>()
                .HasKey(e => new { e.FilmId, e.HelperId });

            modelBuilder.Entity<FilmWatcher>()
                .HasOne(e => e.Film)
                .WithMany(e => e.FilmsWatchers)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Genre>()
                .HasData(new Genre()
                {
                    Id = 1,
                    GenreName="Mystery"
                },
                new Genre()
                {
                    Id = 2,
                    GenreName="Thriller"
                },
                new Genre()
                {
                    Id = 3,
                    GenreName = "sci-fi"
                });

            base.OnModelCreating(modelBuilder);
        }


        public virtual DbSet<Film> Films { get; set; } = null!;
        public DbSet<Genre> Genre { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<FilmWatcher> FilmWatchers { get; set; } = null!;

    }
}
