using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;

namespace DeadLineGames.MIWIGD.Objects.SixthScreen
{
    public class Bug : AObject
    {

        private int velocity;
        public bool isEating;
        public bool isOutOfBounds;
        public int actuaFrame;
        public int framesCount;

        public Bug(String name, Vector2 posicion)
            : base(BasicTextures.GetTexture("Bugs"), 
            name, 
            new FrameRateInfo(7, 0, 1, false),
            posicion)
        {
            this.Frames.ChangeFrame(int.Parse(this.Name.Split('_')[1]));
            this.Frames.Pause = true;
            this.actuaFrame = this.ActualFrame;
            this.framesCount = this.Frames.FrameCount;
            this.velocity = getVelocity();
            isEating = false;
            isOutOfBounds = false;
        }

        public override void Update(TimeSpan elapsed)
        {
            this.SetPosicion(this.Posicion.X, this.Posicion.Y + velocity);
            if (this.Posicion.Y >= DesignOptions.Bounds.MaxY)
                isOutOfBounds = true;

            base.Update(elapsed);
        }

        private int getVelocity()
        {
            double probabilidad = 1 / 4.0;
            double porcentaje = Azar.Instance.GetPorcentual();
            for (int i = 1; i <= 4; i++)
            {
                if (porcentaje <= probabilidad * i)
                    return i;
            }
            return 0;
        }

    }
}
