using Microsoft.AspNetCore.Mvc;
using webkino.Services.Interfaces;
using webkino.ViewModels;

namespace webkino.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IStudioService _studioService;

        public AdminController(IMovieService movieService, IStudioService studioService)
        {
            _movieService = movieService;
            _studioService = studioService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        public IActionResult Dashboard()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Movies()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var movies = _movieService.GetAll();
            return View(movies);
        }

        [HttpGet]
        public IActionResult CreateMovie()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new AdminCreateMoviePageViewModel
            {
                Studios = _studioService.GetOptions()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateMovie(AdminCreateMoviePageViewModel model)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            model.Studios = _studioService.GetOptions();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = _movieService.Create(model.Movie, out string errorMessage);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }

            TempData["AdminSuccess"] = "Фільм успішно додано.";
            return RedirectToAction("Movies");
        }

        [HttpGet]
        public IActionResult EditMovie(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var movie = _movieService.GetEditById(id);
            if (movie == null)
            {
                return NotFound();
            }

            var model = new AdminEditMoviePageViewModel
            {
                Movie = movie,
                Studios = _studioService.GetOptions()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditMovie(AdminEditMoviePageViewModel model)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            model.Studios = _studioService.GetOptions();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = _movieService.Update(model.Movie, out string errorMessage);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }

            TempData["AdminSuccess"] = "Фільм успішно оновлено.";
            return RedirectToAction("Movies");
        }

        [HttpPost]
        public IActionResult DeleteMovie(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var success = _movieService.Delete(id, out string errorMessage);

            if (!success)
            {
                TempData["AdminError"] = errorMessage;
            }
            else
            {
                TempData["AdminSuccess"] = "Фільм успішно видалено.";
            }

            return RedirectToAction("Movies");
        }
    }
}