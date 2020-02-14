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
    public class AnimalEditController : Controller
    {
        #region >>> Variáveis <<<

        private ArquivoProxy _servicoImagem;
        private AnimalProxy _servicoAnimal;
        private string _imagePath;
        IConfiguration _configurationOutPut;
        IHostingEnvironment _appEnviroment;
        public const int TAMANHO_PAGINA = 10;

        #endregion

        #region >>> Construtor<<<

        public AnimalEditController(IConfiguration configurationInput,
                                      IHostingEnvironment environment)
        {
            _configurationOutPut = configurationInput;
            _appEnviroment = environment;
            _imagePath = _appEnviroment.ContentRootPath + AssemblyConstants.GetPathImageByBuild(FileConstants.DEV, AssemblyConstants.PATH_IMAGE_ALBUM);
            _servicoImagem = new ArquivoProxy(_configurationOutPut.GetValue<string>("UriHost"));
            _servicoAnimal = new AnimalProxy(_configurationOutPut.GetValue<string>("UriHost"));
        }
        #endregion

        #region >>> Commom Methods <<<
        public IActionResult Index(int pagina=0)
        {

            var animaisPaginados = _servicoAnimal.GetPaginado(TAMANHO_PAGINA, pagina);
            ViewBag.TamanhoTotal = (int)Math.Ceiling((double)(_servicoAnimal.TotalAnimais() / TAMANHO_PAGINA) + 1);
            ViewBag.PaginaAtual = pagina + 1;

            return View("Index", new List<AnimalDTO>(animaisPaginados));
        }


        private void SetFile(IFormFile file, int posicao)
        {
            MemoryStream ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);


            var animal = new AnimalDTO()
            {
                FotoArquivo = new ArquivoDTO()
                {
                    ContentType = file.ContentType,
                    NomeSalvar = file.FileName,
                    ArquivoArray = ms.ToArray(),
                    Path = ArquivoHelper.GetPathExetension(AssemblyConstants.
                                                            GetPathImageByBuild(FileConstants.PROD, AssemblyConstants.PATH_IMAGE_ALBUM)
                                                            , file.FileName, "album_foto_" + posicao)
                }
            };

            HttpContext.Session.SetObject("TransientAnimalFoto", animal);

            SalvarFoto(animal.FotoArquivo, posicao);
        }

        public bool SalvarFoto(ArquivoDTO arquivoModel, int posicao)
        {
            try
            {


                Arquivo arquivo = new Arquivo()
                {
                    ContentType = arquivoModel.ContentType,
                    Posicao = posicao,
                    Nome = "album_foto_" + posicao,
                    CategoriaArquivoId = (int)CategoriaArquivoEnum.FOTO,
                    SubCategoriaArquivoId = (int)SubCategoriaArquivoEnum.FOTO_ALBUM,
                    NomeSalvar = arquivoModel.NomeSalvar,
                    Tamanho = "500x500",
                    ArquivoArray = arquivoModel.ArquivoArray
                };

                ArquivoHelper.SalvarArquivo(arquivo, _imagePath);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        private void FormatAndSendImages(IEnumerable<Arquivo> lstArquivos)
        {
            var listToTransporter = lstArquivos.Select(x => new ArquivoDTO()
            {
                Id = x.Id,
                ContentType = x.ContentType,
                Nome = x.Nome,
                NomeSalvar = x.NomeSalvar,
                SubCategoriaArquivo = new SubCategoriaArquivoDTO()
                {
                    Id = x.SubCategoriaArquivoId,
                    CategoriaArquivo = new CategoriaArquivoDTO()
                    {
                        Id = x.CategoriaArquivoId
                    }
                },
                Path = x.Path,
                Posicao = x.Posicao,
                Tamanho = x.Tamanho
            });

            _servicoImagem.PostListImagens(listToTransporter);

            HttpContext.Session.Clear();
        }

        [HttpGet]
        public FileStreamResult VerImagemNovo()
        {
            var animalCache = HttpContext.Session.GetObject<AnimalDTO>("TransientAnimalFoto");

            if (animalCache != null && animalCache.FotoArquivo != null
                && animalCache.FotoArquivo.ArquivoArray == null)
                animalCache.FotoArquivo.ArquivoArray = new byte[0];
            else if (animalCache == null || animalCache.FotoArquivo == null
                || animalCache.FotoArquivo.ArquivoArray == null)
                return new FileStreamResult(new MemoryStream(), "image/jpeg");


            return new FileStreamResult(new MemoryStream(animalCache.FotoArquivo.ArquivoArray), "image/jpeg");

        }
        #endregion

        #region >>> Insert Methods <<<

        public IActionResult Create()
        {

            var animal = HttpContext.Session.GetObject<AnimalDTO>("TransientAnimalFoto");

            return View("Create", animal);
        }

        [HttpPost("AnimalEdit/UploadPhoto")]
        public IActionResult UploadPhoto(IFormFile file)
        {

            if (file != null)
            {
                int posicao = _servicoImagem.GetLastFilePosition((int)SubCategoriaArquivoEnum.FOTO_ALBUM) + 1;

                SetFile(file, posicao);

            }
            return RedirectToAction("Create", false);

        }

        [HttpPost("AnimalEdit/Insert")]
        public IActionResult Insert(AnimalDTO animal)
        {
            AnimalDTO retorno = null;

            int posicao = _servicoImagem.GetLastFilePosition((int)SubCategoriaArquivoEnum.FOTO_ALBUM) + 1;

            animal.FotoArquivo.ContentType = "image/jpeg";
            animal.FotoArquivo.Posicao = posicao;
            animal.FotoArquivo.SubCategoriaArquivo = new SubCategoriaArquivoDTO()
            {
                Id = (int)SubCategoriaArquivoEnum.FOTO_ALBUM,
                CategoriaArquivo = new CategoriaArquivoDTO() { Id = (int)CategoriaArquivoEnum.FOTO }
            };
            animal.FotoArquivo.Tamanho = "500x500";
            animal.FotoArquivo.Nome = "album_foto_" + posicao;



            if (animal != null)
                retorno = _servicoAnimal.Post(animal);


            if (retorno != null)
                HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }



        #endregion

        #region >>> Edit Methods <<<

        public IActionResult Edit(int id)
        {
            var animal = _servicoAnimal.GetById(id);
            HttpContext.Session.SetInt32("IdArquivo", animal.FotoArquivo.Id);
            return View("Edit", animal);
        }

        [HttpPost("AnimalEdit/UploadPhotoEdit")]
        public IActionResult UploadPhotoEdit(IFormFile file, int posicao, int idAnimal)
        {
            if (file != null)
            {
                SetFile(file, posicao);
            }

            return RedirectToAction("Edit", new { id = idAnimal });
        }

        [HttpPost("AnimalEdit/Update")]
        public IActionResult Update(int id, AnimalDTO animal)
        {
            bool isTrue;
            animal.FotoArquivo = new ArquivoDTO() { Id = HttpContext.Session.GetInt32("IdArquivo").Value };
            if (animal != null)
                isTrue = _servicoAnimal.Put(animal.Id, animal);

            return RedirectToAction("Index");
        }
        #endregion

        #region >>> Delete Methods <<<


        public IActionResult Delete(int id)
        {
            var animal = _servicoAnimal.GetById(id);
            return View("Delete", animal);
        }

        public IActionResult DeleteAnimal(int id)
        {
            _servicoAnimal.Delete(id);

            return RedirectToAction("Index");
        }


        #endregion

        #region >>> Details Methods<<<
        public IActionResult Details(int id)
        {
            var model=_servicoAnimal.GetById(id);
            return View("Details", model);
        }

        #endregion


    }
}