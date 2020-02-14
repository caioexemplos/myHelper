using System;
namespace Mnema.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public User User {get; set;}
        public Photo Photo { get; set; }
    }
}