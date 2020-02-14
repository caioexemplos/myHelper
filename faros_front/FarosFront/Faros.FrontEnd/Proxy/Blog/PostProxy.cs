using Core.Proxy;
using Faros.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Proxy
{
    public class PostProxy : BaseProxy<PostDTO>
    {
        public PostProxy(string uri) : base(uri)
        {
        }
    }
}
