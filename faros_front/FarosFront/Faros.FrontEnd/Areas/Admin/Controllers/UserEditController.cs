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
    public class UserEditController : Controller
    {
        private string _imagePath;
        private ArquivoProxy _servicoImagem;
        private UsuarioProxy _servicoUsuario;
        IConfiguration _configurationOutPut;
        IHostingEnvironment _appEnviroment;


        public UserEditController(IConfiguration configurationInput,
                                      IHostingEnvironment environment)
        {
            _configurationOutPut = configurationInput;
            _appEnviroment = environment;
            _imagePath = _appEnviroment.ContentRootPath + AssemblyConstants.GetPathImageByBuild(FileConstants.DEV, AssemblyConstants.PATH_IMAGE_USER);

            var uriHost = _configurationOutPut.GetValue<string>("UriHost");

            _servicoImagem = new ArquivoProxy(uriHost);
            _servicoUsuario = new UsuarioProxy(uriHost);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        #region >>> Insert Methods <<<

        public IActionResult Create()
        {

            var animal = HttpContext.Session.GetObject<UsuarioDTO>("TransientUserFoto");

            return View("Create", animal);
        }

        


        [HttpPost("UsuarioEdit/Insert")]
        public IActionResult Insert(UsuarioDTO usuario)
        {
            UsuarioDTO retorno = null;

            int posicao = _servicoImagem.GetLastFilePosition((int)SubCategoriaArquivoEnum.FOTO_ALBUM) + 1;

            usuario.FotoUsuario.ContentType = "image/jpeg";
            usuario.FotoUsuario.Posicao = posicao;
            usuario.FotoUsuario.SubCategoriaArquivo = new SubCategoriaArquivoDTO()
            {
                Id = (int)SubCategoriaArquivoEnum.FOTO_ALBUM,
                CategoriaArquivo = new CategoriaArquivoDTO() { Id = (int)CategoriaArquivoEnum.FOTO }
            };
            usuario.FotoUsuario.Tamanho = "140x140";
            usuario.FotoUsuario.Nome = "album_foto_" + posicao;



            if (usuario != null)
                retorno = _servicoUsuario.Post(usuario);


            if (retorno != null)
                HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }



        #endregion

        [HttpPost("UsuarioEdit/UploadPhoto")]
        public IActionResult UploadPhoto(IFormFile file)
        {

            if (file != null)
            {
                int posicao = _servicoImagem.GetLastFilePosition((int)SubCategoriaArquivoEnum.FOTO_ALBUM) + 1;

                SetFile(file, posicao);

            }
            return RedirectToAction("Create", false);

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
                                                            , file.FileName, "user" + "_" + posicao)
                }
            };

            HttpContext.Session.SetObject("TransientUserFoto", post);

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
                    Nome = "user" + "_" + posicao,
                    CategoriaArquivoId = (int)CategoriaArquivoEnum.FOTO,
                    SubCategoriaArquivoId = (int)SubCategoriaArquivoEnum.FOTO_BLOG_POST,
                    NomeSalvar = arquivoModel.NomeSalvar,
                    Tamanho = "140x140",
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
            var blogCache = HttpContext.Session.GetObject<PostDTO>("TransientUserFoto");

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
    }
}