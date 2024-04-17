﻿using FilmsToWatch.Data.Models;
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

namespace FilmsToWatch.UnitTests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            _context = new ApplicationDbContext(options);
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
            var existingReview = new Review { Id = 1, Content = "Old content", FilmId = 123 };
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
                var result = await service.EditAsync(1, model);

                // Assert
                Assert.AreEqual(model.Id, result); // Ensure the correct film ID is returned

                var updatedReview = await context.Reviews.FindAsync(1);
                Assert.AreEqual(model.Content, updatedReview.Content); // Ensure the review content is updated
            }
        }

        [Test]
        public async Task EditAsync_NonExistingReview_ThrowsException()
        {
            // Arrange
            var model = new ReviewCreateViewModel { Id = 1, Content = "Updated content" };
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new ReviewService(context);

                // Act & Assert
                Assert.ThrowsAsync<KeyNotFoundException>(() => service.EditAsync(1, model));
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
                context.Reviews.Add(new Review { Id = 1, Content = "Test Review" });
                await context.SaveChangesAsync();
            }

            // Use a separate context for the test to ensure isolation
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ReviewService(context);

                // Act
                await service.DeleteAsync(1);

                // Assert
                var deletedReview = await context.Reviews.FindAsync(1);
                Assert.Null(deletedReview);
            }

        }
    }
}

