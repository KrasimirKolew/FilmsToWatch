using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IFilmService
    {
        Task<int> AddFilmAsync(FilmFormModel model);
        Task<FilmFormModel> EditFilmAsync(FilmFormModel model);
        //Task<IEnumerable<Film>> GetAllFilmsAsync();

        Task<IEnumerable<FilmGenreServiceModel>> AllGenresAsync();
        Task<IEnumerable<FilmActorServiceModel>> AllActorsAsync();

    }
}
