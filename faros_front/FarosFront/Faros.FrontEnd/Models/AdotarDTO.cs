using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Models
{
    public class AdotarDTO
    {

        public string NomeAdotante { get; set; }

        public string EmailAdotante { get; set; }

        public string TelefoneAdotante { get; set; }

        public AnimalDTO Animal { get; set; }

        public string Mensagem { get; set; }

    }
}
