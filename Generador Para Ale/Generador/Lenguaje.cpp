using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace C
{
   public class Lenguaje : Sintaxis
   {
       public Lenguaje(string Nombre = "Prueba.cpp") : base(Nombre)
       {
       }
       void Librerias()
       {
           match("#");
           match("include");
           if (getContenido() == "h") 
           {
               match("h");
           }
           if (getClasificacion() == TokenType.identificador) 
           {
               match(TokenType.identificador);
           }
           if (getContenido() == "#") 
           {
               Librerias();
           }
       }
}
