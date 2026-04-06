using System.ComponentModel.DataAnnotations;

namespace webkino.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int ReleaseYear { get; set; }

        public double Rating { get; set; }

        public string? VideoUrl { get; set; }

        public int StudioId { get; set; }

        public Studio? Studio { get; set; }

        public List<Comment> Comments { get; set; } = new();
    }
}