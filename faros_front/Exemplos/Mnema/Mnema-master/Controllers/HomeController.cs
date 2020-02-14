using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mnema.Models;

namespace Mnema.Controllers
{
    public class HomeController : Controller
    {
        UserContext db;
        public HomeController(UserContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var users = db.Users.Include("Photos");
            return View(users.ToList());
        }


    }
}
