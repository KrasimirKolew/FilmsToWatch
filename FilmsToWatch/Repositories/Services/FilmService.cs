using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;
using FilmsToWatch.Repositories.Contracts;
using FilmsToWatch.Repositories.Extension;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FilmsToWatch.Repositories.Services
{
    public class FilmService : IFilmService
    {
        private readonly ApplicationDbContext context;

        public FilmService(ApplicationDbContext _context)
        {
            context = _context;
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

        public Task<FilmFormModel> EditFilmAsync(FilmFormModel model)
        {
            throw new NotImplementedException();
        }
    }
}
