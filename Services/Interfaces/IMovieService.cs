using webkino.ViewModels;

namespace webkino.Services.Interfaces
{
    public interface IMovieService
    {
        List<MovieViewModel> GetAll();

        MovieDetailsViewModel? GetById(int id);

        bool Create(CreateMovieViewModel model, out string errorMessage);

        EditMovieViewModel? GetEditById(int id);

        bool Update(EditMovieViewModel model, out string errorMessage);

        bool Delete(int id, out string errorMessage);
    }
}