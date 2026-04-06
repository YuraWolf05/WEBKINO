namespace webkino.ViewModels
{
    public class MovieDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int ReleaseYear { get; set; }

        public double Rating { get; set; }

        public string StudioName { get; set; } = string.Empty;

        public string StudioCountry { get; set; } = string.Empty;

        public string? VideoUrl { get; set; }

        public List<CommentViewModel> Comments { get; set; } = new();

        public CreateCommentViewModel NewComment { get; set; } = new();
    }
}