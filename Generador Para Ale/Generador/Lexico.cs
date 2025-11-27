using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
/*
    INTEGRANTES DEL EQUIPO
*/
namespace Generador
{
    public class Lexico : Token, IDisposable
    {
        protected StreamReader archivo;
        protected StreamWriter log; //bitacora
        protected StreamWriter clase; //codigo ensamblador
        const int E = -1;
        const int F = -2;
        protected int lineas = 1;
        int[,] TranD =
        {
            {0,1,3,2,5,2,2,2,2,2,2},
            {F,1,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,4,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,6,7,8,9,10,F},
            {F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F},
        };

        public Lexico(string nombre = "Gramatica.txt")
        {
            log = new StreamWriter(Path.GetFileNameWithoutExtension(nombre) + ".log");
            log.AutoFlush = true;
            if (!File.Exists(nombre))
            {
                throw new Error($"Archivo {nombre} no existe.", log);
            }
            archivo = new StreamReader(nombre);
            clase = new StreamWriter("Lenguaje.cpp");
            clase.AutoFlush = true;
            cabecera(nombre);
        }
        public void Dispose()
        {
            archivo.Close();
            log.Close();
            clase.Close();
        }
        private void cabecera(string nombre)
        {
            log.WriteLine("Autor: Omar Alejandro Granados");
            log.WriteLine("Fecha: " + DateTime.Now.ToString());
            log.WriteLine("Archivo: " + nombre);
            log.WriteLine("--------------------------------------------------");
        }

        int columna(char c)
        {
            //Ws	L	-	>	\	|	;	?	(	)	la
            if (char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if (char.IsLetter(c))
            {
                return 1;
            }
            else if (c == '-')
            {
                return 2;
            }
            else if (c == '>')
            {
                return 3;
            }
            else if (c == '\\')
            {
                return 4;
            }
            else if (c == '|')
            {
                return 5;
            }
            else if (c == ';')
            {
                return 6;
            }
            else if (c == '?')
            {
                return 7;
            }
            else if (c == '(')
            {
                return 8;
            }
            else if (c == ')')
            {
                return 9;
            }
            else
            {
                return 10;
            }
        }
        private void clasifica(int estado)
        {
            switch (estado)
            {
                case 1:
                case 2:
                case 3:
                case 5:
                    setClasificacion(TokenType.Simbolo);
                    break;
                case 4:
                    setClasificacion(TokenType.Produce);
                    break;
                case 6:
                    setClasificacion(TokenType.Or);
                    break;
                case 7:
                    setClasificacion(TokenType.FinProduccion);
                    break;
                case 8:
                    setClasificacion(TokenType.Epsilon);
                    break;
                case 9:
                    setClasificacion(TokenType.parentesisIzq);
                    break;
                case 10:
                    setClasificacion(TokenType.parentesisDer);
                    break;
            }
        }

        public void nextToken()
        {
            char caracter;
            string buffer = "";
            int estado = 0;
            while (estado >= 0)
            {
                caracter = (char)archivo.Peek();
                estado = TranD[estado, columna(caracter)];
                clasifica(estado);
                if (estado >= 0)
                {
                    if (estado > 0)
                    {
                        buffer += caracter;
                    }
                    archivo.Read();
                    lineas++;
                }
            }
            if (estado == E)
            {
                throw new Error("Léxico en la linea " + lineas, log);
            }
            setContenido(buffer);
            if (!finArchivo())
            {
                log.WriteLine(getContenido() + " = " + getClasificacion());
            }
        }

        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }

}