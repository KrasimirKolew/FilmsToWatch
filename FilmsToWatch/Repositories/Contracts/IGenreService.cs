using FilmsToWatch.Data.Models;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IGenreService
    {
        Task<bool> AddAsync(Genre model);
        Task<bool> UpdateAsync(Genre model);
        Task<Genre> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        //IQueryable<Genre> List();

        Task<List<Genre>> ListAsync();
    }
}
