﻿using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.ReviewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewViewModel>> GetAllReviewsForEventAsync(int filmId);
        Task CreateReviewAsync(ReviewCreateViewModel reviewModel, string userId, int filmId);
        Task<Review> ReviewByIdAsync(int id);
        Task<Review> ReviewByIdWithUserAsync(int id);
        Task<bool> ExistsAsync(int idFilm);
        Task<bool> SameUserAsync(int reviewId, string currentUserId);
        Task<int> EditAsync(int reviewId, ReviewCreateViewModel model);
        Task DeleteAsync(int reviewId);
    }
}
