using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework;

namespace DeadLineGames.MIWIGD.Objects.TenthScreen
{
    class Title : AnimatedElement
    {

        public Title(Vector2 posicion)
            : base(BasicTextures.GetTexture("MTitle"),
            "title",
            posicion,
            new FrameRateInfo())
        {
            this.Opacity = 0;
        }

        public override void Update(TimeSpan elapsedtime)
        {
            if (this.Opacity < 1)
                this.Opacity += 0.005f;
            else
            {
                float X = this.Posicion.X;
                float Y = this.Posicion.Y;

                if (this.Posicion.X < DesignOptions.Bounds.MaxX - this.Width - 10)
                    X += 0.25f;
                if (this.Posicion.Y > DesignOptions.Bounds.MinY + 10)
                    Y -= 0.25f;

                this.Posicion = new Vector2(X, Y);
            }
        }

    }
}
