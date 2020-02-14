using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Core.Common;
using Faros.Common;
using Faros.Common.Constants;
using Faros.Common.Enums;
using Faros.Common.Helpers;
using Faros.FrontEnd.Models;
using Faros.FrontEnd.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Faros.FrontEnd.Areas
{
    [Area("Admin")]
    public class CarrousselEditController : Controller
    {

        public static List<Arquivo> lstArquivos;
        private ArquivoProxy _servicoImagem;
        private string _imagePath;
        IConfiguration _configurationOutPut;
        IHostingEnvironment _appEnviroment;

        public CarrousselEditController(IConfiguration configurationInput,
                                        IHostingEnvironment environment)

        {
            _configurationOutPut = configurationInput;
            _appEnviroment = environment;
            _imagePath = _appEnviroment.ContentRootPath + AssemblyConstants.GetPathImageByBuild(FileConstants.DEV, AssemblyConstants.PATH_IMAGE_INDEX_CARROUSSEL);
            _servicoImagem = new ArquivoProxy(_configurationOutPut.GetValue<string>("UriHost"));
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]//Colocar o post pois será passado a lista no retorno.
        public IActionResult Index(IEnumerable<Arquivo> arquivos)
        {
            return View(arquivos);
        }


        //Recebe uma lista de arquivos vindas do front usando sempre a entidade IFormFile. 

        [HttpPost]
        public IActionResult UploadImagem(IList<IFormFile> arquivos)
        {

            lstArquivos = new List<Arquivo>();
            if (arquivos.Count() > 0)
            {

                int i = 0;
                MemoryStream ms;


                arquivos.ToList().ForEach(x =>
                {

                    //cria um memory stream para gravar os arquivos.
                    ms = new MemoryStream();
                    //lê os bytes do arquivo e o copia dentro da memoryStream.
                    x.OpenReadStream().CopyTo(ms);

                    lstArquivos.Add(

                        new Arquivo()
                        {
                            ContentType = x.ContentType,
                            Posicao = i++,
                            Nome = "panel_" + i.ToString(),
                            CategoriaArquivoId=(int)CategoriaArquivoEnum.FOTO,
                            SubCategoriaArquivoId = (int)SubCategoriaArquivoEnum.FOTO_CARROUSSEL,
                            NomeSalvar = x.FileName,
                            Tamanho = "1500x500",
                            ArquivoArray = ms.ToArray()
                        });

                });


                ArquivoHelper.SalvarLstArquivos(lstArquivos, _imagePath);

                //Seta o path do arquivo de acordo com o caminho salvo.
                SetFilesPath(lstArquivos, AssemblyConstants.GetPathImageByBuild(FileConstants.PROD, AssemblyConstants.PATH_IMAGE_INDEX_CARROUSSEL));

                //Formata os arquivos para uma entidade de transporte
                FormatAndSendImages(lstArquivos);

                //retorna uma lista com as imagens.
                var listReturn = _servicoImagem.GetArquivosBySubCategoria((int)SubCategoriaArquivoEnum.FOTO_CARROUSSEL);

                return View("Index", lstArquivos);

            }

            return View("Index");
        }

        private void FormatAndSendImages(IEnumerable<Arquivo>lstArquivos)
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
                        Id = (int)CategoriaArquivoEnum.FOTO
                    }
                },
                Path = x.Path,
                Posicao = x.Posicao,
                Tamanho = x.Tamanho
            });

            _servicoImagem.PostListImagens(listToTransporter);
        }

        private void SetFilesPath(List<Arquivo> lstArquivos, string path)
        {
            lstArquivos
                    .ForEach(x =>
                    x.Path = ArquivoHelper.GetPathExetension(path, x.NomeSalvar, x.Nome));
        }

        [HttpGet]
        public FileStreamResult VerImagemNovo(string nome)
        {
            var imgPath = _imagePath + nome + ".jpg";
            FileStream fileStream = new FileStream(imgPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            return new FileStreamResult(fileStream, "image/jpeg");

        }

    }
}