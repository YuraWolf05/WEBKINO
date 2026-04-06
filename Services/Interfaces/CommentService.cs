using Microsoft.EntityFrameworkCore;
using webkino.Data;
using webkino.Models.Entities;
using webkino.Services.Interfaces;
using webkino.ViewModels;

namespace webkino.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CommentViewModel> GetByMovieId(int movieId)
        {
            return _context.Comments
                .Include(c => c.User)
                .Where(c => c.MovieId == movieId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentViewModel
                {
                    Username = c.User != null ? c.User.Username : "Невідомий користувач",
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                })
                .ToList();
        }

        public bool AddComment(CreateCommentViewModel model, out string errorMessage)
        {
            errorMessage = string.Empty;

            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");

            if (userId == null)
            {
                errorMessage = "Щоб залишити коментар, потрібно увійти в акаунт.";
                return false;
            }

            var movieExists = _context.Movies.Any(m => m.Id == model.MovieId);
            if (!movieExists)
            {
                errorMessage = "Фільм не знайдено.";
                return false;
            }

            var comment = new Comment
            {
                MovieId = model.MovieId,
                UserId = userId.Value,
                Content = model.Content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return true;
        }
    }
}