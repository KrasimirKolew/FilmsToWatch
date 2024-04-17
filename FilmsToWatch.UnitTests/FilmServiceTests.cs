using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;
using FilmsToWatch.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FilmsToWatch.UnitTests
{
    [TestFixture]
    public class FilmServiceTests
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
            // Arrange
            var filmService = new FilmService(_context);
            var filmFormModel = new FilmFormModel
            {
                Title = "New Film",
                MovieImage = "image.jpg",
                ReleaseYear = "02/03/2024",
                Director = "Jane Doe",
                GenreId = 1,
                ActorId = 1
            };

            // Act
            var result = await filmService.AddFilmAsync(filmFormModel);

            // Assert
            
            var filmInDb = await _context.Films.FirstOrDefaultAsync(g => g.Title == "New Film");
            Assert.IsNotNull(filmInDb);
        }

        [Test]
        public async Task MarkAsWatchedAsync_NewWatcher_SuccessfullyMarksFilmAsWatched()
        {
            // Arrange
            var filmId = 1;
            var userId = "user123";

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var service = new FilmService(context);

                // Act
                await service.MarkAsWatchedAsync(filmId, userId);

                // Assert
                var filmWatcher = await context.FilmWatchers.FirstOrDefaultAsync(fw => fw.FilmId == filmId && fw.HelperId == userId);
                Assert.NotNull(filmWatcher); // Ensure the film watcher record is added to the database
            }
        }

        [Test]
        public async Task MarkAsWatchedAsync_ExistingWatcher_ThrowsException()
        {
            // Arrange
            var filmId = 1;
            var userId = "user123";

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // Seed the database with an existing film watcher record
                context.FilmWatchers.Add(new FilmWatcher { FilmId = filmId, HelperId = userId });
                context.SaveChanges();

                var service = new FilmService(context);

                // Act & Assert
                Assert.ThrowsAsync<InvalidOperationException>(() => service.MarkAsWatchedAsync(filmId, userId));
            }
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
