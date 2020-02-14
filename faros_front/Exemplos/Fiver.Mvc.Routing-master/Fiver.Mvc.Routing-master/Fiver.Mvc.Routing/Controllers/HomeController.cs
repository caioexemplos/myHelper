using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fiver.Mvc.Routing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Home/Index");
        }

        public IActionResult PageOne()
        {
            return Content("Home/One");
        }

        [HttpGet]
        public IActionResult PageTwo()
        {
            return Content("(GET) Home/Two");
        }

        [HttpPost]
        public IActionResult PageTwo(int id)
        {
            return Content($"(POST) Home/Two: {id}");
        }
    }
}
