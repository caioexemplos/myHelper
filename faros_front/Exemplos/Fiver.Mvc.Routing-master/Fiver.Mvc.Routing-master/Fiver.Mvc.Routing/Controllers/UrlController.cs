using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fiver.Mvc.Routing.Controllers
{
    [Route("url")]
    public class UrlController : Controller
    {
        private readonly IUrlHelper urlHelper;

        public UrlController(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public IActionResult Index()
        {
            var links = new List<string>();

            // exclude protocol and host
            links.Add(this.urlHelper.RouteUrl("goto_one", new { }));
            links.Add(this.urlHelper.Action("PageOne", "Home", new { }));

            // include protocol and host
            links.Add(this.urlHelper.Link("goto_one", new { }));

            return Json(links);
        }
    }
}
