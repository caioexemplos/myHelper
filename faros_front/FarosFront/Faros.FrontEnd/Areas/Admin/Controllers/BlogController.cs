using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Faros.Common;
using Faros.Common.Constants;
using Faros.Common.Enums;
using Faros.Common.Helpers;
using Faros.FrontEnd.Models;
using Faros.FrontEnd.Proxy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Faros.FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        #region >>> Variáveis <<<

        private ArquivoProxy _servicoImagem;
        private UsuarioProxy _servicoUsuario;
        private PostProxy _servicoPost;
        private CategoriaPostProxy _servicoCategoriaPost;
        private string _imagePath;
        IConfiguration _configurationOutPut;
        IHostingEnvironment _appEnviroment;
        public const int TAMANHO_PAGINA = 4;

        #endregion

        #region >>> Construtor <<<
        public BlogController(IConfiguration configurationInput,
                                      IHostingEnvironment environment)
        {
            _configurationOutPut = configurationInput;
            _appEnviroment = environment;
            _imagePath = _appEnviroment.ContentRootPath + AssemblyConstants.GetPathImageByBuild(FileConstants.DEV, AssemblyConstants.PATH_IMAGE_BLOG);

            var uriHost = _configurationOutPut.GetValue<string>("UriHost");

            _servicoImagem = new ArquivoProxy(uriHost);
            _servicoUsuario = new UsuarioProxy(uriHost);
            _servicoPost = new PostProxy(uriHost);
            _servicoCategoriaPost=new CategoriaPostProxy(uriHost);
        }
        #endregion

        public IActionResult Index()
        {

            var posts = _servicoPost.Get();
            return View("Index", posts);
        }



        #region >>> Insert Methods <<<

        public IActionResult Create()
        {
            ViewBag.Usuarios = _servicoUsuario.Get();
            ViewBag.CategoriasPost = _servicoCategoriaPost.Get();

            var post = HttpContext.Session.GetObject<PostDTO>("TransientBlogFoto");

            return View("Create", post);
        }

        [HttpPost]
        public IActionResult Create(PostDTO post)
        {
            var myPost = post;
          
            return View("Index");
        }

        [HttpPost("Blog/Insert")]
        public IActionResult Insert(PostDTO post)
        {
            PostDTO retorno = null;

            int posicao = _servicoImagem.GetLastFilePosition((int)SubCategoriaArquivoEnum.FOTO_BLOG_POST) + 1;

            post.FotoPost.ContentType = "image/jpeg";
            post.FotoPost.Posicao = posicao;
            post.FotoPost.SubCategoriaArquivo = new SubCategoriaArquivoDTO()
            {
                Id = (int)SubCategoriaArquivoEnum.FOTO_BLOG_POST,
                CategoriaArquivo = new CategoriaArquivoDTO() { Id = (int)CategoriaArquivoEnum.FOTO }
            };
            post.FotoPost.Tamanho = "1500x500";
            post.FotoPost.Nome = "blog" + "_" + posicao;



            if (post != null)
                retorno = _servicoPost.Post(post);


            if (retorno != null)
                HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

        #endregion

        #region >>> Edit Methods <<<
        public IActionResult Edit(int id)
        {
            var post = _servicoPost.GetById(id);
            HttpContext.Session.SetInt32("IdArquivo", post.FotoPost.Id);
            ViewBag.Usuarios = _servicoUsuario.Get();
            ViewBag.CategoriasPost = _servicoCategoriaPost.Get();
            return View("Edit", post);
        }

        [HttpPost("Blog/Update")]
        public IActionResult Update(int id, PostDTO post)
        {
            bool isTrue;
            post.FotoPost = new ArquivoDTO() { Id = HttpContext.Session.GetInt32("IdArquivo").Value };
            if (post != null)
            {
                post.Id = id;
                isTrue = _servicoPost.Put(post.Id, post);
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region >>> Delete Methods <<<
        public IActionResult Delete(int id)
        {
            var animal = _servicoPost.GetById(id);
            return View("Delete", animal);
        }

        public IActionResult DeletePost(int id)
        {
            _servicoPost.Delete(id);

            return RedirectToAction("Index");
        }

        #endregion

        #region >>> Details Methods<<<
        public IActionResult Details(int id)
        {
            var model = _servicoPost.GetById(id);
            return View("Details", model);
        }

        #endregion

        #region Photo Manager

        [HttpPost("Blog/UploadPhoto")]
        public IActionResult UploadPhoto(IFormFile file)
        {

            if (file != null)
            {
                int posicao = _servicoImagem.GetLastFilePosition((int)SubCategoriaArquivoEnum.FOTO_BLOG_POST) + 1;

                SetFile(file, posicao);

            }
            return RedirectToAction("Create", false);

        }

        [HttpPost("Blog/UploadPhotoEdit")]
        public IActionResult UploadPhotoEdit(IFormFile file,int animalId)
        {

            if (file != null)
            {
                int posicao = _servicoImagem.GetLastFilePosition((int)SubCategoriaArquivoEnum.FOTO_BLOG_POST) + 1;

                SetFile(file, posicao);

            }
            return RedirectToAction("Edit", new { id = animalId });

        }

        private void SetFile(IFormFile file, int posicao)
        {
            MemoryStream ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);


            var post = new PostDTO()
            {
                FotoPost = new ArquivoDTO()
                {
                    ContentType = file.ContentType,
                    NomeSalvar = file.FileName,
                    ArquivoArray = ms.ToArray(),
                    Path = ArquivoHelper.GetPathExetension(AssemblyConstants.
                                                            GetPathImageByBuild(FileConstants.PROD, AssemblyConstants.PATH_IMAGE_BLOG)
                                                            , file.FileName, "blog"+"_"+posicao)
                }
            };

            HttpContext.Session.SetObject("TransientBlogFoto", post);

            SalvarFoto(post.FotoPost, posicao);
        }

        public bool SalvarFoto(ArquivoDTO arquivoModel, int posicao)
        {
            try
            {


                Arquivo arquivo = new Arquivo()
                {
                    ContentType = arquivoModel.ContentType,
                    Posicao = posicao,
                    Nome = "blog" + "_" + posicao,
                    CategoriaArquivoId = (int)CategoriaArquivoEnum.FOTO,
                    SubCategoriaArquivoId = (int)SubCategoriaArquivoEnum.FOTO_BLOG_POST,
                    NomeSalvar = arquivoModel.NomeSalvar,
                    Tamanho = "1500x500",
                    ArquivoArray = arquivoModel.ArquivoArray
                };

                ArquivoHelper.SalvarArquivo(arquivo, _imagePath);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        [HttpGet]
        public FileStreamResult VerImagemNovo()
        {
            var blogCache = HttpContext.Session.GetObject<PostDTO>("TransientBlogFoto");

            if (blogCache != null)
                HttpContext.Session.Clear();

            if (blogCache != null && blogCache.FotoPost != null
                && blogCache.FotoPost.ArquivoArray == null)
                blogCache.FotoPost.ArquivoArray = new byte[0];
            else if (blogCache == null || blogCache.FotoPost == null
                || blogCache.FotoPost.ArquivoArray == null)
                return new FileStreamResult(new MemoryStream(), "image/jpeg");


            return new FileStreamResult(new MemoryStream(blogCache.FotoPost.ArquivoArray), "image/jpeg");

        }

        #endregion
    }
}