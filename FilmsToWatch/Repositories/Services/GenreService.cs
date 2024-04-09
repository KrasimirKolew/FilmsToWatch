using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FilmsToWatch.Repositories.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext context;

        public GenreService (ApplicationDbContext _context)
        {
            context = _context;
        }

        public bool Add(Genre model)
        {
            try
            {
                context.Genre.Add(model);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;
                context.Genre.Remove(data);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Genre GetById(int id)
        {
            return context.Genre.Find(id);
        }

        public IQueryable<Genre> List()
        {
            var data = context.Genre.AsQueryable();
            return data;
        }

        public bool Update(Genre model)
        {
            try
            {
                context.Genre.Update(model);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
