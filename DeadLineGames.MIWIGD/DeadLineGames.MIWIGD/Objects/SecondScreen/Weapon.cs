using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Class.Objects;
using NamoCode.Game.Utils;

namespace DeadLineGames.MIWIGD.Objects.SecondScreen
{
    public class Weapon : AObject
    {

        public bool catched { get; set; }

        public Weapon(Texture2D textura, String name, Vector2 posicion)
            : base(textura, name, new FrameRateInfo(), posicion)
        {
            catched = false;
            base.Frames = new NamoCode.Game.Utils.FrameRateInfo();
        }
    }
}
