using ClassLibrary1;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            var entidade = new EntidadeZip()
            {
                Nome = @"C:\Users\caio.leite\Desktop\Arquivos_Opcoes\1_iocs_base.csv",
                NomeSalvar = "1_iocs_base.csv",
                Arquivo = File.ReadAllBytes(@"C:\Users\caio.leite\Desktop\Arquivos_Opcoes\1_iocs_base.csv")
            };

            var myReturnArrayByte = SimpleZipUtils.Compressor(entidade);
            //foreach (var item in SimpleZipUtils.Compressor(entidade))
            //{
            //    Console.WriteLine(item);
            //}

            string myPath = @"C:\Users\caio.leite\Desktop\myNewArquive.zip";
            using (FileStream fs = new FileStream(myPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                    fs.Write(myReturnArrayByte, 0, myReturnArrayByte.Length);
            }

                Console.ReadLine();

        }

    }
}
