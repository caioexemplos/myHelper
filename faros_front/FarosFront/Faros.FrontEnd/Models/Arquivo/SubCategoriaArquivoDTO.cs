using Core.Container.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Models
{
    public class SubCategoriaArquivoDTO : EntidadeBaseDTO
    {
        public CategoriaArquivoDTO CategoriaArquivo { get; set; }
    }
}
