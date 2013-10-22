/*
 * Creador : Chema
 * Fecha Creacion: 06/07/2012
 * Notas
 * 
 * Primer tipo de disparo del Boss1
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Utils;

namespace StarPaper.Class.Objects.Shoots.ListaDisparos
{
    public class Boss1Disparo1 : Disparo 
    {
        public Boss1Disparo1(Texture2D textura, string name, Bounds bounds)
            : base(textura, name, bounds)
        {
            base.Damage = 3;
        }

        #region ChangePatron
        
        private static float m_y = -10;
        private static float m_x = 0;

        private static float m_valuey = 1;
        private static float m_valuex = 1;

        /// <summary>
        /// Modifica el patrón de movimiento del disparo
        /// </summary>
        /// <returns>
        /// Devuelve true, cuando llega al final del ciclo.
        /// </returns>
        public bool ChangePatron(bool inverse)
        {
            bool endCicle = false;

            if (m_y == -10 && m_x == 0)
            {
                endCicle = true;
                m_valuex = 1;
                m_valuey = 1;
            }
            else if (m_y == 0 && m_x == 10)
            {
                m_valuex = -1;
                m_valuey = 1;
            }
            else if (m_y == 10 && m_x == 0)
            {
                m_valuex = -1;
                m_valuey = -1;
            }
            else if (m_y == 0 && m_x == -10)
            {
                m_valuex = 1;
                m_valuey = -1;                
            }

            m_x = m_x + m_valuex;
            m_y = m_y + m_valuey;

            
            base.PatronesMovimiento.Clear();

            if (inverse == true)
            {
                base.PatronesMovimiento.Push(new Events.MovementPatron(
                 new Vector4(-m_x, -m_y, 1000, 1000), new Events.ExitBounds(false, false, false, false)));
            }
            else
            {
                base.PatronesMovimiento.Push(new Events.MovementPatron(
                    new Vector4(m_x, m_y, 1000, 1000), new Events.ExitBounds(false, false, false, false)));
            }

            return endCicle;

        }

        public void Inverse()
        {
            
            Vector4 oldMovement = base.PatronesMovimiento.Pop().Movimiento;

            base.PatronesMovimiento.Push(new Events.MovementPatron(
                new Vector4(-oldMovement.X, -oldMovement.Y, 1000, 1000),
                new Events.ExitBounds(false, false, false, false)));            
        }

        #endregion

        public override object Clone()
        {
            return new Boss1Disparo1(this.Texture, this.Name, this.Bounds);
        }

        
    }
}
