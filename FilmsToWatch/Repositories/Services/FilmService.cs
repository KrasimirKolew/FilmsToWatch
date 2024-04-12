using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;
using FilmsToWatch.Repositories.Contracts;
using FilmsToWatch.Repositories.Extension;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks.Sources;

namespace FilmsToWatch.Repositories.Services
{
    public class FilmService : IFilmService
    {
        private readonly ApplicationDbContext context;

        public FilmService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<bool> ActorExistsAsync(int actorId)
        {
            return await context.Actors.AnyAsync(a=>a.Id == actorId);
        }

        public async Task<int> AddFilmAsync(FilmFormModel model)
        {
            Film film = new Film()
            {
                Title = model.Title,
                MovieImage = model.MovieImage,
                ReleaseYear = model.ReleaseYear,
                Director = model.Director,
                GenreId = model.GenreId,
                ActorId = model.ActorId,
                FilmAdderId = "fd0dba54-c56f-4bc0-8c3c-6acce711f0e4"
            };
            context.Add(film);
            await context.SaveChangesAsync();

            return film.Id;
        }

        public async Task<IEnumerable<FilmActorServiceModel>> AllActorsAsync()
        {
            var actors = await context.Actors
                .Select(a => new FilmActorServiceModel
                {
                    Id = a.Id,
                    ActorName = a.ActorName,
                })
                .ToListAsync();

            return actors;
        }

        public async Task<IEnumerable<string>> AllActorsNamesAsync()
        {
            var actorName = await context.Actors
                .Select(a => a.ActorName)
                .Distinct()
                .ToListAsync();

            return actorName;
        }

        public async Task<FilmQueryServiceModel> AllAsync(string? genre = null, string? actor = null, string? searchTerm = null, int currentPage = 1, int filmsPerPage = 1)
        {
            var filmsToShow = context.Films.AsQueryable();

            if (genre != null)
            {
                filmsToShow = filmsToShow
                    .Where(g=>g.Genre.GenreName == genre);
            }

            if (actor != null)
            {
                filmsToShow = filmsToShow
                    .Where(a => a.Actor.ActorName == actor);
            }

            if (searchTerm != null)
            {
                string normalizeSearchTerm = searchTerm.ToLower();

                filmsToShow = filmsToShow.Where(a=>a.Title.ToLower().Contains(searchTerm));
            }

            var films = await filmsToShow
                .Skip((currentPage - 1) * filmsPerPage)
                .Take(filmsPerPage)
                .ProjectToFilmServiceModel()
                .ToListAsync();

            int totalFilms = await filmsToShow.CountAsync();

            return new FilmQueryServiceModel
            {
                Films = films,
                TotalFilmsCount = totalFilms
            };
        }

        public async Task<IEnumerable<FilmGenreServiceModel>> AllGenresAsync()
        {
            var genres = await context.Genre
                  .Select(g => new FilmGenreServiceModel
                  {
                      Id = g.Id,
                      Name = g.GenreName
                  })
                  .ToListAsync();

            return genres;
        }

        public async Task<IEnumerable<string>> AllGenresNamesAsync()
        {
            var genreName = await context.Genre
                .Select(g => g.GenreName)
                .Distinct()
                .ToListAsync();

            return genreName;
        }

        public async Task EditFilmAsync(int filmId, FilmFormModel model)
        {
            var film = await context.Films.FindAsync(filmId);

            if (film == null)
            {
                throw new KeyNotFoundException($"Film with ID {model.Id} not found.");
            }

            film.Title = model.Title;
            film.MovieImage = model.MovieImage;
            film.ReleaseYear = model.ReleaseYear;
            film.Director = model.Director;
            film.GenreId = model.GenreId;
            film.ActorId = model.ActorId;
            //context.Films.Update(film);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Films.AnyAsync(f => f.Id == id);
        }

        public async Task<bool> GenreExistsAsync(int genreId)
        {
            return await context.Genre.AnyAsync(g=> g.Id == genreId);
        }

        public async Task<Film> GetFilmByIdAsync(int id)
        {
            return await context.Films.FindAsync(id);

        }

        public async Task<FilmFormModel?> GetFilmFormModelByIdAsync(int id)
        {
            var film = await context.Films
                .Where(f => f.Id == id)
                .Select(f => new FilmFormModel
                {
                    Title = f.Title,
                    MovieImage = f.MovieImage,
                    ReleaseYear = f.ReleaseYear,
                    Director = f.Director,
                    GenreId = f.GenreId,
                    ActorId = f.ActorId,
                })
                .FirstOrDefaultAsync();

            if (film != null)
            {
                film.Genres = await AllGenresAsync();
                film.Actors = await AllActorsAsync();
            }

            return film;
        }

        public async Task<IEnumerable<Film>> GetWatchedFilmsAsync(string userId)
        {
            var watchedFilms = await context.FilmWatchers
            .Where(fw => fw.HelperId == userId)
            .Select(fw => fw.Film)
            .ToListAsync();

            return watchedFilms;
        }

        public async Task MarkAsWatchedAsync(int filmId, string userId)
        {
            var existingWatcher = await context.FilmWatchers
            .FirstOrDefaultAsync(fw => fw.FilmId == filmId && fw.HelperId == userId);

            if (existingWatcher != null)
            {
                // The film is already marked as watched, so you might want to handle this case.
                throw new InvalidOperationException("The user has already marked this film as watched.");
            }

            // If not, add a new FilmWatcher entry to mark the film as watched.
            var filmWatcher = new FilmWatcher
            {
                HelperId = userId,
                FilmId = filmId
            };

            context.FilmWatchers.Add(filmWatcher);
            await context.SaveChangesAsync();
        }
    }
}
