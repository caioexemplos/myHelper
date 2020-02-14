using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Mnema.Models;
using System.IO;
using AuthApp.ViewModels;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Mnema.Controllers
{
    public class ProfileController : Controller
    {
        UserContext _context;
        IHostingEnvironment _appEnvironment;


        public ProfileController(UserContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;

        }

       [Authorize]
        public IActionResult Index()
        {
            // var user = from u in _context.Users
            //            where u.Email == User.Identity.Name
            //            select u;
            var user = _context.Users.Include("Photos").FirstOrDefault(u => u.Email == User.Identity.Name);
            return View(user);
        }

        [HttpPost]
         public IActionResult DeletePhoto(int? id)
        {
            if (id != null)
            {
                Photo photo = _context.Photos.FirstOrDefault(p => p.PhotoId== id);
                if (photo != null)
                    _context.Photos.Remove(photo);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Profile");
            }
            return NotFound();
        }

         public async Task<IActionResult> DeleteProfileAsync()
        {
            var user = await _context.Users.Include("Photos").FirstOrDefaultAsync(u => u.Email == User.Identity.Name );
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
            //TODO: delete all photo, likes, comments etc
        }

        [HttpGet]
        public IActionResult UploadPhoto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(UploadPhotoModel model)
        {
            if (model.uploadedFile != null)
            {
                var user = _context.Users.Include("Photos").Where(u => u.Email == User.Identity.Name).First();
                // string path = "/Photos/" + "/" + user.Email;

                //  if (!Directory.Exists(path))
                // {
                //     Directory.CreateDirectory(path);
                // }
                // path = path + "/" + model.uploadedFile.FileName;
                string path = "/Photos/" + model.uploadedFile.FileName;
                // сохраняем файл в папку Photos/userEmail/ в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await model.uploadedFile.CopyToAsync(fileStream);
                }

                var photo = new Photo { Name = model.uploadedFile.FileName,
                                        Path = path ,
                                        Description = model.Description };
                photo.User = user;
                user.Photos.Add(photo);
                _context.Photos.Add(photo);
                _context.SaveChanges();
                }


            return RedirectToAction("Index", "Profile");

        }


        public async Task<IActionResult> EditProfile(int? userid)
        {
            if(userid!=null)
            {
                User user = await _context.Users.FirstAsync(u=>u.UserId==userid);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(User user)
        {
            if (user.Avatar != null)
                 Console.WriteLine("not null");
            System.Console.WriteLine(user.UserId);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Profile");
        }
    }
}
