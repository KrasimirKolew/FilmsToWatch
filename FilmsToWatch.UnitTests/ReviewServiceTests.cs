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
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Make sure each test run uses a new db
            .Options;

            _context = new ApplicationDbContext(options);
        }

        [Test]
        public async Task AddFilmAsync_GivenNewGenre_ShouldAddFilm()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Make sure each test has a unique name for the database
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

            //    var mockFilmSet = new Mock<DbSet<Film>>();
            ////var films = new List<Film>
            ////{
            ////    new Film { 
            ////        Id = filmId,
            ////        Title = "New Film",
            ////        MovieImage = "image.jpg",
            ////        ReleaseYear = "02/03/2024",
            ////        Director = "Jane Doe",
            ////        GenreId = 1,
            ////        ActorId = 1,
            ////        FilmAdderId="1"
            ////    }
            ////}.AsQueryable();

            //mockFilmSet.As<IQueryable<Film>>().Setup(m => m.Provider).Returns(films.Provider);
            //mockFilmSet.As<IQueryable<Film>>().Setup(m => m.Expression).Returns(films.Expression);
            //mockFilmSet.As<IQueryable<Film>>().Setup(m => m.ElementType).Returns(films.ElementType);
            //mockFilmSet.As<IQueryable<Film>>().Setup(m => m.GetEnumerator()).Returns(films.GetEnumerator());


            //var mockContext = new Mock<ApplicationDbContext>();
            //mockContext.Setup(c => c.Films).Returns(mockFilmSet.Object);

            //var reviewService = new ReviewService(mockContext.Object);

            //// Act
            //await reviewService.CreateReviewAsync(reviewModel, userId, filmId);
            


            //var reviewInDb = await _context.Reviews.FirstOrDefaultAsync(r => r.FilmId == filmId && r.UserId == userId);
            //Assert.IsNotNull(reviewInDb);

        }
    }
}

//// Arrange
//var reviewService = new ReviewService(_context);

//var userId = "user123";
//var filmId = 1;

//var reviewMoldel =  new ReviewCreateViewModel()
//{
//    Id = 1,
//    Content = "test content for unit testing",
//    FilmId = 1
//};

//var result = await reviewService.CreateReviewAsync(reviewMoldel, userId, filmId);

//var dbActor = await reviewService.GetAllReviewsForEventAsync(filmId);

//Assert.IsTrue(result);
//Assert.That(dbActor.ActorName, Is.EqualTo("Test"));
//Assert.That(dbActor.FilmsInvolve, Is.EqualTo(5));
// Arrange
