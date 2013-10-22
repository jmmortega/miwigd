/*
 * Creador : Chema
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * Estructura control que verifica que hay que hacer cuando un objeto se choca contra uno de los limites de la pantalla.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la estructura
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarPaper.Class.Events
{
    public struct ExitBounds
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">
        /// True: Chequea si sale por los laterales
        /// </param>
        /// <param name="y">
        /// True: Chequea si sale por los horizontales
        /// </param>
        /// <param name="effectx">
        /// True: Produce un efecto rebote, invirtiendo el movimiento en este eje
        /// </param>
        /// <param name="effecty">
        /// True: Produce un efecto rebote, invirtiendo el movimiento en este eje
        /// </param>
        public ExitBounds(bool x, bool y, bool effectx, bool effecty)
        {
            m_x = x;
            m_y = y;
            m_effectX = effectx;
            m_effectY = effecty;
        }

        private bool m_x;

        /// <summary>
        /// Indica si hay que chequear que los laterales salga del limite
        /// </summary>
        public bool X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        private bool m_y;

        /// <summary>
        /// Indica si se requiere chequeo arriba y abajo de que salga del limite.
        /// </summary>
        public bool Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        private bool m_effectX;

        /// <summary>
        /// En caso de que sea positivo se producirá un efecto inverso de movimiento, rebota sobre el eje.
        /// </summary>
        public bool EffectX
        {
            get { return m_effectX; }
            set { m_effectX = value; }
        }

        private bool m_effectY;

        /// <summary>
        /// En caso de que sea positivo se producirá un efecto inverso de movimiento, hace rebotar sobre el eje.
        /// </summary>
        public bool EffectY
        {
            get { return m_effectY; }
            set { m_effectY = value; }
        }

        /// <summary>
        /// Propiedad que confirma si es necesario comprobar si se sale del área de juego.
        /// </summary>
        public bool IsRequiredCheck
        {
            get
            {
                return m_x || m_y;
            }
        }

        public bool HaveEffect
        {
            get
            {
                return m_effectX || m_effectY;
            }
        }




    }
}
