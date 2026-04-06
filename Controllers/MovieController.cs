using Microsoft.AspNetCore.Mvc;
using webkino.Services.Interfaces;
using webkino.ViewModels;

namespace webkino.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICommentService _commentService;

        public MovieController(IMovieService movieService, ICommentService commentService)
        {
            _movieService = movieService;
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            var movies = _movieService.GetAll();
            return View(movies);
        }

        public IActionResult Details(int id)
        {
            var movie = _movieService.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        public IActionResult AddComment(CreateCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new { id = model.MovieId });
            }

            var success = _commentService.AddComment(model, out string errorMessage);

            if (!success)
            {
                TempData["CommentError"] = errorMessage;
            }

            return RedirectToAction("Details", new { id = model.MovieId });
        }
    }
}