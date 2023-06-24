using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Generador
{
    public class Lexico: Token, IDisposable
    {
        private StreamReader archivo;
        protected StreamWriter bitacora;
        protected StreamWriter lenguaje;
        protected StreamWriter program;
        string nombreArchivo;
        protected int linea, caracter;
        const int F = -1;
        const int E = -2;
        int[,] trand   =  {  //WS, L, -, >, \, ;, ?, (, ), |,LA, /, *,EF,#10,[, ]
                            { 0, 1, 2,10, 4,10,10,10,10,10,10,11, 0, F, 0,10,10},//0
                            { F, 1, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//1
                            { F, F, F, 3, F, F, F, F, F, F, F, F, F, F, F, F, F},//2
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//3
                            { F, F, F, F, F, 5, 6, 7, 8, 9, F, F, F, F, F,15,16},//4
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//5
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//6
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//7
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//8
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//9
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//10
                            { F, F, F, F, F, F, F, F, F, F, F,12,13, F, F, F, F},//11
                            {12,12,12,12,12,12,12,12,12,12,12,12,12, 0, 0,12,12},//12
                            {13,13,13,13,13,13,13,13,13,13,13,13,14, E,13,13,13},//13
                            {13,13,13,13,13,13,13,13,13,13,13, 0,14, E,13,13,13},//14
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//15
                            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F},//16
                        };

        public Lexico()
        {
            linea = caracter = 1;
            nombreArchivo = "c.gram";

            Console.WriteLine("Compilando c.gram");
            Console.WriteLine("Iniciando analisis lexico.");

            if (File.Exists("C:\\Archivos\\c.gram"))
            {
                archivo = new StreamReader("C:\\Archivos\\c.gram");
                bitacora = new StreamWriter("C:\\Archivos\\prueba.log");
                lenguaje = new StreamWriter("C:\\Archivos\\lenguaje.cs");
                program = new StreamWriter("C:\\Archivos\\program.cs");
                bitacora.AutoFlush = true;     
                lenguaje.AutoFlush = true;  
                program.AutoFlush = true;        

                DateTime fechaActual = DateTime.Now;

                bitacora.WriteLine("Archivo: c.gram");
                bitacora.WriteLine("Directorio: C:\\Archivos");
                bitacora.WriteLine("Fecha: " + fechaActual);
            }
            else
            {
                throw new Exception("El archivo prueba.gram no existe.");
            }

        }
        public Lexico(string nombre)
        {
            linea = caracter = 1;

            nombreArchivo = Path.GetFileName(nombre);
            string nombreDir = Path.GetDirectoryName(nombre);

            Console.WriteLine("Compilando " + nombreArchivo);
            Console.WriteLine("Iniciando analisis lexico.");

            if (Path.GetExtension(nombre) != ".gram")
            {
                throw new Exception(String.Format("El archivo {0} no es un archivo gram.", nombreArchivo));
            }

            if (File.Exists(nombre))
            {                
                archivo = new StreamReader(nombre);

                string log = Path.ChangeExtension(nombre, "log");
                bitacora = new StreamWriter(log);
                lenguaje = new StreamWriter("C:\\Archivos\\lenguaje.cs");
                program = new StreamWriter("C:\\Archivos\\program.cs");
                bitacora.AutoFlush = true;
                lenguaje.AutoFlush = true;
                program.AutoFlush = true;
                DateTime fechaActual = DateTime.Now;

                bitacora.WriteLine("Archivo: " + nombreArchivo);
                bitacora.WriteLine("Directorio: " + nombreDir);
                bitacora.WriteLine("Fecha: " + fechaActual);                             
            }
            else
            {
                string mensaje = String.Format("El archivo {0} no existe.", nombreArchivo);
                throw new Exception(mensaje);
            }

        }
        //~Lexico()
        public void Dispose()
        {
            Console.WriteLine("Finaliza compilacion de " + nombreArchivo);
            CerrarArchivos();
        }

        private void CerrarArchivos()
        {
            archivo.Close();
            bitacora.Close();
            lenguaje.Close();
            program.Close();
        }

        public void NextToken()
        {
            char transicion;
            string palabra = "";
            int estado = 0;

            while (estado >= 0)
            {
                transicion = (char)archivo.Peek();

                estado = trand[estado, columna(transicion)];
                clasificar(estado);

                if (estado >= 0)
                {
                    archivo.Read();
                    caracter++;

                    if (transicion == 10)
                    {
                        linea++;
                        caracter = 1;
                    }

                    if (estado > 0)
                    {
                        palabra += transicion;
                    }
                    else
                    {
                        palabra = "";
                    }
                }
            }

            setContenido(palabra);
            
            if (estado == -2)
            {
                throw new Error(bitacora, "Error lexico: Se esperaba un cierre de comentario (*/). Linea: " + linea + ", caracter:" + caracter);  
            }
            else if (getClasificacion() == clasificaciones.snt)
            {
                if (!char.IsUpper(getContenido()[0]))
                {
                    setClasificacion(clasificaciones.st);
                }      
            }

            if (getContenido() != "")
            {
                bitacora.WriteLine("Token = " + getContenido());
                bitacora.WriteLine("Clasificacion = " + getClasificacion());
            }            
        }
        

        private void clasificar(int estado)
        {
            switch (estado)
            {
                case 1:
                    setClasificacion(clasificaciones.snt);
                    break;
                case 2:
                case 4:
                case 10:
                    setClasificacion(clasificaciones.st);
                    break;
                case 3:
                    setClasificacion(clasificaciones.flechita);
                    break;
                case 5:
                    setClasificacion(clasificaciones.finProduccion);
                    break;
                case 6:
                    setClasificacion(clasificaciones.cerraduraEpsilon);
                    break;
                case 7:
                    setClasificacion(clasificaciones.parentesisIzquierdo);
                    break;
                case 8:
                    setClasificacion(clasificaciones.parentesisDerecho);
                    break;
                case 9:
                    setClasificacion(clasificaciones.or);
                    break;
                case 12:
                case 13:
                case 14:
                    break;
                case 15:
                    setClasificacion(clasificaciones.corcheteIzquierdo);
                    break;
                case 16:
                    setClasificacion(clasificaciones.corcheteDerecho);
                    break;
            }
        }

        private int columna(char t)
        {
            //WS, L, -, >, \, ;, ?, (, ), |, LA
            if(FinDeArchivo())
            {
              return 13;
            }
            else if(t == 10)
            {
                return 14;
            }
           else if (char.IsWhiteSpace(t))
            {
                return 0;
            }
            else if (char.IsLetter(t))
            {
                return 1;
            }
            else if (t == '-')
            {
                return 2;
            }
            else if (t == '>')
            {
                return 3;
            }
            else if (t == '\\')
            {
                return 4;
            }
            else if (t == ';')
            {
                return 5;
            }
            else if (t == '?')
            {
                return 6;
            }
            else if (t == '(')
            {
                return 7;
            }
            else if (t == ')')
            {
                return 8;
            }
            else if (t == '|')
            {
                return 9;
            }
            else if(t=='/')
            {
                return 11;
            }
            else if(t == '*')
            { 
                return 12;
            }
            else if(t == '[')
            {
                return 15;
            }
            else if(t==']')
            {
                return 16;
            }
            else
            {
                return 10;
            }            
        }

        public bool FinDeArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}