using System.ComponentModel.DataAnnotations;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Models.FilmModels
{
    public class FilmFormModel
    {

        [Required(ErrorMessage = RequiredMesage)]
        [StringLength(TitleMaxLen,
            MinimumLength = TitleMinLen,
            ErrorMessage = LenghtMessage)]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Image")]
        public string MovieImage { get; set; } = string.Empty;


        [Required(ErrorMessage = RequiredMesage)]
        public string ReleaseYear { get; set; } = string.Empty;


        [Required(ErrorMessage = RequiredMesage)]
        [StringLength(DirectorMaxLen,
            MinimumLength = DirectorMinLen,
            ErrorMessage = LenghtMessage)]
        public string Director { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMesage)]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }


        [Required(ErrorMessage = RequiredMesage)]
        [Display(Name = "Actor")]
        public int ActorId { get; set; }


        public IEnumerable<FilmGenreServiceModel> Genres { get; set; } =
            new List<FilmGenreServiceModel>();

        public IEnumerable<FilmActorServiceModel> Actors { get; set; } =
            new List<FilmActorServiceModel>();

    }
}
