using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;
using System.IO;

namespace FilmsToWatch.Repositories.Extension
{
    public static class IQuerableFilmExtension
    {
        public static IQueryable<FilmServiceModel> ProjectToFilmServiceModel(this IQueryable<Film> houses)
        {
            return houses
                .Select(h => new FilmServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    MovieImage = h.MovieImage,
                    ReleaseYear= h.ReleaseYear,
                    Director= h.Director,
                });
        }
    }
}
