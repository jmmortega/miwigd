using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;

namespace DeadLineGames.MIWIGD.Objects.SeventhScreen
{
    class Block : AObject
    {

        private float posYInint = 0;
        private int up = -5;
        private int cont = 0;
        public int actualFrame = 0;
        public bool activated { get; set; }

        public Block(String name, Vector2 posicion)
            : base(BasicTextures.GetTexture("Block"),
            name,
            new FrameRateInfo(2, 0),
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
                if (cont % 2 == 0)
                {
                    actualFrame = this.Frames.ActualFrame = 1;
                    if(cont % 4 == 0)
                        up = -up;
                    this.SetPosicion(this.Posicion.X, this.Posicion.Y - up);
                    if (this.Posicion.Y == this.posYInint)
                    {
                        activated = false;
                    }
                }
                cont++;
            }

            base.Update(elapsed);
        }

    }
}
