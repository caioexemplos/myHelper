using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Container.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Faros.FrontEnd.Models
{
    public class ArquivoDTO : EntidadeBaseDTO
    {
        public byte[] ArquivoArray { get; set; }
        public string ArquivoString { get; set; }
        public int Posicao { get; set; }
        public string ContentType { get; set; }
        public SubCategoriaArquivoDTO SubCategoriaArquivo { get; set; }
        public string NomeSalvar { get; set; }
        public string Tamanho { get; set; }
        public string Path { get; set; }
    }
}
