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
        //private Mock<ApplicationDbContext> _mockContext;
        //private FilmService _service;

        //[SetUp]
        //public void Setup()
        //{
        //    _mockContext = new Mock<ApplicationDbContext>();
        //    _service = new FilmService(_mockContext.Object);
        //}
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

        //[Test]
        //public async Task ActorExistsAsync_ReturnsTrue_WhenActorExists()
        //{
        //    // Arrange
        //    var actors = new List<Actor>
        //    {
        //         new Actor { Id = 1, ActorName = "John Doe" }
        //    }.AsQueryable();

        //    var mockSet = new Mock<DbSet<Actor>>();
        //    mockSet.As<IQueryable<Actor>>().Setup(m => m.Provider).Returns(actors.Provider);
        //    mockSet.As<IQueryable<Actor>>().Setup(m => m.Expression).Returns(actors.Expression);
        //    mockSet.As<IQueryable<Actor>>().Setup(m => m.ElementType).Returns(actors.ElementType);
        //    mockSet.As<IQueryable<Actor>>().Setup(m => m.GetEnumerator()).Returns(actors.GetEnumerator());
        //    _mockContext.Setup(c => c.Actors).Returns(mockSet.Object);

        //    // Act
        //    var result = await _service.ActorExistsAsync(1);

        //    // Assert
        //    Assert.IsTrue(result);
        //}

        //[Test]
        //public async Task ActorExistsAsync_ReturnsFalse_WhenActorDoesNotExist()
        //{
        //    // Arrange similar to above, with no matching actorId
        //    var actors = new List<Actor>
        //    {
        //         new Actor { Id = 2, ActorName = "Krasimir Kolev" }
        //    }.AsQueryable();

        //    var mockSet = new Mock<DbSet<Actor>>();
        //    mockSet.As<IQueryable<Actor>>().Setup(m => m.Provider).Returns(actors.Provider);
        //    mockSet.As<IQueryable<Actor>>().Setup(m => m.Expression).Returns(actors.Expression);
        //    mockSet.As<IQueryable<Actor>>().Setup(m => m.ElementType).Returns(actors.ElementType);
        //    mockSet.As<IQueryable<Actor>>().Setup(m => m.GetEnumerator()).Returns(actors.GetEnumerator());
        //    _mockContext.Setup(c => c.Actors).Returns(mockSet.Object);

        //    // Act & Assert
        //    Assert.IsFalse(await _service.ActorExistsAsync(999));
        //}


        //[Test]s
        //public async Task AllActorsAsync_ReturnsAllActors()
        //{
        //    // Arrange similar to ActorExistsAsync, but return a list of actors

        //    var actors = new List<Actor>
        //    {
        //       new Actor { Id = 1, ActorName = "John Doe" },
        //       new Actor { Id = 2, ActorName = "Jabe Eod" }
        //    }.AsQueryable();

        //    // Act
        //    var result = await _service.AllActorsAsync();

        //    // Assert
        //    Assert.That(result.Count(), Is.EqualTo(2)); // assuming 2 actors in the setup
        //}

        //[Test]
        //public async Task AllActorsNamesAsync_ReturnsDistinctActorNames()
        //{
        //    // Arrange & Act similar to above, specifically testing distinct functionality
        //    var actors = new List<Actor>
        //    {
        //       new Actor { Id = 1, ActorName = "John Doe" },
        //       new Actor { Id = 2, ActorName = "Jabe Eod" }
        //    }.AsQueryable();

        //    //Act


        //    // Assert
        //    Assert.AreEqual(expectedNames.Count, result.Count());
        //}
    }
}
