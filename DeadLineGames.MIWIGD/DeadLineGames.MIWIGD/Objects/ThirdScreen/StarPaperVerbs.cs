
/*
 * Creador : Chema 
 * Fecha Creacion: 30/05/2012
 * Notas
 * 
 * Verbos únicos para el juego.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * Creación de la clase.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NamoCode.Game.Class.Events;

namespace StarPaper.Class.Events
{
    public class StarPaperVerbs
    {
        public static Verb Prueba = new Verb("Test", 0);

        public static Verb[] Verbos = { Prueba };

        /// <summary>
        /// Recupera el verbo indicado por la descripción.
        /// </summary>
        /// <param name="descripcion"></param>
        /// <returns>
        /// En caso de no localizar el buscado devuelve un Verbo de tipo None generico de la libreria.
        /// </returns>
        public static Verb SearchVerb(string descripcion)
        {
            foreach (Verb verbo in Verbos)
            {
                if (verbo.Description == descripcion)
                {
                    return verbo;
                }
            }

            return Verbs.None;
        }
    }
}
