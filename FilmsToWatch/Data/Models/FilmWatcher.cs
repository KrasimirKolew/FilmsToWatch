using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmsToWatch.Data.Models
{
    public class FilmWatcher
    {
        [Required]
        public string HelperId { get; set; } = string.Empty;

        [ForeignKey(nameof(HelperId))]
        public IdentityUser Helper { get; set; } = null!;

        [Required]
        public int FilmId { get; set; }

        [ForeignKey(nameof(FilmId))]
        public Film Films { get; set; } = null!;
    }
}
