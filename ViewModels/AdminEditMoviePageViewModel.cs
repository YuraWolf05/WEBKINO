namespace webkino.ViewModels
{
    public class AdminEditMoviePageViewModel
    {
        public EditMovieViewModel Movie { get; set; } = new();

        public List<StudioOptionViewModel> Studios { get; set; } = new();
    }
}