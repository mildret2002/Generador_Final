using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                using (Lenguaje l = new Lenguaje())
                {
                    l.generador();
                    /*while (!l.finArchivo())
                    {
                        l.nextToken();  
                    }*/
                }  
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        } 

    }
}