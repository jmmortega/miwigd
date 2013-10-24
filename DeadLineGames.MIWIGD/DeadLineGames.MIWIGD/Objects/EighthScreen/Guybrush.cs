using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework;

namespace DeadLineGames.MIWIGD.Objects.EighthScreen
{
    class Guybrush: AnimatedElement
    {

        private const int TALKINGTIME = 200;
        private int talks = 0;

        public int contWinStakes = 0;

        public bool isTalking = false;
        public bool endTalking = false;
        public bool isFighting = false;
        public bool isLoosed = false;

        public Guybrush(Vector2 posicion)
            : base(BasicTextures.GetTexture("guybrush-speak"),
            "GuyBrush",
            posicion,
            new FrameRateInfo(5, 0.15f, 1, false))
        { }

        public override void Update(TimeSpan elapsedtime)
        {

            if (isTalking)
            {
                if (talks < TALKINGTIME)
                    talks++;
                else
                {
                    talks = 0;
                    endTalking = true;
                    isTalking = false;
                }
            }

            base.Update(elapsedtime);
        }

    }
}
