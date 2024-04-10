using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;
using FilmsToWatch.Repositories.Contracts;
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
                FilmAdderId = "44be44ab-fc04-49e1-8284-0387c1404a2e"
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

        public Task<FilmFormModel> EditFilmAsync(FilmFormModel model)
        {
            throw new NotImplementedException();
        }
    }
}
