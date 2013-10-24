using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;

namespace DeadLineGames.MIWIGD.Objects.EighthScreen
{
    class Pirate : AnimatedElement
    {

        private const int TALKINGTIME = 200;
        private int talks = 0;

        public int contWinStakes = 0;

        public bool isTalking = false;
        public bool endTalking = false;
        public bool isFighting = false;
        public bool isLoosed = false;

        //public Pirate(Vector2 posicion)
        //    : base(BasicTextures.GetTexture("keeps"),
        //    "GuyBrush",
        //    posicion,
        //    new FrameRateInfo(4, 0, 2, false))
        //{ }
        public Pirate(Vector2 posicion)
            : base("Pirate",
            posicion,
            new FrameRateInfo(4, 0, 2, false))
        { }

        public override void  Update(TimeSpan elapsedtime)
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
        
        }

    }
}
