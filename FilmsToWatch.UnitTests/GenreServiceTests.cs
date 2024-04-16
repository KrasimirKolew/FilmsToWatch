using FilmsToWatch.Controllers;
using FilmsToWatch.Data;
using FilmsToWatch.Data.Models;
using FilmsToWatch.Repositories.Contracts;
using FilmsToWatch.Repositories.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Packaging.Core;

namespace FilmsToWatch.UnitTests
{
    [TestFixture]
    public class GenreServiceTests
    {

        [SetUp]
        public void SetUp()
        {
         
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


    }
}