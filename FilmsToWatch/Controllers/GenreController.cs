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
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
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

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _genreService.GetByIdAsync(id); // Assuming GetByIdAsync is the async version
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Genre model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model); // Make sure to return to the "Edit" view if validation fails

            var result = await _genreService.UpdateAsync(model); // Assuming UpdateAsync is the async version
            if (result)
            {
                TempData["msg"] = "Updated Successfully";
                return RedirectToAction(nameof(GenreList)); // Assuming GenreList is an action method for listing genres
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View("Edit", model); // Return to the "Edit" view with the model in case of failure
            }
        }

        public async Task<IActionResult> GenreList()
        {
            var genres = await _genreService.ListAsync();
            return View(genres);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _genreService.DeleteAsync(id);
            return RedirectToAction(nameof(GenreList));
        }
    }
}
