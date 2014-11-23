using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Design.BackGrounds;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Class.Objects;
using NamoCode.Game.Class.Design;
using DeadLineGames.MIWIGD.Objects.NinethScreen;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using DeadLineGames.MIWIGD.Objects.Common;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Cabal Screen. A take to navero project.
    /// In other case create a puzzle game like a Candy Crush
    /// </summary>
    public class NinethScreen : Screen
    {
        private BackGround m_background;

        private AObjects m_runners;

        private Element m_winnerLabel;
        private Element m_winnerPosition;
        private bool m_endRace = false;
        
        public NinethScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();
            m_endRace = false;

            m_background = new BackGround(
                base.Content.Load<Texture2D>("NinethScreen/TrackAndField"),
                DesignOptions.Bounds,
                5, 0);

            m_runners = new AObjects();

            m_runners.Add(new Runner(
                base.Content.Load<Texture2D>("NinethScreen/Runner1"),
                "Runner1",
                new Vector2(-40, 100),
                DesignOptions.Bounds,
                new FrameRateInfo(12, 0.2f)));

            m_runners.Add(new Runner(
                base.Content.Load<Texture2D>("NinethScreen/Runner2"),
                "Runner2",
                new Vector2(-40, 150),
                DesignOptions.Bounds,
                new FrameRateInfo(12, 0.2f)));

            m_runners.Add(new Runner(
                base.Content.Load<Texture2D>("NinethScreen/Runner3"),
                "Runner3",
                new Vector2(-40, 200),
                DesignOptions.Bounds,
                new FrameRateInfo(12, 0.2f)));

            m_runners.Add(new Runner(
                base.Content.Load<Texture2D>("NinethScreen/Runner4"),
                "Runner4",
                new Vector2(-40, 250),
                DesignOptions.Bounds,
                new FrameRateInfo(12, 0.2f)));            
        }

        private bool firstPressed;
        private bool secondReleased;
        private bool thirdReleased;
        private bool fourReleased;

        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);

            base.Input = InputState.GetInputState();

            if (m_endRace == false)
            {
                if ((base.Input.GamepadOne.IsButtonDown(Buttons.A) == true || base.Input.GamepadOne.IsButtonDown(Buttons.B) == true
                    || base.Input.GamepadOne.IsButtonDown(Buttons.Y) == true || base.Input.GamepadOne.IsButtonDown(Buttons.X) == true ||
                    base.Input.KeyboardState.IsKeyDown(Keys.A) == true) && firstPressed == false)
                {
                    m_runners.GetElement("Runner1").MoverDerecha();
                    firstPressed = true;
                }

                if ((base.Input.GamepadTwo.IsButtonDown(Buttons.A) == true || base.Input.GamepadTwo.IsButtonDown(Buttons.B) == true
                    || base.Input.GamepadTwo.IsButtonDown(Buttons.Y) == true || base.Input.GamepadTwo.IsButtonDown(Buttons.X) == true ||
                    base.Input.KeyboardState.IsKeyDown(Keys.S) == true) && secondReleased == false)
                {
                    m_runners.GetElement("Runner2").MoverDerecha();
                    secondReleased = true;
                }

                if ((base.Input.GamepadThree.IsButtonDown(Buttons.A) == true || base.Input.GamepadThree.IsButtonDown(Buttons.B) == true
                    || base.Input.GamepadThree.IsButtonDown(Buttons.Y) == true || base.Input.GamepadThree.IsButtonDown(Buttons.X) == true ||
                    base.Input.KeyboardState.IsKeyDown(Keys.D) == true) && thirdReleased == false)
                {
                    m_runners.GetElement("Runner3").MoverDerecha();
                    thirdReleased = true;
                }

                if ((base.Input.GamepadFour.IsButtonDown(Buttons.A) == true || base.Input.GamepadFour.IsButtonDown(Buttons.B) == true
                    || base.Input.GamepadFour.IsButtonDown(Buttons.Y) == true || base.Input.GamepadFour.IsButtonDown(Buttons.X) == true ||
                    base.Input.KeyboardState.IsKeyDown(Keys.F) == true) && fourReleased == false)
                {
                    m_runners.GetElement("Runner4").MoverDerecha();
                    fourReleased = true;
                }
            }

            if (base.Input.GamepadOne.IsButtonDown(Buttons.Start) && m_endRace == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.TEN_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Tenth");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }

            m_background.DoMovement();
            m_runners.Update(elapsed);

            ChangeSlide();
            Release();
            CheckWinner();

            if (base.Input.KeyboardState.IsKeyDown(Keys.Escape) == true)
            {
                Game.Exit();
            }
        }

        private void Release()
        {
            if ((base.Input.GamepadOne.IsButtonUp(Buttons.A) == true && base.Input.GamepadOne.IsButtonUp(Buttons.B) == true
                && base.Input.GamepadOne.IsButtonUp(Buttons.Y) == true && base.Input.GamepadOne.IsButtonUp(Buttons.X) == true &&
                base.Input.KeyboardState.IsKeyUp(Keys.A) == true))
            {                                
                firstPressed = false;
            }

            if ((base.Input.GamepadTwo.IsButtonUp(Buttons.A) == true && base.Input.GamepadTwo.IsButtonUp(Buttons.B) == true
                && base.Input.GamepadTwo.IsButtonUp(Buttons.Y) == true && base.Input.GamepadTwo.IsButtonUp(Buttons.X) == true &&
                base.Input.KeyboardState.IsKeyUp(Keys.S) == true))
            {
                secondReleased = false;
            }

            if ((base.Input.GamepadThree.IsButtonUp(Buttons.A) == true && base.Input.GamepadThree.IsButtonUp(Buttons.B) == true
                && base.Input.GamepadThree.IsButtonUp(Buttons.Y) == true && base.Input.GamepadThree.IsButtonUp(Buttons.X) == true &&
                base.Input.KeyboardState.IsKeyUp(Keys.D) == true))
            {
                thirdReleased = false;
            }

            if ((base.Input.GamepadFour.IsButtonUp(Buttons.A) == true && base.Input.GamepadFour.IsButtonUp(Buttons.B) == true
                && base.Input.GamepadFour.IsButtonUp(Buttons.Y) == true && base.Input.GamepadFour.IsButtonUp(Buttons.X) == true &&
                base.Input.KeyboardState.IsKeyUp(Keys.F) == true))
            {
                fourReleased = false;
            }            
        }

        private const int c_winnerWidth = 570;

        public void CheckWinner()
        {
            foreach (AObject runner in m_runners)
            {
                if (runner.Posicion.X == c_winnerWidth)
                {
                    string nameRunner = runner.Name;

                    m_winnerLabel = new Element(base.Content.Load<Texture2D>("NinethScreen/Winner"), "WinnerLabel",
                        new Vector2(120, 200));

                    if (nameRunner == "Runner1")
                    {
                        m_winnerPosition = new Element(base.Content.Load<Texture2D>("NinethScreen/FirstPosition"), "Position",
                            new Vector2(150, 250));
                    }
                    else if (nameRunner == "Runner2")
                    {
                        m_winnerPosition = new Element(base.Content.Load<Texture2D>("NinethScreen/SecondPosition"), "Position",
                            new Vector2(200, 250));
                    }
                    else if (nameRunner == "Runner3")
                    {
                        m_winnerPosition = new Element(base.Content.Load<Texture2D>("NinethScreen/ThirdPosition"), "Position",
                            new Vector2(250, 250));
                    }
                    else if (nameRunner == "Runner4")
                    {
                        m_winnerPosition = new Element(base.Content.Load<Texture2D>("NinethScreen/ForthPosition"), "Position",
                            new Vector2(300, 250));
                    }

                    m_endRace = true;
                }                
            }
        }

        public override void Draw()
        {
            base.Draw();

            m_background.Draw(base.SpriteBatch);
            m_runners.Draw(base.SpriteBatch);
            
            if (m_endRace == true)
            {
                m_winnerLabel.Draw(base.SpriteBatch);
                m_winnerPosition.Draw(base.SpriteBatch);
            }
        }

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) || base.Input.KeyboardState.IsKeyDown(Keys.Q))
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.EIGHT_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Eight");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) || base.Input.KeyboardState.IsKeyDown(Keys.W))
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.TEN_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Tenth");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
        }

    }
}
