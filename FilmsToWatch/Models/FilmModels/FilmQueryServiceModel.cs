using FilmsToWatch.Data.Models;

namespace FilmsToWatch.Models.FilmModels
{
    public class FilmQueryServiceModel
    {
        public int TotalFilmsCount { get; set; }

        public IEnumerable<FilmServiceModel> Films { get; set; } = new List<FilmServiceModel>();
    }
}
