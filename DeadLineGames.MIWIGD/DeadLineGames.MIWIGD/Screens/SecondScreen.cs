using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Zelda Screen to get the first sword in Zelda. Take de object, and appear a string with detail the technology
    /// that use. (Classic sound of Zelda)
    /// 
    /// The "weapons" are XNA+MonoGame | Unity | Cocos.
    /// 
    /// Allow exit to room and show other string with others technologies.
    /// </summary>
    public class SecondScreen : Screen
    {
        public SecondScreen(Game game)
            : base(game)
        { }
    }
}
