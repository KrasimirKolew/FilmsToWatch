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
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void SetUp()
        {

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Make sure each test run uses a new db
            .Options;

            _context = new ApplicationDbContext(_options);

            _context.Actors.AddRange(
                    new Actor { Id = 1, ActorName = "Actor1", FilmsInvolve = 5 },
                    new Actor { Id = 2, ActorName = "Actor2", FilmsInvolve = 5 }
                );
            _context.SaveChanges();
        }

        [Test]
        public async Task AddFilmAsync_GivenNewGenre_ShouldAddFilm()
        {
            // Arrange
            var actorService = new ActorService(_context);

            var result = await actorService.AddAsync(new Actor()
            {
                Id = 3,
                ActorName = "Test",
                FilmsInvolve = 5
            });

            var dbActor = await actorService.GetByIdAsync(3);

            Assert.IsTrue(result);
            Assert.That(dbActor.ActorName, Is.EqualTo("Test"));
            Assert.That(dbActor.FilmsInvolve, Is.EqualTo(5));

        }

        [Test]
        public async Task DeleteAsync_DeletesExistingActor()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ActorService(context);
                var actorId = 3;

                // Act
                var result = await service.DeleteAsync(actorId);

                // Assert
                Assert.IsTrue(result);
                Assert.IsNull(await context.Actors.FindAsync(actorId));
            }
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalseForNonExistingActor()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ActorService(context);
                var nonExistingActorId = 99; // Assuming actor with ID 99 does not exist

                // Act
                var result = await service.DeleteAsync(nonExistingActorId);

                // Assert
                Assert.IsFalse(result);
            }
        }
        [Test]
        public async Task UpdateAsync_UpdatesExistingActor()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ActorService(context);
                var actorToUpdate = await context.Actors.FindAsync(1);
                actorToUpdate.ActorName = "UpdatedActorName";

                // Act
                var result = await service.UpdateAsync(actorToUpdate);

                // Assert
                Assert.IsTrue(result);
                var updatedActor = await context.Actors.FindAsync(1);
                Assert.AreEqual("UpdatedActorName", updatedActor.ActorName);
            }
        }

        [Test]
        public async Task UpdateAsync_ReturnsFalseForNonExistingActor()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ActorService(context);
                var nonExistingActor = new Actor { Id = 99, ActorName = "NonExistingActor" }; // Assuming actor with ID 99 does not exist

                // Act
                var result = await service.UpdateAsync(nonExistingActor);

                // Assert
                Assert.IsFalse(result);
            }
        }
        [Test]
        public async Task ListAsync_ReturnsAllActors()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new ActorService(context);

                // Act
                var actors = await service.ListAsync();

                // Assert
                Assert.IsNotNull(actors);
                Assert.AreEqual(2, actors.Count);
            }
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
