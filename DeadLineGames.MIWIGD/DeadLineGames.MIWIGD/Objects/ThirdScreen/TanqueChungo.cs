using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Utils;

using StarPaper.Class.Objects.Shoots.ListaDisparos;
using StarPaper.Utils;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class TanqueChungo : EnemigoParteMovil
    {

        public TanqueChungo(Texture2D texture, Texture2D movepart, string name, Bounds bounds)
            : base(texture, movepart, name, bounds)
        { 
                //TODO: Test
            base.Shield = 5;

            base.TypeShot = new DisparoMovil(ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds);

            base.ShootLogic = new Shoots.ShootLogic(1000, 1);

            base.StartShooting();

            //Hay que usar decimales para que el movimiento sea "bonito"
            base.PatronesMovimiento.Push(new Events.MovementPatron(
                new Vector4(0, 0.5f, 10000, 10000), new Events.ExitBounds(false, false, false, false)));

        }

        
        
    }
}
