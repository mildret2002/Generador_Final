using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Error: Exception
    {
        public Error(string message,StreamWriter log) : base(message)
        {
            log.WriteLine($"\nError: {message}");
        }
    } 
} 