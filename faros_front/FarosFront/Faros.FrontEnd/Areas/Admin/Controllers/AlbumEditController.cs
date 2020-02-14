using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Faros.Admin.Models;
using Faros.Common;
using Faros.Common.Constants;
using Faros.Common.Enums;
using Faros.Common.Helpers;
using Faros.FrontEnd.Models;
using Faros.FrontEnd.Proxy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace Faros.FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlbumEditController : Controller
    {
        public static List<Arquivo> lstArquivos;

        private ArquivoProxy _servicoImagem;
        private string _imagePath;
        IConfiguration _configurationOutPut;
        IHostingEnvironment _appEnviroment;
      

        public AlbumEditController(IConfiguration configurationInput,
                                      IHostingEnvironment environment)

        {
            _configurationOutPut = configurationInput;
            _appEnviroment = environment;
             _imagePath = _appEnviroment.ContentRootPath + AssemblyConstants.GetPathImageByBuild(FileConstants.DEV, AssemblyConstants.PATH_IMAGE_ALBUM);
            _servicoImagem = new ArquivoProxy(_configurationOutPut.GetValue<string>("UriHost"));
        }

        public IActionResult Index()
        {
            IEnumerable<ArquivoDTO> listaRetorno = _servicoImagem.GetArquivosBySubCategoria((int)SubCategoriaArquivoEnum.FOTO_ALBUM);


            if (listaRetorno == null)
                listaRetorno = new List<ArquivoDTO>();

            return View("Index", new UploadFileModel<ArquivoDTO>() { lstAuxiliar = listaRetorno });

        }

        [HttpPost("UploadPhoto")]
        public IActionResult UploadPhoto(IFormFile file)
        {

            if(file!=null)
            {
               // int posicao = _servicoImagem.GetLastFilePosition((int)SubCategoriaArquivoEnum.FOTO_ALBUM) + 1;
                MemoryStream ms = new MemoryStream();
                file.OpenReadStream().CopyTo(ms);

                HttpContext.Session.SetObject("TransientAlbumFoto", ms.ToArray());
            
                //Arquivo arquivo = new Arquivo()
                //{
                //    ContentType = file.UploadedFile.ContentType,
                //    Posicao = posicao,
                //    Nome = "album_foto_"+ posicao,
                //    CategoriaArquivoId = (int)CategoriaArquivoEnum.FOTO,
                //    SubCategoriaArquivoId = (int)SubCategoriaArquivoEnum.FOTO_ALBUM,
                //    NomeSalvar = file.UploadedFile.FileName,
                //    Tamanho = "500x500",
                //    ArquivoArray = ms.ToArray()
                //};

                //ArquivoHelper.SalvarArquivo(arquivo, _imagePath);

                //arquivo.Path = ArquivoHelper.GetPathExetension(AssemblyConstants.GetPathImageByBuild(FileConstants.PROD, AssemblyConstants.PATH_IMAGE_ALBUM), arquivo.NomeSalvar, arquivo.Nome);


                ////TODO:Fazer um tratamento para arquivo único
                //var mylist = new List<Arquivo>();
                //mylist.Add(arquivo);
                //FormatAndSendImages(mylist);

            }


            return RedirectToAction("Index", "AlbumEdit");
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
        }

        
        [HttpGet]
        public FileStreamResult VerImagemNovo(/*string nome*/)
        {
            //var imgPath = _imagePath + nome + ".jpg";
            //FileStream fileStream = new FileStream(imgPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var myByteArray = HttpContext.Session.GetObject<byte[]>("TransientAlbumFoto");

            if (myByteArray == null)
                myByteArray = new byte[0];

            return new FileStreamResult(new MemoryStream(myByteArray), "image/jpeg");

        }


    }
}