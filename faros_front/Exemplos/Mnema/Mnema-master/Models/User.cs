using System;
using System.Collections.Generic;

namespace Mnema.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public  List<Photo> Photos { get; set; }
        public byte[] Avatar { get; set; }

    }
}
