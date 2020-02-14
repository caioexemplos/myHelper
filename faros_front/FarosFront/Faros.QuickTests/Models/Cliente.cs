using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.QuickTests.Models
{
    public class Cliente
    {
        private int codigo;

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        private string nome;

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private string telefone;

        public string Telefone
        {
            get { return telefone; }
            set { telefone = value; }
        }
    }
}
