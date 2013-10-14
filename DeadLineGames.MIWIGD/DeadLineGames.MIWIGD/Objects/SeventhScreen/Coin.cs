using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Objects;
using NamoCode.Game.Utils;

namespace DeadLineGames.MIWIGD.Objects.SeventhScreen
{
    public class Coin : AObject
    {

        private float posYInint = 0;
        private int up = -5;
        private int cont = 0;
        public int actualFrame = 0;
        public bool activated { get; set; }

        public Coin(String name, Vector2 posicion)
            : base(BasicTextures.GetTexture("Coin"),
            name,
            new FrameRateInfo(4, 0.10f, 1, false),
            posicion)
        {
            this.Frames.Pause = true;
            posYInint = this.Posicion.Y;
            activated = false;
        }

        public override void Update(TimeSpan elapsed)
        {
            if (activated)
            {
                this.Frames.Pause = false;
                if (cont % 2 == 0)
                {
                    if (cont % 20 == 0)
                    {
                        up = -up;
                    }
                    this.SetPosicion(this.Posicion.X, this.Posicion.Y - up);
                    if (this.Posicion.Y == this.posYInint)
                    {
                        activated = false; 
                        this.Frames.Pause = true;
                    }
                }
                cont++;
            }

            base.Update(elapsed);
        }

    }
}
