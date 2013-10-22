/*
 * Creador : Chema
 * Fecha Creacion: 02/07/2012
 * Notas
 * 
 * Las opciones generales dentro del juego
 * 
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StarPaper.Class.Events.Director;

namespace StarPaper
{
    public static class StarPaperOptions
    {
        private static int m_Music = 50;

        /// <summary>
        /// Indica el volumen de la música en el juego. El valor va de 0 a 100
        /// </summary>
        /// <remarks>
        /// Posteriormente hay que convertirlo en un valor float
        /// </remarks>
        public static int Music
        {
            get { return StarPaperOptions.m_Music; }
            set { StarPaperOptions.m_Music = value; }
        }

        private static int m_Sonido = 50;

        /// <summary>
        /// Indica el volumen del sonido en el juego. El valor va de 0 a 100;
        /// </summary>
        /// <remarks>
        /// Posteriormente hay que convertirlo en un valor float
        /// </remarks>
        public static int Sonido
        {
            get { return StarPaperOptions.m_Sonido; }
            set { StarPaperOptions.m_Sonido = value; }
        }

        private static int m_Extend = 50000;

        /// <summary>
        /// Indica la cantidad de puntos necesaria para obtener una nueva vida
        /// </summary>
        /// <remarks>
        /// Los valores son 20000, 50000, 100000 , 500000
        /// </remarks>
        public static int Extend
        {
            get { return StarPaperOptions.m_Extend; }
            set { StarPaperOptions.m_Extend = value; }
        }

        private static EnumDificultad m_dificultad = EnumDificultad.Normal;

        /// <summary>
        /// Marca la dificultad dentro del juego.
        /// </summary>
        public static EnumDificultad Dificultad 
        {
            get { return StarPaperOptions.m_dificultad; }
            set { StarPaperOptions.m_dificultad = value; }
        }





    }
}
