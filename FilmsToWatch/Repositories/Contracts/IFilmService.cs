using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IFilmService
    {
        Task<int> AddFilmAsync(FilmFormModel model);
        Task<FilmFormModel> EditFilmAsync(FilmFormModel model);
        
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

    }
}
