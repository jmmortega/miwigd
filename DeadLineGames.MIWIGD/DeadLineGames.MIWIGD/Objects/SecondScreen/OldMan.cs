using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using NamoCode.Game.Class.Design;

namespace DeadLineGames.MIWIGD.Objects.SecondScreen
{
    public class OldMan : AObject
    {

        public OldMan(Vector2 posicion)
            : base(BasicTextures.CargarTextura("SecondScreen/OldMan", "OldMan"),
            "OldMan",
            new FrameRateInfo(),
            posicion)
        { }

    }
}
