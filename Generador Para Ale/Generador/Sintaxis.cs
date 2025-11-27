using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Sintaxis : Lexico
    {
        public Sintaxis (string Nombre = "Prueba.cpp"):base(Nombre)
        {
            nextToken();
        }

        public void match(string esperado)
        {
            if(esperado == getContenido())
            {
                nextToken();
            }
            else
            {
                throw new Error("de Sintaxis, se espera un "+esperado+" (" + getContenido() + ") en la linea "+lineas,log);
            }
        }
        public void match(TokenType esperado)
        {
            if(esperado == getClasificacion())
            {
                nextToken();
            }
            else
            {
                throw new Error("de Sintaxis, se espera un "+esperado + " en la linea "+lineas,log);
            }
        }
    }
}