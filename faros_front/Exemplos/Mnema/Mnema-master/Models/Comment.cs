using System;
namespace Mnema.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public User User { get; set; }
        public Photo Photo { get; set; }
    }
}
