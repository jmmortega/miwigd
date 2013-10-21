using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework;

namespace DeadLineGames.MIWIGD.Objects.TenthScreen
{
    class Dancers : AnimatedElement
    {

        public Dancers(Vector2 posicion)
            : base(BasicTextures.GetTexture("dancers"),
            "dancers",
            posicion,
            new FrameRateInfo(8, 0.15f, 8, true))
        {
            this.Opacity = 0;
        }

        public override void Update(TimeSpan elapsedtime)
        {
            if (this.Opacity < 1)
                this.Opacity += 0.001f;

            base.Update(elapsedtime);
        }

    }
}
