using Microsoft.EntityFrameworkCore;
using Mnema.Models;

namespace AuthApp.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }
    }
}