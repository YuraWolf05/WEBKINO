namespace webkino.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int ReleaseYear { get; set; }

        public double Rating { get; set; }

        public string StudioName { get; set; } = string.Empty;

        public string? VideoUrl { get; set; }
    }
}