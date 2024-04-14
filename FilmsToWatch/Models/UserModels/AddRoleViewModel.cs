using System.ComponentModel.DataAnnotations;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Models.UserModels
{
    public class AddRoleViewModel
    {
        [Required]
        [StringLength(UserRoleNameMaxLenght, MinimumLength = UserRoleNameMinLenght, ErrorMessage = LenghtMessage)]
        public string RoleName { get; set; } = string.Empty;
    }
}
