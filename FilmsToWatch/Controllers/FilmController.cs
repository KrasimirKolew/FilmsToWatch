using FilmsToWatch.Data.Models;
using FilmsToWatch.Extensions;
using FilmsToWatch.Models.FilmModels;
using FilmsToWatch.Repositories.Contracts;
using FilmsToWatch.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FilmsToWatch.Controllers
{
    public class FilmController : BaseController
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

        [HttpPost]
        public async Task<IActionResult> MarkAsWatched(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await _filmService.MarkAsWatchedAsync(filmId, userId);
                return RedirectToAction(nameof(WatchedFilms));
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = "You have already marked this film as watched.";
                return RedirectToAction(nameof(WatchedFilms));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while marking the film as watched.");
                return View(); // Return to the current view or an error view.
            }
        }

        [HttpGet]
        public async Task<IActionResult> WatchedFilms()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var watchedFilms = await _filmService.GetWatchedFilmsAsync(userId);
                return View(watchedFilms);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
