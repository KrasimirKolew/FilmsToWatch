using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.ReviewModels;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using System.Globalization;
using System.Runtime.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FilmsToWatch.Repositories.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<ReviewService> logger;

        public ReviewService(ApplicationDbContext _context, ILogger<ReviewService> _logger)
        {
            context = _context;
            logger = _logger;
        }


        public Task<Review> CommentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Review> CommentByIdWithUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task CreateCommentAsync(ReviewViewModel reviewModel, int filmId)
        {
            Review review = new Review()
            {
                Id= reviewModel.Id,
                Content = reviewModel.Content,
                FilmId = filmId,
            };


            try
            {
                await context.AddAsync(review);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) 
            { 
            
                throw new ApplicationException("Database failed to save info", ex);
            }
        }

        public Task DeleteAsync(int reviewId)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(int reviewId, ReviewViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReviewViewModel>> GetAllCommentsForEventAsync(int filmId)
        {
            return await context.Reviews
                .Where(r=> r.FilmId == filmId)
                .Select(r => new ReviewViewModel()
                {
                    Id = r.Id,
                    Content = r.Content,
                })
                .ToListAsync();
        }

        public Task<bool> SameUserAsync(int reviewId, string currentUserId)
        {
            throw new NotImplementedException();
        }
    }
}
