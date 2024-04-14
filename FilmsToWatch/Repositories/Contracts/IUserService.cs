using Microsoft.AspNetCore.Identity;

namespace FilmsToWatch.Repositories.Contracts
{
    public interface IUserService
    {
        IdentityRole CreateRole(string roleName);
    }
}
