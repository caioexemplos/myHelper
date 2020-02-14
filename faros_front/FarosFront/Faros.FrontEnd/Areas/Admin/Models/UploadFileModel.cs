using Faros.Common;
using Faros.FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faros.Admin.Models
{
    public class UploadFileModel<T>
    {
        public IFormFile UploadedFile { get; set; }
        public T UploadEntity { get; set; }
        public IEnumerable<T> lstAuxiliar { get; set; }

    }
}
