using Core.Container.DTO;
using Faros.FrontEnd.Models.FixedDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Models
{
    public class AnimalDTO: EntidadeBaseDTO
    {
        public int RESA { get; set; }
        public EspecieDTO Especie { get; set; }
        public IdadeDTO Idade { get; set; }
        public SexoDTO Sexo { get; set; }
        public ArquivoDTO FotoArquivo { get; set; }
        public DateTime? Castracao { get; set; }


        public int TotalPagAux { get; set; }

    }
}
