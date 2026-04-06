using System.ComponentModel.DataAnnotations;

namespace webkino.Models.Entities
{
    public class Studio
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Country { get; set; }

        public List<Movie> Movies { get; set; } = new();
    }
}