using FilmsToWatch.Data.Models;
using FilmsToWatch.Data;
using FilmsToWatch.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmsToWatch.Models.ReviewModels;
using Moq;
using FilmsToWatch.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace FilmsToWatch.UnitTests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private ApplicationDbContext _context;
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void SetUp()
        {

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "TestDatabase")
           .Options;

            _context = new ApplicationDbContext(_options);

            var user = new IdentityUser { UserName = "petko@abv.bg", Id = "1", Email = "asd@abv.bg" };

            _context.Reviews.AddRange(
                    new Review { Id = 1, FilmId = 1, Content = "Review 1", UserId = user.Id, User=user },
                    new Review { Id = 2, FilmId = 1, Content = "Review 2", UserId = user.Id, User = user },
                    new Review { Id = 3, FilmId = 2, Content = "Review 3", UserId = user.Id, User = user }
                );
            _context.SaveChanges();
        }

        [Test]
        public async Task AddFilmAsync_GivenNewGenre_ShouldAddFilm()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            var userId = "user123";
            var filmId = 1;

            using (var context = new ApplicationDbContext(options))
            {
                context.Films.Add(new Film
                {
                    Id = filmId,
                    Title = "New Film",
                    MovieImage = "image.jpg",
                    ReleaseYear = "02/03/2024",
                    Director = "Jane Doe",
                    GenreId = 1,
                    ActorId = 1,
                    FilmAdderId = "1"
                });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var reviewService = new ReviewService(context);
                var reviewModel = new ReviewCreateViewModel
                {
                    Content = "test content for unit testing",
                    FilmId = filmId
                };
                await reviewService.CreateReviewAsync(reviewModel, userId, filmId);

                var reviewInDb = await context.Reviews.FirstOrDefaultAsync(r => r.FilmId == filmId);
                Assert.IsNotNull(reviewInDb);
            }

        }
        [Test]
        public async Task EditAsync_ExistingReview_UpdatesReviewAndReturnsFilmId()
        {
            // Arrange
            var model = new ReviewCreateViewModel { Id = 123, Content = "Updated content" };
            var existingReview = new Review { Id = 4, Content = "Old content", FilmId = 123 };
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Reviews.Add(existingReview);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new ReviewService(context);

                // Act
                var result = await service.EditAsync(4, model);

                // Assert
                Assert.AreEqual(model.Id, result); // Ensure the correct film ID is returned

                var updatedReview = await context.Reviews.FindAsync(4);
                Assert.AreEqual(model.Content, updatedReview.Content); // Ensure the review content is updated
            }
        }

        [Test]
        public async Task EditAsync_NonExistingReview_ThrowsException()
        {
            // Arrange
            var model = new ReviewCreateViewModel { Id = 6, Content = "Updated content" };
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new ReviewService(context);

                // Act & Assert
                Assert.ThrowsAsync<KeyNotFoundException>(() => service.EditAsync(6, model));
            }
        }

        [Test]
        public async Task DeleteAsync_RemovesReview()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Seed the database with some test data
            using (var context = new ApplicationDbContext(options))
            {
                context.Reviews.Add(new Review { Id = 7, Content = "Test Review" });
                await context.SaveChangesAsync();
            }

            // Use a separate context for the test to ensure isolation
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewService(context);

                // Act
                await service.DeleteAsync(7);

                // Assert
                var deletedReview = await context.Reviews.FindAsync(7);
                Assert.Null(deletedReview);
            }

        }
        [Test]
        public async Task ExistsAsync_ReturnsTrueForExistingReview()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);

                // Act
                var exists = await service.ExistsAsync(1);

                // Assert
                Assert.IsTrue(exists);
            }
        }

        [Test]
        public async Task ExistsAsync_ReturnsFalseForNonExistingReview()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);

                // Act
                var exists = await service.ExistsAsync(99); // Assuming review with ID 99 does not exist

                // Assert
                Assert.IsFalse(exists);
            }
        }
        [Test]
        public async Task GetAllReviewsForEventAsync_ReturnsCorrectReviews()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);
                var filmId = 1;

                // Act
                var reviews = await service.GetAllReviewsForEventAsync(filmId);

                // Assert
                Assert.AreEqual(2, reviews.Count());
                Assert.IsTrue(reviews.All(r => r.FilmId == filmId));
            }
        }

        [Test]
        public async Task GetAllReviewsForEventAsync_ReturnsEmptyListForNonExistingEvent()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);
                var nonExistingFilmId = 99; // Assuming film with ID 99 does not exist

                // Act
                var reviews = await service.GetAllReviewsForEventAsync(nonExistingFilmId);

                // Assert
                Assert.IsEmpty(reviews);
            }
        }

        [Test]
        public async Task SameUserAsync_ReturnsTrueForSameUser()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);
                var reviewId = 1;
                var currentUserId = "1";

                // Act
                var result = await service.SameUserAsync(reviewId, currentUserId);

                // Assert
                Assert.IsTrue(result);
            }
        }

        [Test]
        public async Task SameUserAsync_ReturnsFalseForDifferentUser()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);
                var reviewId = 1;
                var currentUserId = "user2";

                // Act
                var result = await service.SameUserAsync(reviewId, currentUserId);

                // Assert
                Assert.IsFalse(result);
            }
        }

        [Test]
        public async Task SameUserAsync_ReturnsFalseForNonExistingReview()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);
                var nonExistingReviewId = 99; // Assuming review with ID 99 does not exist
                var currentUserId = "user1";

                // Act
                var result = await service.SameUserAsync(nonExistingReviewId, currentUserId);

                // Assert
                Assert.IsFalse(result);
            }
        }

        [Test]
        public async Task ReviewByIdAsync_ReviewExists_ShouldReturnReview()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);

                // Act
                var result = await service.ReviewByIdAsync(1);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Review 1", result.Content);
            }
        }

        [Test]
        public async Task ReviewByIdAsync_ReviewDoesNotExist_ShouldThrowException()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);

                // Act & Assert
                Assert.ThrowsAsync<InvalidOperationException>(async () => await service.ReviewByIdAsync(100));
            }
        }

        [Test]
        public async Task ReviewByIdWithUserAsync_ReviewExists_ShouldReturnReviewWithUser()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);

                // Act
                var result = await service.ReviewByIdWithUserAsync(1);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Review 1", result.Content);
                Assert.IsNotNull(result.User);
                Assert.AreEqual("1", result.User.Id);
            }
        }

        [Test]
        public async Task ReviewByIdWithUserAsync_ReviewDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ReviewService(context);

                // Act
                var result = await service.ReviewByIdWithUserAsync(100);

                // Assert
                Assert.IsNull(result);
            }
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}

