using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faros.FrontEnd.Models;
using Faros.FrontEnd.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Faros.FrontEnd.Controllers
{
    public class BlogController : Controller
    {

        #region >>> Variáveis <<<
        private PostProxy _servicePost;
        IConfiguration _configurationOutPut;
        private const int TAMANHO_PAGINA = 4;
        #endregion

        public BlogController(IConfiguration configurationInput)
        {
            _configurationOutPut = configurationInput;
            _servicePost = new PostProxy(_configurationOutPut.GetValue<string>("UriHost"));
        }
        public IActionResult BlogHome()
        {
            var posts = _servicePost.Get();

            return View("BlogHome",posts);
        }

        public IActionResult BlogPost(int postId)
        {
            var post = _servicePost.GetById(postId);
            return View("BlogPost", post);
        }
      
    }
}