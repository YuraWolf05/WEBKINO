using Microsoft.EntityFrameworkCore;
using webkino.Data;
using webkino.Models.Entities;
using webkino.Services.Interfaces;
using webkino.ViewModels;

namespace webkino.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;
        private readonly ICommentService _commentService;

        public MovieService(AppDbContext context, ICommentService commentService)
        {
            _context = context;
            _commentService = commentService;
        }

        public List<MovieViewModel> GetAll()
        {
            return _context.Movies
                .Include(m => m.Studio)
                .Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    Rating = m.Rating,
                    StudioName = m.Studio != null ? m.Studio.Name : "Невідома студія",
                    VideoUrl = m.VideoUrl
                })
                .ToList();
        }

        public MovieDetailsViewModel? GetById(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Studio)
                .Where(m => m.Id == id)
                .Select(m => new MovieDetailsViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseYear = m.ReleaseYear,
                    Rating = m.Rating,
                    StudioName = m.Studio != null ? m.Studio.Name : "Невідома студія",
                    StudioCountry = m.Studio != null ? (m.Studio.Country ?? "Невідомо") : "Невідомо",
                    VideoUrl = m.VideoUrl
                })
                .FirstOrDefault();

            if (movie == null)
            {
                return null;
            }

            movie.Comments = _commentService.GetByMovieId(id);
            movie.NewComment = new CreateCommentViewModel
            {
                MovieId = id
            };

            return movie;
        }

        public bool Create(CreateMovieViewModel model, out string errorMessage)
        {
            errorMessage = string.Empty;

            var studioExists = _context.Studios.Any(s => s.Id == model.StudioId);
            if (!studioExists)
            {
                errorMessage = "Обрана студія не знайдена.";
                return false;
            }

            var movie = new Movie
            {
                Title = model.Title,
                Description = model.Description,
                ReleaseYear = model.ReleaseYear,
                Rating = model.Rating,
                VideoUrl = model.VideoUrl,
                StudioId = model.StudioId
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            return true;
        }

        public EditMovieViewModel? GetEditById(int id)
        {
            return _context.Movies
                .Where(m => m.Id == id)
                .Select(m => new EditMovieViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseYear = m.ReleaseYear,
                    Rating = m.Rating,
                    VideoUrl = m.VideoUrl,
                    StudioId = m.StudioId
                })
                .FirstOrDefault();
        }

        public bool Update(EditMovieViewModel model, out string errorMessage)
        {
            errorMessage = string.Empty;

            var movie = _context.Movies.FirstOrDefault(m => m.Id == model.Id);
            if (movie == null)
            {
                errorMessage = "Фільм не знайдено.";
                return false;
            }

            var studioExists = _context.Studios.Any(s => s.Id == model.StudioId);
            if (!studioExists)
            {
                errorMessage = "Обрана студія не знайдена.";
                return false;
            }

            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.ReleaseYear = model.ReleaseYear;
            movie.Rating = model.Rating;
            movie.VideoUrl = model.VideoUrl;
            movie.StudioId = model.StudioId;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id, out string errorMessage)
        {
            errorMessage = string.Empty;

            var movie = _context.Movies
                .Include(m => m.Comments)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                errorMessage = "Фільм не знайдено.";
                return false;
            }

            if (movie.Comments.Any())
            {
                _context.Comments.RemoveRange(movie.Comments);
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return true;
        }
    }
}