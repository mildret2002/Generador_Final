using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Token
    {
        public enum TokenType
        {
            Simbolo,
            Produce,
            Or,
            FinProduccion,
            Epsilon,
            parentesisDer,
            parentesisIzq
        }
        private string contenido;
        private TokenType clasificacion;
        public Token()
        {
            this.contenido = "";
            this.clasificacion = TokenType.Simbolo;
        }
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public void setClasificacion(TokenType clasificacion)
        {
            this.clasificacion = clasificacion;
        }
        public string getContenido()
        {
            return this.contenido;
        }
        public TokenType getClasificacion()
        {
            return this.clasificacion;
        }
    }
}