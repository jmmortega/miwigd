/*
 * Creador : Chema 
 * Fecha Creacion: 10/07/2012
 * Notas
 * 
 * Disparo
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
    public class DisparoMovil : Disparo
    {
        private const int c_velocidadDisparo = 3;

        public DisparoMovil(Texture2D textura, string name, Bounds bounds)
            : base(textura, name, bounds)
        {
            base.Damage = 1;
        }


        #region Fields
        
        #endregion

        #region Methods

        /// <summary>
        /// Calcula el movimiento del disparo, según los radianes del ángulo
        /// </summary>
        /// <param name="radians"></param>
        /// <remarks>
        /// El cálculo para la posición se puede encontrar en la segunda respuesta del foro
        /// http://forums.create.msdn.com/forums/t/13803.aspx
        /// </remarks>
        public void SetMovement(double radians)
        {
            Vector2 vector = GodClass.CalcVectorToAngle(radians);

            Vector4 movementPatron = new Vector4(
                vector.X * c_velocidadDisparo,
                vector.Y * c_velocidadDisparo,
                1000,
                1000);

            MovementPatron patron = new MovementPatron(movementPatron, new ExitBounds(false, false, false, false));

            base.PatronesMovimiento.Push(patron);
                
        }

        public override object Clone()
        {
            DisparoMovil Disparo = new DisparoMovil(base.Texture, base.Name, base.Bounds);

            if (PatronesMovimiento.Count > 0)
            {
                Disparo.PatronesMovimiento = this.PatronesMovimiento;
            }

            return Disparo;
        }

        #endregion
    }
}
