using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fiver.Mvc.Routing.Controllers
{
    public class MobileController : Controller
    {
        public IActionResult Index()
        {
            var url = Url.Action("Index"); // /mobile
            return Content($"Mobile/Index (Url: {url})");
        }

        public IActionResult PageOne()
        {
            var url = Url.Action("PageOne"); // /mobile/PageOne
            return Content($"Mobile/One (Url: {url})");
        }

        [HttpGet]
        public IActionResult PageTwo()
        {
            var url = Url.Action("PageTwo"); // /mobile/PageTwo OR /mobile/PageTwo/1?
            return Content($"(GET) Mobile/Two (Url: {url})");
        }

        [HttpPost]
        public IActionResult PageTwo(int id)
        {
            var url = Url.Action("PageTwo"); // /mobile/PageTwo/1
            return Content($"(POST) Mobile/Two: {id} (Url: {url})");
        }
        
        public IActionResult PageThree()
        {
            var url = Url.RouteUrl("goto_two", new { id = 5 }); // /two/5
            return Content($"Mobile/Three (Url: {url})");
        }

        public IActionResult PageFour()
        {
            var url = Url.RouteUrl("goto_two", new { q = 5 }); // /two?q=5
            return Content($"Mobile/Four (Url: {url})");
        }

        public IActionResult PageFive()
        {
            return RedirectToAction("PageSix");
        }

        public IActionResult PageSix()
        {
            return Content("Mobile/Six (Mobile/Five will also come here)");
        }
    }
}
