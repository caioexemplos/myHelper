using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Models
{
    public class PostDTO
    {
        public int Id { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public CategoriaPostDTO CategoriaBlogPost { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Resumo { get; set; }
        public ArquivoDTO FotoPost { get; set; }
        public DateTime Data { get; set; }



    }
}
