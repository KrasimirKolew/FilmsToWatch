using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.ReviewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewViewModel>> GetAllCommentsForEventAsync(int filmId);
        Task CreateCommentAsync(ReviewViewModel reviewModel, int filmId);
        Task<Review> CommentByIdAsync(int id);
        Task<Review> CommentByIdWithUserAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> SameUserAsync(int reviewId, string currentUserId);
        Task<int> EditAsync(int reviewId, ReviewViewModel model);
        Task DeleteAsync(int reviewId);
    }
}
