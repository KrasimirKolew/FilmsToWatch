using FilmsToWatch.Data.Models;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IGenreService
    {
        bool Add(Genre model);
        bool Update(Genre model);
        Genre GetById(int id);
        bool Delete(int id);
        IQueryable<Genre> List();
    }
}
