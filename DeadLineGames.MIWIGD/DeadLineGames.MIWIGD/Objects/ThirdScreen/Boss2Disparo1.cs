/*
 * Creador : Chema 
 * Fecha Creacion: 11/07/2012
 * Notas
 * 
 * Creación de la clase disparo para el Boss2 
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Utils;

using StarPaper.Class.Events;
using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Shoots.ListaDisparos;

namespace StarPaper.Class.Objects.Shoots.ListaDisparos
{
    public class Boss2Disparo1 : Disparo , IMyCloneable
    {
        #region Rotaciones

        private const float c_derecha = 90;
        private const float c_izquierda = -90;
        private const float c_abajo = 180;

        #endregion

        #region Movimientos

        private MovementPatron m_arriba = new MovementPatron(
            new Vector4(0, -5, 0, 1000), new ExitBounds(false, false, false, false));

        private MovementPatron m_abajo = new MovementPatron(
            new Vector4(0, 5, 0, 1000), new ExitBounds(false, false, false, false));

        private MovementPatron m_derecha = new MovementPatron(
            new Vector4(5, 0, 1000, 0), new ExitBounds(false, false, false, false));

        private MovementPatron m_izquierda = new MovementPatron(
            new Vector4(-5, 0, 1000, 0), new ExitBounds(false, false, false, false));
            

        #endregion

        #region Constructor

        public Boss2Disparo1(Texture2D textura, string name, Bounds bounds)
            :base(textura , name , bounds)
        {
            m_direcciondisparo = EnumMovement.Arriba;

            SetDirection();

            base.Damage = 1;
        }

        public Boss2Disparo1(Texture2D textura, string name, Bounds bounds, EnumMovement direcciondisparo)
            :base(textura,name,bounds)
        {
            m_direcciondisparo = direcciondisparo;

            SetDirection();

            base.Damage = 1;
        }

        #region Fields

        private EnumMovement m_direcciondisparo = EnumMovement.None;

        public EnumMovement DireccionDisparo
        {
            get { return m_direcciondisparo; }
            set 
            {
                m_direcciondisparo = value; 
                SetDirection();
            }
        }

        #endregion

        #endregion

        #region Methods
        
        private void SetDirection()
        {
            if (m_direcciondisparo == EnumMovement.Izquierda)
            {
                base.Rotation = c_izquierda;
                base.PatronesMovimiento.Push((MovementPatron)m_izquierda.Clone());
            }
            else if (m_direcciondisparo == EnumMovement.Derecha)
            {
                base.Rotation = c_derecha;
                base.PatronesMovimiento.Push((MovementPatron)m_derecha.Clone());
            }
            else if (m_direcciondisparo == EnumMovement.Arriba)
            {
                base.Rotation = 0;
                base.PatronesMovimiento.Push((MovementPatron)m_arriba.Clone());
            }
            else if (m_direcciondisparo == EnumMovement.Abajo)
            {
                base.Rotation = c_abajo;
                base.PatronesMovimiento.Push((MovementPatron)m_abajo.Clone());
            }
        }

        #endregion

        #region IMyClonable

        public override object Clone()
        {
            return new Boss2Disparo1(this.Texture, this.Name, this.Bounds, this.DireccionDisparo);

            

            

        }

        #endregion
    }
}
