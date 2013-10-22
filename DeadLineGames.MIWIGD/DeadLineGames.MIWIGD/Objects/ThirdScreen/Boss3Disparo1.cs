/*
 * Creador : Chema 
 * Fecha Creacion: 12/07/2012
 * Notas
 * 
 * El disparo potente del tercer monstruo
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
 
namespace StarPaper.Class.Objects.Shoots.ListaDisparos
{
    public class Boss3Disparo1 : Disparo
    {
        #region Movimientos

        private MovementPatron m_izquierda = new MovementPatron(
            new Vector4(-5, 0, 1000, 0), new ExitBounds(false, false, false, false));

        private MovementPatron m_derecha = new MovementPatron(
            new Vector4(5, 0, 1000, 0), new ExitBounds(false, false, false, false));

        #endregion

        #region Constructor

        public Boss3Disparo1(Texture2D texture, string name, Bounds bounds , EnumMovement direccionDisparo)
            : base(texture, name, bounds)
        {
            base.Damage = 30;
            m_direccionDisparo = direccionDisparo;

            SetDirection();
        }

        #endregion

        #region Fields

        private EnumMovement m_direccionDisparo = EnumMovement.Izquierda;

        public EnumMovement DireccionDisparo
        {
            get { return m_direccionDisparo; }
            set 
            { 
                m_direccionDisparo = value;
                SetDirection();
            }
        }

        #endregion

        #region Methods

        private void SetDirection()
        {
            if (m_direccionDisparo == EnumMovement.Izquierda)
            {
                base.PatronesMovimiento.Push((MovementPatron)m_izquierda.Clone());
            }
            else if (m_direccionDisparo == EnumMovement.Derecha)
            {
                base.PatronesMovimiento.Push((MovementPatron)m_derecha.Clone());
                base.Rotation = 180; //Tenemos que darle la vuelta porque la figura apunta para la izquierda.
                
            }
        }

        #endregion

        #region IMyClonable

        public override object Clone()
        {
            return new Boss3Disparo1(this.Texture, this.Name, this.Bounds, this.m_direccionDisparo);
        }

        #endregion
    }
}
