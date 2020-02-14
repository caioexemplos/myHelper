using System;
using System.Collections.Generic;

namespace Mnema.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
