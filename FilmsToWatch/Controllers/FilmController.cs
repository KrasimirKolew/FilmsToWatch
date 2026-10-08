using FilmsToWatch.Models.FilmModels;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Details(int id)
        {
            if (await _filmService.ExistsAsync(id) == false)
            {
                return NotFound();
            }

            var model = await _filmService.FilmDetailsByIdAsync(id);

            return View(model);
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Add(FilmFormModel model)
        {

            ModelState.Remove(nameof(model.MovieImage));

            if (await _filmService.GenreExistsAsync(model.GenreId) == false)
            {
                ModelState.AddModelError(nameof(model.GenreId), "Genre does not exist");
            }

            if (await _filmService.ActorExistsAsync(model.ActorId) == false)
            {
                ModelState.AddModelError(nameof(model.ActorId), "Actor does not exist. Add an actor first.");
            }

            if (ModelState.IsValid == false)
            {
                model.Genres = await _filmService.AllGenresAsync();
                model.Actors = await _filmService.AllActorsAsync();
                return View(model);
            }

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileResult = await _fileService.SaveImageAsync(model.ImageFile);
                if (fileResult.Success == false)
                {
                    ModelState.AddModelError(nameof(model.ImageFile), fileResult.ErrorMessage);
                    model.Genres = await _filmService.AllGenresAsync();
                    model.Actors = await _filmService.AllActorsAsync();
                    return View(model);
                }
                model.MovieImage = fileResult.FileName;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _filmService.AddFilmAsync(model, userId);
            TempData["msg"] = "Added Successfully";

            return RedirectToAction(nameof(Add));

        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var film = await _filmService.GetFilmByIdAsync(id);

            if (film == null)
            {
                return BadRequest();
            }

            var model = await _filmService.GetFilmFormModelByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, FilmFormModel model)
        {

            var film = await _filmService.GetFilmByIdAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            // Uploading a new image is optional when editing
            ModelState.Remove(nameof(model.MovieImage));
            ModelState.Remove(nameof(model.ImageFile));

            if (await _filmService.GenreExistsAsync(model.GenreId) == false)
            {
                ModelState.AddModelError(nameof(model.GenreId), "Genre does not exist");
            }

            if (await _filmService.ActorExistsAsync(model.ActorId) == false)
            {
                ModelState.AddModelError(nameof(model.ActorId), "Actor does not exist");
            }

            // Keep the current image unless a new one is uploaded
            var oldImage = film.MovieImage;
            model.MovieImage = oldImage;

            if (ModelState.IsValid == false)
            {
                model.Genres = await _filmService.AllGenresAsync();
                model.Actors = await _filmService.AllActorsAsync();
                return View(model);
            }

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileResult = await _fileService.SaveImageAsync(model.ImageFile);
                if (fileResult.Success == false)
                {
                    ModelState.AddModelError(nameof(model.ImageFile), fileResult.ErrorMessage);
                    model.Genres = await _filmService.AllGenresAsync();
                    model.Actors = await _filmService.AllActorsAsync();
                    return View(model);
                }
                model.MovieImage = fileResult.FileName;
            }

            await _filmService.EditFilmAsync(id, model);

            // A new image replaced the old one, so remove the old file
            if (model.MovieImage != oldImage && !string.IsNullOrEmpty(oldImage))
            {
                _fileService.DeleteImage(oldImage);
            }

            return RedirectToAction(nameof(All));

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
                return RedirectToAction(nameof(WatchedFilms));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while marking the film as watched.");
                return BadRequest();
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
