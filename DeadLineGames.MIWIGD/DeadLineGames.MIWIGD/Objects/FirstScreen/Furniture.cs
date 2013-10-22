using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DeadLinesGames.MIWIGD.Objects.FirstScreen
{
    public class Furniture : AObject
    {
        public Furniture(Texture2D texture, string name, Vector2 posicion)
            : base(texture, name, posicion)
        { }        
    }
}
