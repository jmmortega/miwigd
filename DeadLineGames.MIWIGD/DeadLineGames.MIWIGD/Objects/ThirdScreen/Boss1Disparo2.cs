/*
 * Creador : Chema
 * Fecha Creacion: 06/07/2012
 * Notas
 * 
 * Segundo tipo de disparo del Boss 1
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
    public class Boss1Disparo2 : Disparo 
    {
        public Boss1Disparo2(Texture2D textura, string name, Bounds bounds)
            : base(textura, name, bounds)
        {
            base.Damage = 1;

            base.PatronesMovimiento.Push(new Events.MovementPatron(
                new Vector4(0, 10, 0, 1000), new Events.ExitBounds(false, false, false, false)));
        }

        public override object Clone()
        {
            return new Boss1Disparo2(this.Texture, this.Name, this.Bounds);
        }

    }
}
