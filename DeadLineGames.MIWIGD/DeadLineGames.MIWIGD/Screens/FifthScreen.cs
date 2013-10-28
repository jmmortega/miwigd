using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using DeadLineGames.MIWIGD.Objects.FifthScreen;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NamoCode.Game.Class.Input;
using NamoCode.Game.Class.Media;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using DeadLineGames.MIWIGD.Objects.Common;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Create a QTE game. (Required a QTEManager). I think a developer we need press a random button combination to 
    /// achieve the game, and later catch bugs with a QTE.
    /// </summary>
    public class FifthScreen : Screen
    {

        private AnimatedElement back;
        private FrameRateInfo baseFrame;
        private Buttons button;

        private int TIMETOCHANGEBUTTON;
        private int timeToChange;

        private int score;
        private ElementString Score;

        private int time;
        private ElementString Time;
        private TimeSpan ts;

        private bool press;
        private Buttons bPress;

        DateTime dt;

        public FifthScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            baseFrame = new FrameRateInfo(2, 0.80f, 2, true);
            //BasicTextures.FreeMemory();
            back = new AnimatedElement(base.Content.Load<Texture2D>("FifthScreen/programando"),
                "back",
                new Vector2(DesignOptions.Bounds.MinX, DesignOptions.Bounds.MinY),
                new FrameRateInfo(2, 0.80f, 2, true));

            TIMETOCHANGEBUTTON = Azar.Instance.GetNumber(50, 300);
            timeToChange = 0;
            
            score = 0;
            Score = new ElementString(base.Content.Load<SpriteFont>("FirstScreen/PokemonFont"),
                "Score",
                "Score: " + score,
                new Vector2(DesignOptions.Bounds.MinX + 20,
                    DesignOptions.Bounds.MinY + 73));
            
            time = 50;
            Time = new ElementString(base.Content.Load<SpriteFont>("FirstScreen/PokemonFont"),
                "Time",
                "Time: " + time,
                new Vector2(DesignOptions.Bounds.MinX + 20,
                    Score.Posicion.Y + Score.Height));

            Score.Color = Time.Color = Color.DarkRed;
            
            press = false;

            dt = DateTime.Now;

            QTEPusher.Instance.setPosition(new Vector2(DesignOptions.Bounds.MinX + 285 - 68, DesignOptions.Bounds.MaxY - 84));

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<Song>("FifthScreen/theme"), "EyeTiger");
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("FifthScreen/punch"), "Punch");
            Player.Instance.RepeatMusic = true;
            Player.Instance.Play("EyeTiger", 0.70f);

            base.Initialize();
        }

        protected void GoBack()
        {
            ScreenManager.TransitionTo("Menu");
        }

        public override void Update(TimeSpan elapsed)
        {

            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.Back)
                || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Escape))
                this.GoBack();

            if (time > 0)
            {
                back.Update(elapsed);

                if (timeToChange % TIMETOCHANGEBUTTON == 0)
                {
                    QTEPusher.Instance.changeButton();
                    TIMETOCHANGEBUTTON = Azar.Instance.GetNumber(100, 500);
                }
                timeToChange++;

                if (isButtonReleased())
                {
                    score++;
                    press = false;
                    if (back.SecondPerFrame > 0.10f)
                        back.SecondPerFrame -= 0.05f;
                    Player.Instance.Play("Punch");
                }
                else
                {
                    if (bPress != QTEPusher.Instance.AButton)
                    {
                        back.SecondPerFrame = baseFrame.SecondPerFrame;
                        press = false;
                    }
                }

                if (QTEPusher.Instance.AButton == getPressButton())
                {
                    bPress = QTEPusher.Instance.AButton;
                    press = true;
                }

                updateStrings(elapsed);

                QTEPusher.Instance.playButton(elapsed);
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SIXTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Sixth");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }

            ChangeSlide();
            base.Update(elapsed);
        }

        private Buttons getPressButton()
        {
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.A)
                || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.S))
                return Buttons.A;
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.B)
                || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.D))
                return Buttons.B;
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.X)
                || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.A))
                return Buttons.X;
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.Y)
                || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.W))
                return Buttons.Y;
            return Buttons.Back;
        }

        private bool isButtonReleased()
        {

            if (bPress == Buttons.A && press)
            {
                if (InputState.GetInputState().GamepadOne.IsButtonUp(Buttons.A) 
                    && InputState.GetInputState().KeyboardState.IsKeyUp(Keys.S))
                    return true;
            }
            if (bPress == Buttons.B && press)
            {
                if (InputState.GetInputState().GamepadOne.IsButtonUp(Buttons.B)
                    && InputState.GetInputState().KeyboardState.IsKeyUp(Keys.D))
                    return true;
            }
            if (bPress == Buttons.X && press)
            {
                if (InputState.GetInputState().GamepadOne.IsButtonUp(Buttons.X)
                    && InputState.GetInputState().KeyboardState.IsKeyUp(Keys.A))
                    return true;
            }
            if (bPress == Buttons.Y && press)
            {
                if (InputState.GetInputState().GamepadOne.IsButtonUp(Buttons.Y)
                    && InputState.GetInputState().KeyboardState.IsKeyUp(Keys.W))
                    return true;
            }

            return false;
        }

        private void updateStrings(TimeSpan elapsed)
        {
            Score.LabelContent = "Score: " + score;
            DateTime now = DateTime.Now;
            if (now.Second - dt.Second == 1
                || now.Minute - dt.Minute == 1)
            {
                Time.LabelContent = "Time: " + --time;
                dt = now;
            }
            

            Score.Update(elapsed);
            Time.Update(elapsed);
        }

        public override void Draw()
        {
            back.Draw(base.SpriteBatch);
            Score.Draw(base.SpriteBatch);
            Time.Draw(base.SpriteBatch);
            QTEPusher.Instance.Draw(base.SpriteBatch);
            base.Draw();
        }

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.FORTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Forth");
                Player.Instance.Stop();

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SIXTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Sixth");
                Player.Instance.Stop();

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
        }

    }
}
