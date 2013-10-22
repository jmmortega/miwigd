/*
 * Creador : Chema
 * Fecha Creacion: 08/06/2012
 * Notas
 * 
 * Es el tipo de disparo del prota.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase.
 * 
 * 1.1 14/06/2012
 * 
 * Se agrega el daño por disparo.
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
    public class GoodGuyShoot : Disparo , IGoodGuyShoot
    {
        public GoodGuyShoot(Texture2D texture , string name , Bounds bounds)
            :base(texture , name , bounds)
        {
            base.PatronesMovimiento.Push
                (new Events.MovementPatron(
                    new Vector4(0, -3, 0, 1000),
                    new Events.ExitBounds(false, false, false, false)));

            base.Damage = 1;
        }
        
        public override object Clone()
        {
            return new GoodGuyShoot(base.Texture, base.Name, base.Bounds);
        }
    }
}
