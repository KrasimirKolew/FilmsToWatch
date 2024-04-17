using FilmsToWatch.Extensions;
using FilmsToWatch.Models.ReviewModels;
using FilmsToWatch.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Controllers
{
    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService _reviewService)
        {
            reviewService = _reviewService;
        }

        public async Task<IActionResult> All(int Id)
        {
            ViewBag.Id = Id;
            var model = await reviewService.GetAllReviewsForEventAsync(Id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create(int Id)
        {
            ViewBag.Id = Id;

            var model = new ReviewCreateViewModel();

            model.FilmId = Id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateViewModel model, int Id)
        {

            var userId = User.Id();
            await reviewService.CreateReviewAsync(model, userId, Id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(All), new { Id = Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id, int filmId)
        {
            if ((await reviewService.ExistsAsync(Id)) == false)
            {
                return RedirectToAction(nameof(All), new { Id = filmId });
            }

            var userId = User.Id();
            if (await reviewService.SameUserAsync(Id, userId) == false)
            {
                return RedirectToAction(nameof(All), new { Id = filmId });
            };

            var eventModel = await reviewService.ReviewByIdAsync(Id);

            var model = new ReviewCreateViewModel()
            {
                Id=eventModel.Id,
                Content = eventModel.Content,
                FilmId= filmId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReviewCreateViewModel model, int Id)
        {
            if (Id != model.Id)
            {
                return RedirectToAction(nameof(All), new { Id = model.FilmId });
            }

            if (await reviewService.ExistsAsync(model.Id) == false)
            {
                ModelState.AddModelError("", "Review does not exist");
            }

            if (await reviewService.SameUserAsync(model.Id, User.Id()) == false)
            {
                return RedirectToAction(nameof(All), new { Id = model.FilmId });
            };

            var result = await reviewService.EditAsync(Id, model);

            if (ModelState.IsValid == false)
            {
                return View(model);
            }
              
            return RedirectToAction(nameof(All), new { Id = result });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id, int filmId)
        {
            if ((await reviewService.ExistsAsync(Id) == false))
            {
                return RedirectToAction(nameof(All), new { Id = filmId });
            }

            if (await reviewService.SameUserAsync(Id, User.Id()) == false)
            {
                return RedirectToAction(nameof(All), new { Id = filmId });
            };

            var commentToDelete = await reviewService.ReviewByIdWithUserAsync(Id);

            if(commentToDelete==null)
            {
                return NotFound();
            }

            var model = new ReviewDeleteViewModel()
            {
                Content = commentToDelete.Content,
                UserName = commentToDelete.User.UserName,
                FilmId = filmId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ReviewDeleteViewModel model, int Id)
        {
            if ((await reviewService.ExistsAsync(Id) == false))
            {
                return RedirectToAction(nameof(All), new { Id = model.FilmId });
            }

            if (await reviewService.SameUserAsync(Id, User.Id()) == false)
            {
                return RedirectToAction(nameof(All), new { Id = model.FilmId });
            };

            await reviewService.DeleteAsync(model.Id);

            return RedirectToAction(nameof(All), new { Id = model.FilmId });
        }

    }
}
