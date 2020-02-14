using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Faros.FrontEnd.Models;
using Faros.FrontEnd.Proxy;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.IO;
using Core.Common;
using Faros.Common.Helpers;
using System.Net.Http.Headers;
using Faros.Common.Constants;
using Faros.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Faros.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration _iConfiguration;
        ArquivoProxy _serviceArquivo;

        public HomeController(IConfiguration iconfiguration)
        {
            _iConfiguration = iconfiguration;
            _serviceArquivo = new ArquivoProxy(_iConfiguration.GetValue<string>("UriHost"));
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetString("test", "Session Value");
            var fotos = _serviceArquivo.GetArquivosBySubCategoria((int)SubCategoriaArquivoEnum.FOTO_CARROUSSEL);
            return View(fotos);
        }

        public IActionResult About()
        {

            var hehe = HttpContext.Session.GetString("test");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContatoDTO contato)
        {

            new ContatoProxy(_iConfiguration.GetValue<string>("UriHost")).Post(contato);
            return View();
        }


        


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
