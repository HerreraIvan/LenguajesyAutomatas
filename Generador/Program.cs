using System;

namespace Generador
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje l = new Lenguaje("C:\\Archivos\\c.gram"))                
                {
                    /*
                    while (!l.FinDeArchivo())
                    {
                        l.NextToken();
                    }
                    */
                    l.gramatica();
                   
                }
            }
            catch (Error e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}