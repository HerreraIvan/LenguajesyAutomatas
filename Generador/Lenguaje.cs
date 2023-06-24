using System;
using System.Collections.Generic;
using System.Text;

// Requerimiento 1: Agregar Comentarios De Linea Y Multilinea A Nivel Lexico
// Requerimiento 2: El Proyecto Se Debe De Llamar Igual Que El Lenguaje
// Requerimiento 3: Indentar El Codigo Generado. Tip: Escribe(int numeroTabs, string instruccion)
// Requerimiento 4: En La Cerradura Epsilon, Considerar getClasificacion y getContenido
// Requerimiento 5: Implementar El Operador OR 

/*
    Lenguaje -> lenguaje:identificador; { ListaProducciones }
    ListaProducciones -> snt flechita ListaSimbolos finProduccion ListaProducciones ?
    ListaSimbolos -> snt | st ListaSimbolos ?
*/

namespace Generador
{
    public class Lenguaje : Sintaxis
    {
        protected int numeroTab;
        public Lenguaje()
        {
            numeroTab = 0;
            Console.WriteLine("Iniciando analisis gramatical.");
        }

        public Lenguaje(string nombre) : base(nombre)
        {
            numeroTab = 0;
            Console.WriteLine("Iniciando analisis gramatical.");
        }

        // Lenguaje -> lenguaje:identificador; { ListaProducciones }
        public void gramatica()
        {
            match("lenguaje");
            match(":");
            string NnameSpace = getContenido();
            Cabecera2(NnameSpace);
            if (getClasificacion() == clasificaciones.snt)
            {
                match(clasificaciones.snt);
            }
            else
            {
                match(clasificaciones.st);
            }

            match(";");

            Cabecera(NnameSpace);
            
            match("{");
            ListaProducciones();
            match("}");
            Escribe("}");
            Escribe("}");
        }
       
        // ListaProducciones -> snt flechita ListaSimbolos finProduccion ListaProducciones ?
        private void ListaProducciones()
        {
            Escribe("public void " + getContenido() + "()");
            match(clasificaciones.snt);
            match(clasificaciones.flechita);

            Escribe("{");
            ListaSimbolos();
            Escribe("}");

            match(clasificaciones.finProduccion);

            if (getClasificacion() == clasificaciones.snt)
            {
                ListaProducciones();
            }
        }

        // ListaSimbolos -> snt | st ListaSimbolos ?
        private void ListaSimbolos()
        {
            if (getClasificacion() == clasificaciones.snt)
            {
                Escribe("" + getContenido() + "();");
                match(clasificaciones.snt);
            }
            else if (getClasificacion() == clasificaciones.st)
            {
                if (esClasificacion(getContenido()))
                {
                    Escribe("" + "match(clasificaciones." + getContenido() + ");");
                }
                else
                {
                    Escribe("" + "match(\"" + getContenido() + "\");");
                }
                match(clasificaciones.st);
            }
            else if (getClasificacion() == clasificaciones.parentesisIzquierdo)
            {
                match(clasificaciones.parentesisIzquierdo);
                RepetidoIf();
                match(clasificaciones.st);

                if (getClasificacion() == clasificaciones.snt || getClasificacion() == clasificaciones.st)
                {
                    ListaSimbolos();
                  
                }
                  match(clasificaciones.parentesisDerecho);
                  Escribe("" + "}");
                    match(clasificaciones.cerraduraEpsilon);

            }

            else if (getClasificacion() == clasificaciones.corcheteIzquierdo)
            {
                match(clasificaciones.corcheteIzquierdo);
                RepetidoIf();
                match(clasificaciones.st);

                if (getClasificacion() == clasificaciones.snt || getClasificacion() == clasificaciones.st || getClasificacion() == clasificaciones.or)
                {
                    ListaORs();
                   
                }
                 match(clasificaciones.corcheteDerecho);
                
            }
               if (getClasificacion() == clasificaciones.snt || getClasificacion() == clasificaciones.st || getClasificacion() == clasificaciones.parentesisIzquierdo || getClasificacion() == clasificaciones.corcheteIzquierdo)
                {
                    ListaSimbolos();
                }


        }

