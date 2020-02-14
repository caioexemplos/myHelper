using System.Linq;
using AuthApp.Models;
using Mnema.Models;

namespace Mnema
{
    public static class DataInit
    {
        public static void Initialize(UserContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Name = "Alina",
                        Email = "Alina@gmail.com",
                        Password = "alina",
                    },
                     new User
                     {
                         Name = "Gogi",
                         Email = "Gogi@gmail.com",
                         Password = "gogi",
                     }
                );
                context.SaveChanges();
            }
        }
    }
}