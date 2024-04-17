using FilmsToWatch.Data.Models;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Controllers
{
    public class ActorController : BaseController
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Add(Actor model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _actorService.AddAsync(model);
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

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _actorService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Actor model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            var result = await _actorService.UpdateAsync(model);
            if (result)
            {
                TempData["msg"] = "Updated Successfully";
                return RedirectToAction(nameof(ActorList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View("Edit", model);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ActorList()
        {
            var genres = await _actorService.ListAsync();
            return View(genres);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _actorService.DeleteAsync(id);
            return RedirectToAction(nameof(ActorList));
        }
    }
}
