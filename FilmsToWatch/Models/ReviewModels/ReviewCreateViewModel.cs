using System.ComponentModel.DataAnnotations;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Models.ReviewModels
{
    public class ReviewCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredMesage)]
        [StringLength(ContentMaxLen,
            MinimumLength = ContentMinLen,
            ErrorMessage = LenghtMessage)]
        public string Content { get; set; } = string.Empty;

        public int FilmId { get; set; }
    }
}
