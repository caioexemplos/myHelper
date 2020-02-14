using Core.Proxy;
using Faros.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Proxy
{
    public class AnimalProxy : BaseProxy<AnimalDTO>
    {
        public AnimalProxy(string uri) : base(uri)
        {
        }

        public IEnumerable<AnimalDTO> GetPaginado(int tamanhoPagina, int pagina = 0)
        {
            try
            {
                var response = _Client.GetAsync(string.Format("{0}/{1}/?pagina={2}&tamanhoPagina={3}", _Uri, "GetPaginado", pagina,tamanhoPagina)).Result;

                return response.Content.ReadAsAsync<IEnumerable<AnimalDTO>>().Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int TotalAnimais()
        {
            try
            {
                var response = _Client.GetAsync(string.Format("{0}/{1}", _Uri, "TotalAnimais")).Result;

                return response.Content.ReadAsAsync<int>().Result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
