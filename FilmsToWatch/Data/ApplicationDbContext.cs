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

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Film> Films { get; set; } = null!;
		public DbSet<Genre> Genre { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
		public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<FilmWatcher> FilmWatchers { get; set; } = null!;

    }
}
