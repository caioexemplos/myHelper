using Core.Proxy;
using Faros.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Proxy
{
    public class UsuarioProxy : BaseProxy<UsuarioDTO>
    {
        public UsuarioProxy(string uri) : base(uri)
        {
        }
    }
}
