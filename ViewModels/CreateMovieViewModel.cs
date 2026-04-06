using System.ComponentModel.DataAnnotations;

namespace webkino.ViewModels
{
    public class CreateMovieViewModel
    {
        [Required(ErrorMessage = "Введіть назву фільму")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(1900, 2100, ErrorMessage = "Введіть коректний рік")]
        public int ReleaseYear { get; set; }

        [Range(0, 10, ErrorMessage = "Рейтинг має бути від 0 до 10")]
        public double Rating { get; set; }

        public string? VideoUrl { get; set; }

        [Required(ErrorMessage = "Оберіть студію")]
        public int StudioId { get; set; }
    }
}