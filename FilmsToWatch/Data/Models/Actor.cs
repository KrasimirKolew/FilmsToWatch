using System.ComponentModel.DataAnnotations;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Data.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ActorNameMaxLen)]
        public string ActorName { get; set; } = string.Empty;

        [Required]
        public int FilmsInvolve { get; set; }
    }
}
