using System.ComponentModel.DataAnnotations;

namespace FilmsToWatch.Models.FilmModels
{
    public class FilmDetailsServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public string Genre { get; set; } = string.Empty;

        public string Actor { get; set; } = string.Empty;

        public string MovieImage { get; set; } = string.Empty;

        public string ReleaseYear { get; set; } = string.Empty;

        public string Director { get; set; } = string.Empty;
    }
}
