using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Models
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public ArquivoDTO FotoUsuario { get; set; }


    }
}
