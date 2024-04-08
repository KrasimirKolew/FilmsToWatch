using System.ComponentModel.DataAnnotations;

namespace FilmsToWatch.Models.FilmModels
{
    public class FilmFormModel
    {
        [Required]

        public string Title { get; set; } = string.Empty;

        public string MovieImage { get; set; } = string.Empty;

        [Required]
        public string ReleaseYear { get; set; } = string.Empty;

        [Required]

        public string Director { get; set; } = string.Empty;

        [Required]
        public int GenreId { get; set; }

        [Required]
        public int ActorId { get; set; }

        public IEnumerable<FilmGenreServiceModel> Genres { get; set; } =
            new List<FilmGenreServiceModel>();

        public IEnumerable<FilmActorServiceModel> Actors { get; set; } =
            new List<FilmActorServiceModel>();

    }
}
