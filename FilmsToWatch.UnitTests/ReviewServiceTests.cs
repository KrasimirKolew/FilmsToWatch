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
    }
}

