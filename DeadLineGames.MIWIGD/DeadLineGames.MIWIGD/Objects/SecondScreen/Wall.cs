using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;

namespace DeadLineGames.MIWIGD.Objects.SecondScreen
{
    class Wall : AObject
    {

        public Wall(Vector2 posicion)
            : base(BasicTextures.GetTexture("wall"),
            "wall",
            posicion)
        { }

    }
}
