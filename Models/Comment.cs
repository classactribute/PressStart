using System.ComponentModel.DataAnnotations;

namespace PressStart.Models
{
    public class Comment
    {
        public int CommentId {get; set;}

        [Required]
        public int UserId {get; set;}

        [Required]
        public int GameId {get; set;}
    }
}