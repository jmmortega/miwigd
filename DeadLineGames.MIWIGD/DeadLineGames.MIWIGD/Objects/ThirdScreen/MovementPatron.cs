/*
 * Creador : Chema
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * El patrón de movimiento.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase.
 * 
 * 1.1 16/05/2012
 * 
 * Se modifica el booleano de ExitBounds por la estructura ExitBounds
 * Se agrega el método cambio de movimiento.
 * 
 * 1.2 06/06/2012
 * 
 * Se integra la interfaz IDisposable.
 * 
 * 1.3 10/07/2012
 * 
 * Se modifica el método GetMovement, para no hacer la conversión de los valores X e Y a enteros.
 * Se retira del método GetMovement la comprobación de los valores 0 para null.
 * 
 * 1.4 11/07/2012
 * 
 * Se implementa el IMyCloneable a la clase.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using NamoCode.Game.Utils;

namespace StarPaper.Class.Events
{
    /// <summary>
    /// 
    /// </summary>    
    public class MovementPatron : IDisposable , IMyCloneable
    {

        #region Constructor

        /// <summary>
        /// Constructor del patrón de movimiento
        /// </summary>
        /// <param name="movimiento">
        /// La dirección del movimiento
        /// Siendo un vector 2. 
        /// Siendo X la velocidad que se mueve en el eje X
        /// Siendo Y la velocidad que se mueve en el eje Y
        /// Siendo Z la cantidad recorrida en el eje X
        /// Siendo W la cantidad recorrida en el eje Y
        /// (Cuando el valor de Z o W sea 0 es que llega hasta el final de la pantalla)
        /// </param>
        /// <param name="amount">
        /// La cantidad de movimiento desplazado 
        /// </param>
        /// <param name="exitbounds">
        /// Indica si este movimiento lo sacará de pantalla o no.
        /// </param>
        public MovementPatron(Vector4 movimiento , ExitBounds exitbounds)
        {
            m_movimiento = movimiento;
            
            m_exitBounds = exitbounds;                        
        }

        #endregion

        #region Fields

        private Vector4 m_movimiento;

        public Vector4 Movimiento
        {
            get { return m_movimiento; }
            set { m_movimiento = value; }
        }
       
        private ExitBounds m_exitBounds;

        /// <summary>
        /// True en caso de que se salga y false en caso contrario
        /// </summary>
        /// <remarks>
        /// Llego a pensar que un booleano se me queda corto... 
        /// Debería necesitar tres valores:
        /// Se sale de pantalla
        /// Se queda en pantalla
        /// Se queda en pantalla y rebota.
        /// </remarks>
        public ExitBounds ExitBounds
        {
            get { return m_exitBounds; }
            set { m_exitBounds = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recupera una unidad de movimiento
        /// </summary>
        /// <returns>
        /// Recupera los movimientos en ese momento.
        /// Si devuelve null, indica que ya no hay movimientos válidos para ese objeto.
        /// </returns>
        /// <remarks>
        /// He indicado que me devuelva null una estructura. 
        /// Creo que cuando haga el port, me va a dar error, porque los tipos nullables no existían antes.
        /// </remarks>
        public Vector2? GetMovement()
        {
            float x = 0;
            float y = 0;

            if (m_movimiento.Z > 0)
            {
                m_movimiento.Z -= 1;
                x = m_movimiento.X;
            }
            if (m_movimiento.W > 0)
            {
                m_movimiento.W -= 1;
                y = m_movimiento.Y;
            }

            //if (x != 0 || y != 0)
            if(m_movimiento.W != 0 || m_movimiento.Z != 0)
            {
                return new Vector2(x, y);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Modifica el movimiento producido
        /// </summary>
        /// <param name="colision">
        /// Donde se produjo la colisión.
        /// </param>
        public void ChangeMovement(EnumMovement colision)
        {
            if (colision == EnumMovement.Arriba)
            {
                m_movimiento = new Vector4(m_movimiento.X, -m_movimiento.Y, m_movimiento.Z, m_movimiento.W);
            }
            else if (colision == EnumMovement.Derecha)
            {
                m_movimiento = new Vector4(-m_movimiento.X, m_movimiento.Y, m_movimiento.Z, m_movimiento.W);
            }
            else if (colision == EnumMovement.Abajo)
            {
                m_movimiento = new Vector4(m_movimiento.X, -m_movimiento.Y, m_movimiento.Z, m_movimiento.W);
            }
            else if (colision == EnumMovement.Izquierda)
            {
                m_movimiento = new Vector4(-m_movimiento.X, m_movimiento.Y, m_movimiento.Z, m_movimiento.W);
            }
        }

        /// <summary>
        /// Modifica el movimimiento del vector pasado
        /// </summary>
        /// <param name="colision"></param>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        public Vector2? ChangeMovement(EnumMovement colision, Vector2? movimiento)
        {
            if (colision == EnumMovement.Arriba)
            {
                return new Vector2(movimiento.Value.X , -movimiento.Value.Y);
            }
            else if (colision == EnumMovement.Derecha)
            {
                return new Vector2(-movimiento.Value.X, movimiento.Value.Y);
            }
            else if (colision == EnumMovement.Abajo)
            {
                return new Vector2(movimiento.Value.X, -movimiento.Value.Y);
            }
            else if (colision == EnumMovement.Izquierda)
            {
                return new Vector2(-movimiento.Value.X, movimiento.Value.Y);
            }
            else 
            {
                return movimiento;
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);            
        }

        protected virtual void Dispose(bool disposing)
        {
            GC.SuppressFinalize(this);
        }

        ~MovementPatron()
        {
            Dispose(false);
        }

        #endregion

        #region IMyCloneable

        public object Clone()
        {
            return new MovementPatron(
                new Vector4(m_movimiento.X, m_movimiento.Y, m_movimiento.Z, m_movimiento.W), m_exitBounds);

           
        }

        #endregion
    }


}
