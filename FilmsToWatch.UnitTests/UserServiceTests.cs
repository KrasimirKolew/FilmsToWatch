using FilmsToWatch.Data;
using FilmsToWatch.Repositories.Contracts;
using FilmsToWatch.Repositories.Services;
using Microsoft.EntityFrameworkCore;

namespace FilmsToWatch.UnitTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService userService;
        private ApplicationDbContext context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDataBase")
                .Options;

            context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Test]
        public void CreateRole_ReturnsRoleWithCorrectProperties()
        {
            userService = new UserService();

            var roleName = "Admin";

            var role = userService.CreateRole(roleName);

            Assert.IsNotNull(role);
            Assert.That(role.Name, Is.EqualTo(roleName));
            Assert.That(role.NormalizedName, Is.EqualTo(roleName.ToUpper()));
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
