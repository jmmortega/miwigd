﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;

namespace DeadLineGames.MIWIGD.Objects.Common
{
    public class TransitionScreen : Screen
    {
        private ElementString m_message;

        private string m_screenToTransition;

        public TransitionScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            string title = string.Empty;

            if (base.Parameters.ContainsKey(Consts.PARAMETERTITLE) == true)
            {
                title = base.Parameters[Consts.PARAMETERTITLE].ToString();
            }

            if (base.Parameters.ContainsKey(Consts.PARAMETERSCREEN) == true)
            {
                m_screenToTransition = base.Parameters[Consts.PARAMETERSCREEN].ToString();
            }

            m_message = new ElementString(DesignOptions.Fuente, "Title", title, new Vector2(150, 150));
            m_message.Color = Color.White;

            (base.Game as Game1).ClearColor = Color.Black;

            base.Initialize();
        }

        public override void Update(TimeSpan elapsed)
        {
            base.Input = InputState.GetInputState();

            if((base.Input.KeyboardState.IsKeyDown(Keys.Space) == true) ||
            (base.Input.GamepadOne.IsButtonDown(Buttons.A) == true))
            {
                ScreenManager.TransitionTo(m_screenToTransition);
            }

            base.Update(elapsed);
        }

        public override void Draw()
        {
            m_message.Draw(base.SpriteBatch);
            base.Draw();
        }

    }
}
