using System.ComponentModel.DataAnnotations;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Models.UserModels
{
    public class UserToRoleViewModel : AddRoleViewModel
    {
        public int RoleId { get; set; }

        [Required]
        [StringLength(UserEmailMaxLenght, MinimumLength = UserEmailMinLenght, ErrorMessage = LenghtMessage)]
        public string UserName { get; set; } = string.Empty;

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
