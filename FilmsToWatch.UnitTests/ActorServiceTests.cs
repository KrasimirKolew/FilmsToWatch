using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Models.FilmModels;
using FilmsToWatch.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FilmsToWatch.UnitTests
{
    [TestFixture]
    public class ActorServiceTests
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
            var actorService = new ActorService(_context);

            var result = await actorService.AddAsync(new Actor()
            {
                Id = 1,
                ActorName = "Test",
                FilmsInvolve = 5
            });

            var dbActor = await actorService.GetByIdAsync(1);

            Assert.IsTrue(result);
            Assert.That(dbActor.ActorName, Is.EqualTo("Test"));
            Assert.That(dbActor.FilmsInvolve, Is.EqualTo(5));

            //var actor = new Actor
            //{
            //    Id = 1,
            //    ActorName = "Test",
            //    FilmsInvolve = 5
            //};

            //// Act
            //var result = await actorService.AddAsync(actor);
            //var pencho = await actorService.GetByIdAsync(actor.Id);

            //// Assert
            //var actorInDb = await _context.Actors.FirstOrDefaultAsync(g => g.ActorName == "Test");
            //Assert.IsNotNull(actorInDb);
        }
    }
}