     private void ListaORs()
        {
           
            if (getClasificacion() == clasificaciones.or)
            {
                match(clasificaciones.or);
                Escribe("}");

                string contenido = getContenido();
                match(clasificaciones.st);

                if (getClasificacion() == clasificaciones.or)
                {
                    if (esClasificacion(contenido))
                    {
                        Escribe("else if (getClasificacion() == clasificaciones." + contenido + ")");
                    }
                        
                    else
                    {
                         Escribe("else if (getContenido() == \"" + contenido + "\")");
                    }
                       
                    Escribe("{");
                    if (esClasificacion(contenido))
                    {
                        Escribe("match(clasificaciones." + contenido + ");");
                    }
                        
                    else
                    {
                        Escribe("match(\"" + contenido + "\");");
                    }
                        
                    ListaORs();
                }
                else
                {
                    Escribe("else");
                    Escribe("{");

                    if (esClasificacion(contenido))
                        Escribe("match(clasificaciones." + contenido + ");");
                    else
                        Escribe("match(\"" + contenido + "\");");
                    Escribe("}");
                }
            }
        }
        private void Cabecera(string NnameSpace)
        {
            //quitar lo identado
            Escribe("using System;");
            Escribe("using System.Collections.Generic;");
            Escribe("using System.Text;");
            Escribe("");
            Escribe("namespace " + NnameSpace);
            Escribe("{");
            Escribe("public class Lenguaje: Sintaxis");
            Escribe("{");
            Escribe("public Lenguaje()");
            Escribe("{");
            Escribe("Console.WriteLine(\"Iniciando analisis gramatical.\");");
            Escribe("}");
            Escribe("");
            Escribe("public Lenguaje(string nombre): base(nombre)");
            Escribe("{");
            Escribe("Console.WriteLine(\"Iniciando analisis gramatical.\");");
            Escribe("}");
        }
            public void Cabecera2(string NnameSpace)
        {
            
            Escribe2("using System;");
            Escribe2("using System.Collections.Generic;");
            Escribe2("using System.Text;");
            Escribe2("");
            Escribe2("namespace " + NnameSpace);
            Escribe2("{");
            Escribe2("class Program");
            Escribe2("{");
            Escribe2("public static void Main()");
            Escribe2("{");
            Escribe2("try");
            Escribe2("{");
            Escribe2("");
            Escribe2("using (Lenguaje l = new Lenguaje(\"C:\\\\archivos\\\\c.gram\")");
            Escribe2("{");
            Escribe2("l.gramatica();");
            Escribe2("}");
            Escribe2("}");
            Escribe2("catch(Error e)");
            Escribe2("{");
            Escribe2("Console.WriteLine(e.Message);");
            Escribe2("}");
            Escribe2("Console.ReadKey();");
            Escribe2("}");
            Escribe2("}");
            Escribe2("}");
        }
        private void Escribe(string instruccion)
        {
            if (instruccion == "}")
            {
                numeroTab--;
            }
            for (int i = 0; i < numeroTab; i++)
            {
                lenguaje.Write("\t");
            }
            if (instruccion == "{")
            {
                numeroTab++;
            }
                lenguaje.WriteLine(instruccion);
        }
        private void Escribe2(string instruccion)
        {
            if (instruccion == "}")
            {
                numeroTab--;
            }
            for (int i = 0; i < numeroTab; i++)
            {
                program.Write("\t");
            }
            if (instruccion == "{")
            {
                numeroTab++;
            }
                program.WriteLine(instruccion);
        }

        private void RepetidoIf()
        {

            if(esClasificacion(getContenido()))
            {
                Escribe("if (getClasificacion() == clasificaciones." + getContenido() + ")");
            }
            else
            {
                Escribe("if (getContenido() == \"" + getContenido() + "\")");
            }
            
            Escribe("" + "{");

            if (esClasificacion(getContenido()))
            {
                Escribe("match(clasificaciones." + getContenido() + ");");
            }
            else
            {
                Escribe("match(\"" + getContenido() + "\");");
            }

        }
    }
}