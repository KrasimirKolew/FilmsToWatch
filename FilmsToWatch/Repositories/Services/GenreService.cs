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

        public async Task<bool> AddAsync(Genre model)
        {
            try
            {
                await context.Genre.AddAsync(model); 
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Consider logging the exception details to help with debugging
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var data = await this.GetByIdAsync(id); // Make sure to await the result
                if (data == null)
                {
                    return false;
                }
                context.Genre.Remove(data);
                await context.SaveChangesAsync(); // Use SaveChangesAsync to save asynchronously
                return true;
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return false;
            }
        }

        //public Genre GetById(int id)
        //{
        //    return context.Genre.Find(id);
        //}

        public async Task<Genre> GetByIdAsync(int id)
        {
            var genre = await context.Genre.FindAsync(id);
            return genre;
        }

        //public IQueryable<Genre> List()
        //{
        //    var data = context.Genre.AsQueryable();
        //    return data;
        //}

        public async Task<List<Genre>> ListAsync()
        {
            var data = await context.Genre.ToListAsync();
            return data;
        }

        //public bool Update(Genre model)
        //{
        //    try
        //    {
        //        context.Genre.Update(model);
        //        context.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public async Task<bool> UpdateAsync(Genre model)
        {
            try
            {
                context.Genre.Update(model); // No async equivalent needed for Update
                await context.SaveChangesAsync(); // Asynchronously save the changes
                return true;
            }
            catch (Exception ex)
            {
                // Optionally log the exception here
                return false;
            }
        }
    }
}
