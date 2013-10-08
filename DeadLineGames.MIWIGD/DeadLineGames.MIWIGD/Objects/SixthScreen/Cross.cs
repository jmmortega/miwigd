using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadLineGames.MIWIGD.Objects.SixthScreen
{
    public class Cross : AObject
    {
        public Cross(Texture2D textura, String name, Vector2 posicion)
            : base(textura, name, posicion)
        { }
    }
}
