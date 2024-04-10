using FilmsToWatch.Data.Models;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IActorService
    {
        Task<bool> AddAsync(Actor model);
        Task<bool> UpdateAsync(Actor model);
        Task<Actor> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<List<Actor>> ListAsync();
    }
}
