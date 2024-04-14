using FilmsToWatch.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;

namespace FilmsToWatch.Repositories.Services
{
    public class UserService : IUserService
    {
        public IdentityRole CreateRole(string roleName) => new IdentityRole()
        {
            Name = roleName,
            NormalizedName = roleName.ToUpper()
        };
    }
}
