using System.ComponentModel.DataAnnotations;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Models.FilmModels
{
    public class FilmServiceModel
    {
        public int Id { get; set; }

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
    }
}
