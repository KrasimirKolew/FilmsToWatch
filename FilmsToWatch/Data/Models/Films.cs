using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Data.Models
{
    public class Films
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLen)]
        public string Title { get; set; } = string.Empty;

        public string MovieImage { get; set; } = string.Empty;

        [Required]
        public string ReleaseYear { get; set; } = string.Empty;

        [Required]
        [MaxLength(DirectorMaxLen)]
        public string Director { get; set; } = string.Empty;

        [Required]
        public int GenreId { get; set; }

        [Required]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        [Required]
        public string FilmAdderId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(FilmAdderId))]
        public IdentityUser FilmAdder { get; set; } = null!;

        public IList<FilmWatcher> FilmsWatchers { get; set; } = new List<FilmWatcher>();

    }
}
