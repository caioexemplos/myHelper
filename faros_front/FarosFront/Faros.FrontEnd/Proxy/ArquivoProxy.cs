using Core.Proxy;
using Faros.Common;
using Faros.FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Faros.FrontEnd.Proxy
{
    public class ArquivoProxy : BaseProxy<ArquivoDTO>
    {
        public ArquivoProxy(string uri) : base(uri)
        {
        }

        public bool PostListImagens(IEnumerable<ArquivoDTO> lstArquivos)
        {
            try
            {

                var response = _Client.PostAsJsonAsync(string.Format("{0}/{1}", _Uri, "InsertListImagens"), lstArquivos).Result;

                var result = response.Content.ReadAsAsync<bool>().Result;
                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IEnumerable<ArquivoDTO> GetArquivosByTamanho(string tamanho)
        {
            try
            {
                var response = _Client.GetAsync(string.Format("{0}/{1}/?tamanho={2}", _Uri, "GetArquivosByTamanho", tamanho)).Result;

                return response.Content.ReadAsAsync<IEnumerable<ArquivoDTO>>().Result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<ArquivoDTO>GetArquivosBySubCategoria(int subCategoriaId)
        {
            try
            {
                var response = _Client.GetAsync(string.Format("{0}/{1}/?subCategoriaId={2}", _Uri, "GetArquivosBySubCategoria", subCategoriaId)).Result;

                return response.Content.ReadAsAsync<IEnumerable<ArquivoDTO>>().Result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int GetLastFilePosition(int subCategoriaId)
        {
            try
            {
                var response = _Client.GetAsync(string.Format("{0}/{1}/?subCategoriaId={2}", _Uri, "GetLastFilePosition", subCategoriaId)).Result;

                return response.Content.ReadAsAsync<int>().Result;
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
