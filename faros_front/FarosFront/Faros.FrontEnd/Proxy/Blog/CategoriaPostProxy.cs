using Core.Proxy;
using Faros.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Proxy
{
    public class CategoriaPostProxy : BaseProxy<CategoriaPostDTO>
    {
        public CategoriaPostProxy(string uri) : base(uri)
        {
        }
    }
}
