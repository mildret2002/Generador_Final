using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
    Requerimiento 1: la primera produccion debe ser publica. (30%)
    Requerimiento 3: Implmentar el "Or" (70%)
*/
namespace Generador
{
    public class Lenguaje : Sintaxis
    {
        List<string> listaSNT;
        List<string> listaST;
        public Lenguaje(string Nombre = "Gramatica.txt") : base(Nombre)
        {
            listaSNT = new List<string>();
            listaST = new List<string>();
        }
        public void generador()
        {
            cabecera();
            producciones();
            tokens();
            main();
            clase.WriteLine("}");
        }
        void cabecera()
        {
            match("Lenguaje");
            match(":");
            string nombre = getContenido();
            match(TokenType.Simbolo);
            match(";");
            clase.WriteLine("using System;");
            clase.WriteLine("using System.Collections.Generic;");
            clase.WriteLine("using System.Linq;");
            clase.WriteLine("using System.Runtime.CompilerServices;");
            clase.WriteLine("using System.Text.Json;");
            clase.WriteLine("using System.Text.RegularExpressions;");
            clase.WriteLine("using System.Threading.Tasks;");
            clase.WriteLine();
            clase.WriteLine("namespace " + nombre);
            clase.WriteLine("{");
            clase.WriteLine("   public class Lenguaje : Sintaxis");
            clase.WriteLine("   {");
            clase.WriteLine("       public Lenguaje(string Nombre = \"Prueba.cpp\") : base(Nombre)");
            clase.WriteLine("       {");
            clase.WriteLine("       }");
        }
        void producciones()
        {
            match("Producciones");
            match(":");
            listaProducciones();
            match(";");
        }
        void listaProducciones()
        {
            listaSNT.Add(getContenido());
            match(TokenType.Simbolo);
            if (getContenido() == ",")
            {
                match(",");
                listaProducciones();
            }
        }
        void tokens()
        {
            match("Tokens");
            match(":");
            listaTokens();
            match(";");
        }
        void listaTokens()
        {
            listaST.Add(getContenido());
            match(TokenType.Simbolo);
            if (getContenido() == ",")
            {
                match(",");
                listaTokens();
            }
        }
        void main()
        {
            match("{");
            produccionesGramaticales();
            match("}");
        }
        void produccionesGramaticales()
        {
            produccion();
            if (getContenido() != "}")
            {
                produccionesGramaticales();
            }
        }
        void produccion()
        {
            string nombreProduccion = getContenido();
            clase.WriteLine("       void " + nombreProduccion + "()");
            clase.WriteLine("       {");
            match(TokenType.Simbolo);
            match(TokenType.Produce);
            string primerSimbolo = getContenido();
            listaSimbolos(nombreProduccion, primerSimbolo);
            match(TokenType.FinProduccion);
            clase.WriteLine("       }");
        }
        bool hayUnEpsilon(string nombreDeLaProduccion, string primerSimbolo, string simboloActual)
        {
            if (getClasificacion() == TokenType.Epsilon)
            {
                if (listaST.Exists(x => x == simboloActual))
                {
                    clase.WriteLine("           if (getClasificacion() == TokenType." + simboloActual + ") ");
                    clase.WriteLine("           {");
                }
                else if (listaSNT.Exists(x => x == simboloActual))
                {
                    if (simboloActual == nombreDeLaProduccion)
                    {
                        if (listaST.Exists(x => x == primerSimbolo))
                        {
                            clase.WriteLine("           if (getClasificacion() == TokenType." + primerSimbolo + ") ");
                            clase.WriteLine("           {");
                        }
                        else
                        {
                            clase.WriteLine("           if (getContenido() == \"" + primerSimbolo + "\") ");
                            clase.WriteLine("           {");
                        }
                    }
                    else
                    {
                        throw new Error("de Sintaxis, el primer simbolo es SNT \"" + simboloActual + "\" en la linea " + lineas, log);
                    }
                }
                else
                {
                    clase.WriteLine("           if (getContenido() == \"" + simboloActual + "\") ");
                    clase.WriteLine("           {");
                }
                return true;
            }
            return false;
        }
        void listaSimbolos(string nombreProduccion, string primerSimbolo)
        {
            bool epsilon = false;
            if (listaSNT.Exists(x => x == getContenido()))
            {
                string simboloActual = getContenido();
                match(TokenType.Simbolo);
                epsilon = hayUnEpsilon(nombreProduccion, primerSimbolo, simboloActual);
                if (epsilon)
                {
                    clase.WriteLine("               " + simboloActual + "();");
                    match(TokenType.Epsilon);
                    clase.WriteLine("           }");
                }
                else
                {
                    clase.WriteLine("           " + simboloActual + "();");
                }
            }
            else if (listaST.Exists(x => x == getContenido()))
            {
                string simboloActual = getContenido();
                Console.WriteLine(simboloActual);
                match(TokenType.Simbolo);
                epsilon = hayUnEpsilon(nombreProduccion, primerSimbolo, simboloActual);
                if (epsilon)
                {
                    clase.WriteLine("               match(TokenType." + simboloActual + ");");
                    match(TokenType.Epsilon);
                    clase.WriteLine("           }");
                }
                else
                {
                    clase.WriteLine("           match(TokenType." + simboloActual + ");");
                }
            }
            else if (getClasificacion() == TokenType.Simbolo)
            {
                string simboloActual = getContenido();
                match(TokenType.Simbolo);
                epsilon = hayUnEpsilon(nombreProduccion, primerSimbolo, simboloActual);
                if (epsilon)
                {
                    clase.WriteLine("               match(\"" + simboloActual + "\");");
                    match(TokenType.Epsilon);
                    clase.WriteLine("           }");
                }
                else
                {
                    clase.WriteLine("           match(\"" + simboloActual + "\");");
                }
            }
            else
            {
                match(TokenType.parentesisIzq);
                listaSimbolos(nombreProduccion, primerSimbolo);
                match(TokenType.parentesisDer);
                /*if (getClasificacion() == TokenType.Epsilon)
                {
                    match(TokenType.Epsilon);
                }*/
            }
            if (getClasificacion() != TokenType.FinProduccion &&
                getClasificacion() != TokenType.parentesisDer)
            {
                listaSimbolos(nombreProduccion, primerSimbolo);
            }
        }
    }
}

