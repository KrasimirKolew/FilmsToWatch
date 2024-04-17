using FilmsToWatch.Controllers;
using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Repositories.Contracts;
using FilmsToWatch.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Packaging.Core;
using System;

namespace FilmsToWatch.UnitTests
{
    [TestFixture]
    public class GenreServiceTests
    {

        private ApplicationDbContext _context;
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void SetUp()
        {

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Make sure each test run uses a new db
            .Options;

            _context = new ApplicationDbContext(_options);

            using (var context = new ApplicationDbContext(_options))
            {
                // Seed the in-memory database with test data
                context.Genre.AddRange(
                    new Genre { Id = 1, GenreName = "Genre1" },
                    new Genre { Id = 2, GenreName = "Genre2" }
                );
                context.SaveChanges();
            }
        }


        [Test]
        public async Task GetByGenreIdAsync_ValidId_ReturnsGenre()
        {
            var genre = new Genre
            {
                Id = 1,
                GenreName = "Test",
            };

            var genreService = new Mock<IGenreService>();
            genreService.Setup(x=>x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(genre);
            var controler = new GenreController(genreService.Object);

            var getGenreId = await controler.Edit(1);

            Assert.IsNotNull(getGenreId);
        }
        [Test]
        public async Task AddAsync_GivenNewGenre_ShouldAddGenre()
        {
            // Arrange
            var genreService = new GenreService(_context);
            var genre = new Genre { Id=1, GenreName = "Drama" };

            // Act
            var result = await genreService.AddAsync(genre);

            // Assert
            Assert.IsTrue(result);

            var genreInDb = await _context.Genre.FirstOrDefaultAsync(g => g.GenreName == "Drama");
            Assert.IsNotNull(genreInDb);
        }
        [Test]
        public async Task DeleteAsync_GivenValidId_ShouldDeleteGenre()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Make sure each test run uses a new db
                .Options;

            using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.Genre.Add(new Genre { Id = 2, GenreName = "Drama" });
                await setupContext.SaveChangesAsync();
            }

            using (var testContext = new ApplicationDbContext(options))
            {
                var genreService = new GenreService(testContext);

                // Act
                var result = await genreService.DeleteAsync(2);

                // Assert
                Assert.IsTrue(result, "Genre should be deleted successfully");

                // Additional verification to ensure the genre was deleted
                var genreInDb = await testContext.Genre.FirstOrDefaultAsync(g => g.Id == 2);
                Assert.IsNull(genreInDb, "Genre should no longer exist in the database");
            }
        }

        [Test]
        public async Task ListAsync_ReturnsAllGenres()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new GenreService(context);

                // Act
                var genres = await service.ListAsync();

                // Assert
                Assert.IsNotNull(genres);
                Assert.AreEqual(2, genres.Count);
            }
        }

        [Test]
        public async Task UpdateAsync_GenreExists_ShouldUpdateGenre()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new GenreService(context);
                var updatedGenre = new Genre { Id = 1, GenreName = "UpdatedGenre" };

                // Act
                var result = await service.UpdateAsync(updatedGenre);

                // Assert
                Assert.IsTrue(result);

                var genreFromDb = await context.Genre.FindAsync(1);
                Assert.IsNotNull(genreFromDb);
                Assert.AreEqual("UpdatedGenre", genreFromDb.GenreName);
            }
        }

        [Test]
        public async Task UpdateAsync_GenreDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new GenreService(context);
                var nonExistentGenre = new Genre { Id = 100, GenreName = "NonExistentGenre" };

                // Act
                var result = await service.UpdateAsync(nonExistentGenre);

                // Assert
                Assert.IsFalse(result);
            }
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose(); 
        }
    }
}