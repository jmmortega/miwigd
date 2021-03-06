﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Class.Media;
using Microsoft.Xna.Framework.Media;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using DeadLineGames.MIWIGD.Objects.Common;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Menu screen like a Contra Menu Screen
    /// Background Scrolling down to start menu.
    /// </summary>
    public class MenuScreen : Screen
    {

        private const int TIMESCROLL = 2;

        private AnimatedElement title;
        private AnimatedElement start;
        private Element _codemotionEd;

        private int timetoscroll = 0;
        private bool m_playedSong = false;

        public MenuScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            (base.Game as Game1).ClearColor = Color.Black;

            title = new AnimatedElement(base.Content.Load<Texture2D>("MenuScreen/Title"),
                "title",
                new Vector2(94.50f, DesignOptions.Bounds.MinY - 280),
                new FrameRateInfo());

            start = new AnimatedElement(base.Content.Load<Texture2D>("MenuScreen/PressStart"),
                "start",
                new Vector2(),
                new FrameRateInfo(2, 0.50f));
            start.Posicion = new Vector2(334.5f - (start.Width / 2), DesignOptions.Bounds.MaxY - 64);
            start.Visible = false;

            _codemotionEd = new Element(base.Content.Load<Texture2D>("MenuScreen/CodemotionEd"),
                "edition",
                new Vector2(260, DesignOptions.Bounds.MaxY - 100));
            _codemotionEd.Visible = false;
            
            timetoscroll = 0;
            base.Initialize();
        }

        protected override void GoBack()
        {
            Game.Exit();
        }

        public override void Update(TimeSpan elapsed)
        {
            if (m_playedSong == false)
            {
                Player.Instance.Sounds.Clear();
                Player.Instance.Sounds.Add(base.Content.Load<Song>("MenuScreen/TitleTheme"), "TitleTheme");
                Player.Instance.Play("TitleTheme");
                Player.Instance.RepeatMusic = false;
                m_playedSong = true;
            }

            if (title.Posicion.Y <= DesignOptions.Bounds.MinY + 30)
            {
                if (timetoscroll % TIMESCROLL == 0)
                    title.Posicion = new Vector2(title.Posicion.X, title.Posicion.Y + 2);
                else
                    timetoscroll++;
            }
            else
            {
                _codemotionEd.Visible = true;
                start.Visible = true;
                if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.Start) ||
                    InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Enter))
                {
                    //ScreenManager.TransitionTo("First");
                    Dictionary<string, object> parameters = new Dictionary<string,object>();

                    parameters.Add(Consts.PARAMETERTITLE, Strings.FIRST_TITLE);
                    parameters.Add(Consts.PARAMETERSCREEN, "First");
                    ScreenManager.TransitionTo("TransitionScreen", parameters);
                    Player.Instance.Stop();
                }
                if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.Back) ||
                    InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Escape))
                {
                    this.GoBack();
                }
            }

            start.Update(elapsed);

            base.Update(elapsed);
        }

        public override void Draw()
        {
            title.Draw(base.SpriteBatch);
            start.Draw(base.SpriteBatch);
            _codemotionEd.Draw(base.SpriteBatch);
            base.Draw();
        }

    }
}
