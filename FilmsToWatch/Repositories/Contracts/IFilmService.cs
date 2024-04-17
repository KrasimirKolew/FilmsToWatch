using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IFilmService
    {
        Task<int> AddFilmAsync(FilmFormModel model);
        Task EditFilmAsync(int filmId, FilmFormModel model);
        Task<Film> GetFilmByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> GenreExistsAsync(int genreId);
        Task<bool> ActorExistsAsync(int actorId);

        Task<FilmFormModel?> GetFilmFormModelByIdAsync(int id);

        Task<FilmQueryServiceModel> AllAsync(
            string? genre = null, 
            string? actor = null,
            string? searchTerm = null,
            int currentPage = 1,
            int filmsPerPage = 1);

        Task<IEnumerable<string>> AllGenresNamesAsync();

        Task<IEnumerable<string>> AllActorsNamesAsync();

        Task<IEnumerable<FilmGenreServiceModel>> AllGenresAsync();
        Task<IEnumerable<FilmActorServiceModel>> AllActorsAsync();
        Task MarkAsWatchedAsync(int filmId, string userId);
        Task<IEnumerable<Film>> GetWatchedFilmsAsync(string userId);

        Task<FilmDetailsServiceModel> FilmDetailsByIdAsync(int id);

        //addon
        Task<bool> FilmHasBeenWatched(string userId, int filmId);

    }
}
