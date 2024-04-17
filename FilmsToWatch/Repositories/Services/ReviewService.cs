using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.ReviewModels;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FilmsToWatch.Repositories.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext context;

        public ReviewService(ApplicationDbContext _context)
        {
            context = _context;
        }


        public async Task<Review> ReviewByIdAsync(int id)
        {
            return await context.Reviews
                .Where(r=>r.Id == id)
                .FirstAsync();
        }

        public async Task<Review> ReviewByIdWithUserAsync(int id)
        {
            return await context.Reviews
            .Where(c => c.Id == id)
            .Include(c => c.User)
            .FirstOrDefaultAsync();
        }

        public async Task CreateReviewAsync(ReviewCreateViewModel reviewModel, string userId, int filmId)
        {
            var filmExists = await context.Films.AnyAsync(f => f.Id == filmId);
            if (!filmExists)
            {
                throw new ArgumentException("Film does not exist.");
            }

            Review review = new Review()
            {
                Content = reviewModel.Content,
                FilmId = filmId,
                UserId = userId,
            };


            try
            {
                context.Add(review);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("Database failed to save info", ex);
            }
        }

        public async Task DeleteAsync(int reviewId)
        {
            var reviewToDel = await context.Reviews
                .Where(r=>r.Id==reviewId)
                .FirstAsync();

            try
            {
                context.Reviews.Remove(reviewToDel);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Delete faild", ex);
            }    
        }

        public async Task<int> EditAsync(int reviewId, ReviewCreateViewModel model)
        {
            var review = await context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {model.Id} not found.");
            }

            review.Content = model.Content;

            await context.SaveChangesAsync();
            return review.FilmId;
        }

        public async Task<bool> ExistsAsync(int idFilm)
        {
            return await context.Reviews.AnyAsync(r=> r.Id == idFilm);
        }

        public async Task<IEnumerable<ReviewViewModel>> GetAllReviewsForEventAsync(int filmId)
        {
            return await context.Reviews
                .Where(r=> r.FilmId == filmId)
                .Select(r => new ReviewViewModel()
                {
                    Id = r.Id,
                    Content = r.Content,
                    FilmId=filmId,
                    UserId=r.UserId,
                    UserName=r.User.UserName     
                })
                .ToListAsync();
        }

        public async Task<bool> SameUserAsync(int reviewId, string currentUserId)
        {
            bool result = false;
            var review = await context.Reviews
                .Where(r => r.Id == reviewId)
                .FirstOrDefaultAsync();

            if (review?.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }
    }
}
