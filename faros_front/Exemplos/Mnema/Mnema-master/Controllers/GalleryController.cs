using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mnema.Models;

namespace Mnema.Controllers
{
    public class GalleryController : Controller
    {
         UserContext _context;
        public GalleryController(UserContext context)
        {
            _context = context;
        }

       [Authorize]
        public IActionResult Index()
        {
            var photos = _context.Photos.Include(p => p.User);
            return View(photos.ToList());
        }
         public IActionResult ProfileView(int? id)
        {
            if (id != null)
            {
                User user = _context.Users.Include("Photos").FirstOrDefault(u => u.UserId== id);
                if (user != null)
                    if(user.Email == User.Identity.Name){
                        return RedirectToAction("Index","Profile");
                    }
                    return View(user);
            }
            return NotFound();
        }

    }
}
