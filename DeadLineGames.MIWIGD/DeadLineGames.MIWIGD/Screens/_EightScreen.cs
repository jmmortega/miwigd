using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using DeadLineGames.MIWIGD.Objects.Common;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Paper boy screen. A simple lane.
    /// </summary>
    public class EightScreen : Screen
    {
        public EightScreen(Game game)
            : base(game)
        { }

        public override void Update(TimeSpan elapsed)
        {
            ChangeSlide();
            base.Update(elapsed);
        }

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SEVENTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Seventh");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.NINE_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Nineth");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
        }
    }
}
