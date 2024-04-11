using FilmsToWatch.Data.Models;
using FilmsToWatch.Extensions;
using FilmsToWatch.Models.FilmModels;
using FilmsToWatch.Repositories.Contracts;
using FilmsToWatch.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FilmsToWatch.Controllers
{
    public class FilmController : Controller
    {
        private readonly IFilmService _filmService;
        private readonly IGenreService _genreService;
        private readonly IFileService _fileService;

        public FilmController(IFilmService filmService,
            IGenreService genService,
            IFileService fileService)
        {
            _filmService = filmService;
            _genreService = genService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery]AllFilmsQueryModel query)
        {
            var model = await _filmService.AllAsync(
                query.Genre,
                query.Actor,
                query.SearchTerm,
                query.CurrentPage,
                query.FilmsPerPage);

            query.TotalFilmsCount = model.TotalFilmsCount;
            query.Films = model.Films;
            query.Genres = await _filmService.AllGenresNamesAsync();
            query.Actors = await _filmService.AllActorsNamesAsync();

            return View(query);
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new FilmFormModel()
            {
                Genres = await _filmService.AllGenresAsync(),
                Actors = await _filmService.AllActorsAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(FilmFormModel model)
        {

            if (ModelState.IsValid == false)
            {
                model.Genres = await _filmService.AllGenresAsync();
                model.Actors = await _filmService.AllActorsAsync();

                return View(model);
            }


            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MovieImage = imageName;
            }

            var result = await _filmService.AddFilmAsync(model);
            TempData["msg"] = "Added Successfully";

            return RedirectToAction(nameof(Add));

        }
    }
}
