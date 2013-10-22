/*
 * Creador : Chema 
 * Fecha Creacion: 29/06/2012
 * Notas
 * 
 * Clase que representa el bonus para daño.
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

using NamoCode.Game.Class.Objects;
using NamoCode.Game.Utils;

using StarPaper.Class.Objects.Enemies;

namespace StarPaper.Class.Objects.Buenos
{
    /// <summary>
    /// Hereda de enemigo para poder aprovechar ciertas caracteristicas del mismo.
    /// </summary>
    public class BonusObject : Enemigo, IBonus
    {
        #region Constructor

        public BonusObject(Texture2D texture , string name , Bounds bounds , EnumBonusType bonusType)
            :base(texture , name , bounds)
        {
            m_BonusType = bonusType;

            base.PatronesMovimiento.Push(new Events.MovementPatron(
                new Vector4(0, 2, 0, 1000),
                new Events.ExitBounds(false, false, false, false)));

            base.Shield = 0;
        }

        #endregion

        #region Fields

        private EnumBonusType m_BonusType;

        /// <summary>
        /// El tipo de bonus del que se trata.
        /// </summary>
        public EnumBonusType BonusType
        {
            get { return m_BonusType; }
            set { m_BonusType = value; }
        }

        #endregion

        #region Methods

        public override void Kill()
        {
            this.Dispose();
        }

        #endregion
    }
}
