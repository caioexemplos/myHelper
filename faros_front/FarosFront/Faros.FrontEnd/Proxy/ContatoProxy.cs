using Core.Proxy;
using Faros.FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Proxy
{
    public class ContatoProxy : BaseProxy<ContatoDTO>
    {
        public ContatoProxy(string uri) : base(uri)
        {


        }

        public AdotarDTO PostContatoAdotar(AdotarDTO obj)
        {
            try
            {

                var response = _Client.PostAsJsonAsync(string.Format("{0}/{1}", _Uri, "PostContatoAdotar"), obj).Result;

                var entity = response.Content.ReadAsAsync<AdotarDTO>().Result;
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
