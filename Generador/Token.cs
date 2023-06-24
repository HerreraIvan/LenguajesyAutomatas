
using System;
using System.Collections.Generic;
using System.Text;

namespace Generador
{
    public class Token: Error
    {
        public enum clasificaciones
        {
            snt, st, flechita, finProduccion, cerraduraEpsilon, 
            parentesisIzquierdo, parentesisDerecho, or,

            corcheteIzquierdo, corcheteDerecho

            /*
                snt -> L+
                st -> L+ | clasificaciones.tipo | caracter
                flechita -> '->'
                cerraduraEpsilon -> \?
                parentesisIzquierdo -> \( 
                parentesisDerecho -> \) 
                or -> \|
                comentarios -> \//
                multiComentario -> \
                corcheteIzquierdo -> \[
                corcheteDerecho -> \]
            */
        }
        private string contenido;
        private clasificaciones clasificacion;
        protected bool esClasificacion(string clasificacion)
        {
            switch(clasificacion)
            {
                case "identificador":
                case "numero":
                case "asignacion": 
                case "inicializacion": 
                case "finSentencia": 
                case "operadorLogico": 
                case "operadorRelacional": 
                case "operadorTermino":
                case "operadorFactor": 
                case "incrementoTermino": 
                case "incrementoFactor": 
                case "cadena": 
                case "operadorTernario": 
                case "caracter": 
                case "tipoDato": 
                case "zona": 
                case "condicion": 
                case "ciclo": 
                case "inicioBloque": 
                case "finBloque": 
                case "flujoEntrada": 
                case "flujoSalida":  
                return true;
            }
            return false; 
        } 
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }

        public void setClasificacion(clasificaciones clasificacion)
        {
            this.clasificacion = clasificacion;
        }

        public string getContenido()
        {
            return contenido;
        }

        public clasificaciones getClasificacion()
        {
            return clasificacion;
        }
    }
}
