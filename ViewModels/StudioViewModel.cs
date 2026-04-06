namespace webkino.ViewModels
{
    public class StudioViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public int MoviesCount { get; set; }
    }
}