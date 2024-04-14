using FilmsToWatch.Data.Models;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsToWatch.Controllers
{
    public class GenreController : BaseController
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Add(Genre model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _genreService.AddAsync(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _genreService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Genre model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            var result = await _genreService.UpdateAsync(model);
            if (result)
            {
                TempData["msg"] = "Updated Successfully";
                return RedirectToAction(nameof(GenreList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View("Edit", model); 
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GenreList()
        {
            var genres = await _genreService.ListAsync();
            return View(genres);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _genreService.DeleteAsync(id);
            return RedirectToAction(nameof(GenreList));
        }
    }
}
