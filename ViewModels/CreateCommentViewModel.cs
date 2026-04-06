using System.ComponentModel.DataAnnotations;

namespace webkino.ViewModels
{
    public class CreateCommentViewModel
    {
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Введіть текст коментаря")]
        public string Content { get; set; } = string.Empty;
    }
}