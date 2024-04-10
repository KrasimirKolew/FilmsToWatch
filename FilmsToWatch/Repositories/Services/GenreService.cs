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
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var data = await this.GetByIdAsync(id);
                if (data == null)
                {
                    return false;
                }
                context.Genre.Remove(data);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            var genre = await context.Genre.FindAsync(id);
            return genre;
        }

        public async Task<List<Genre>> ListAsync()
        {
            var data = await context.Genre.ToListAsync();
            return data;
        }

        public async Task<bool> UpdateAsync(Genre model)
        {
            try
            {
                context.Genre.Update(model);
                await context.SaveChangesAsync(); 
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
