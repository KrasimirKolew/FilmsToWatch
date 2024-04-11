using System.ComponentModel.DataAnnotations;

namespace FilmsToWatch.Models.FilmModels
{
    public class AllFilmsQueryModel
    {
        public int FilmsPerPage { get; } = 3;

        public string Genre { get; init; } = null!;

        public string Actor { get; init; } = null!;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; } = null!;

        public int CurrentPage { get; init; } = 1;

        public int TotalFilmsCount { get; set; }

        public IEnumerable<string> Genres { get; set; } = null!;

        public IEnumerable<string> Actors { get; set; } = null!;

        public IEnumerable<FilmServiceModel> Films { get; set; } = new List<FilmServiceModel>();
    }
}
