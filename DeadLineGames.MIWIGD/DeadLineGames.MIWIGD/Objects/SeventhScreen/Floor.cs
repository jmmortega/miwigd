using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace DeadLineGames.MIWIGD.Objects.SeventhScreen
{
    class Floor : AObject
    {

        public Floor(Texture2D textura, Vector2 posicion)
            : base(textura,
            "floor",
            new FrameRateInfo(),
            posicion)
        { }

    }
}
