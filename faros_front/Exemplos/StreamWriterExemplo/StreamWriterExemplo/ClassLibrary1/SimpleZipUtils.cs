using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace ClassLibrary1
{
    public static class SimpleZipUtils
    {

        private const string _fileExtension = "MyZipFile.zip";
        public static byte[] Compressor(EntidadeZip myEntidadeZip)
        {
            using (MemoryStream memory = new MemoryStream())
            {

                using (ZipArchive zipArchive = new ZipArchive(memory, ZipArchiveMode.Create, true))
                {

                    ZipArchiveEntry zipItem = zipArchive.CreateEntry(myEntidadeZip.NomeSalvar);

                    using (MemoryStream ms = new MemoryStream(myEntidadeZip.Arquivo))
                    {
                        using (var myStream = zipItem.Open())
                        {
                            ms.CopyTo(myStream);
                        }
                    }


                    //ZipArchive zipArquive =new ZipArchive()

                }


                return memory.ToArray();
            }
        }


    }
}
