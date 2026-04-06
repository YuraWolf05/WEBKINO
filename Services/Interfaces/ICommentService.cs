using webkino.ViewModels;

namespace webkino.Services.Interfaces
{
    public interface ICommentService
    {
        List<CommentViewModel> GetByMovieId(int movieId);

        bool AddComment(CreateCommentViewModel model, out string errorMessage);
    }
}