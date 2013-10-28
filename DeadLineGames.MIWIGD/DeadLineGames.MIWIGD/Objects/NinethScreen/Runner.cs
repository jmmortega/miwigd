using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DeadLineGames.MIWIGD.Objects.NinethScreen
{
    public class Runner : AObject
    {
        public Runner(Texture2D texture, string name, Vector2 position, Bounds bounds, FrameRateInfo frame)
            : base(texture, name, position, frame, bounds)
        {
            base.Movimiento = 10;
            base.Frames.Auto = true;
            base.Frames.TypePlaying = EnumPlaying.Loop;
        }
            
        
    }
}
