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
    public class AlbumController : Controller
    {
        #region >>> Variáveis <<<
        private AnimalProxy _serviceAnimal;
        private ContatoProxy _serviceContato;
        IConfiguration _configurationOutPut;
        private const int TAMANHO_PAGINA=6;
        #endregion

        public AlbumController(IConfiguration configurationInput)
        {
            _configurationOutPut = configurationInput;
            _serviceAnimal = new AnimalProxy(_configurationOutPut.GetValue<string>("UriHost"));
            _serviceContato = new ContatoProxy(_configurationOutPut.GetValue<string>("UriHost"));
            
        }
        public IActionResult AlbumHome(int pagina=0)
        {
            var animaisPaginados = _serviceAnimal.GetPaginado(TAMANHO_PAGINA,pagina);
            ViewBag.TamanhoTotal = (int)Math.Ceiling((double)(_serviceAnimal.TotalAnimais()/ TAMANHO_PAGINA)+1);
            ViewBag.PaginaAtual = pagina+1;

            return View(animaisPaginados);
        }

        public IActionResult ContatoAdotar(int animalId)
        {
            var animal = _serviceAnimal.GetById(animalId);
            var adotaAnimal = new AdotarDTO()
            {
                Animal = new AnimalDTO()
                {
                    Id = animal.Id,
                    RESA = animal.RESA,
                    Nome = animal.Nome,
                    FotoArquivo = new ArquivoDTO()
                    {
                        Id = animal.FotoArquivo.Id,
                        Path = animal.FotoArquivo.Path
                    }
                }
            };

            return View("ContatoAdotar", adotaAnimal);


        }

        [HttpPost]
        public IActionResult ContatoAdotar(AdotarDTO adotar)
        {
            var retorno = _serviceContato.PostContatoAdotar(adotar);
            return RedirectToAction("AlbumHome");
        }

       
    }
}