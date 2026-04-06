namespace webkino.ViewModels
{
    public class AdminCreateMoviePageViewModel
    {
        public CreateMovieViewModel Movie { get; set; } = new();

        public List<StudioOptionViewModel> Studios { get; set; } = new();
    }
}