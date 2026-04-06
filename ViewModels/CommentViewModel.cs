namespace webkino.ViewModels
{
    public class CommentViewModel
    {
        public string Username { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}