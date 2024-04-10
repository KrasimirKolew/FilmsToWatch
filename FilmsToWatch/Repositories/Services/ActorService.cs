using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FilmsToWatch.Repositories.Services
{
    public class ActorService : IActorService
    {
        private readonly ApplicationDbContext context;

        public ActorService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<bool> AddAsync(Actor model)
        {
            try
            {
                await context.Actors.AddAsync(model);
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
                var data = await GetByIdAsync(id);
                if (data == null)
                {
                    return false;
                }
                context.Actors.Remove(data);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Actor> GetByIdAsync(int id)
        {
            var actor = await context.Actors.FindAsync(id);
            return actor;
        }

        public async Task<List<Actor>> ListAsync()
        {
            var data = await context.Actors.ToListAsync();
            return data;
        }

        public async Task<bool> UpdateAsync(Actor model)
        {
            try
            {
                context.Actors.Update(model);
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
