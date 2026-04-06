using System.ComponentModel.DataAnnotations;

namespace webkino.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}