using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework.Graphics;
using DeadLineGames.MIWIGD.Objects.SixthScreen;
using NamoCode.Game.Class.Media;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NamoCode.Game.Class.Input;
using DeadLineGames.MIWIGD.Objects.Common;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Bug hunting. 
    /// 
    /// Show a cross and shot bugs...
    /// </summary>
    public class SixthScreen : Screen
    {

        private AnimatedElement fondo;
        private Pumba pumba;
        private Timon timon;

        private bool EndScreen;

        public static int Score;

        public SixthScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            //BasicTextures.FreeMemory();

            fondo = new AnimatedElement(base.Content.Load<Texture2D>("SixthScreen/Background"),
                "fondo",
                new Vector2(DesignOptions.Bounds.MinX, DesignOptions.Bounds.MinY),
                new FrameRateInfo());

            cargarTimon();
            cargarPumba();

            BasicTextures.CargarTextura("SixthScreen/Bugs/Bugs", "Bugs");

            Score = 0;
            EndScreen = false;

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<Song>("SixthScreen/Theme"), "Bugs");
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("SixthScreen/Pumba/gulp"), "Gulp");
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("SixthScreen/Pumba/burp"), "Burp");
            Player.Instance.RepeatMusic = false;
            Player.Instance.Play("Bugs");

            base.Initialize();
        }

        public void cargarTimon()
        {
            BasicTextures.CargarTextura("SixthScreen/Timon/idle", "Tidle");
            BasicTextures.CargarTextura("SixthScreen/Timon/run", "Trun");
            BasicTextures.CargarTextura("SixthScreen/Timon/turn", "Tturn");
            timon = new Timon(new Vector2(285 - (BasicTextures.GetTexture("Tidle").Width),
                DesignOptions.Bounds.MinY - BasicTextures.GetTexture("Tidle").Height + 161));
        }

        public void cargarPumba()
        {
            BasicTextures.CargarTextura("SixthScreen/Pumba/idle", "idle");
            BasicTextures.CargarTextura("SixthScreen/Pumba/eating", "eating");
            BasicTextures.CargarTextura("SixthScreen/Pumba/startrun", "startrun");
            BasicTextures.CargarTextura("SixthScreen/Pumba/run", "run");
            BasicTextures.CargarTextura("SixthScreen/Pumba/turn", "turn");
            BasicTextures.CargarTextura("SixthScreen/Pumba/burping", "burp");
            pumba = new Pumba(new Vector2(285 - (BasicTextures.GetTexture("idle").Width / 4),
                DesignOptions.Bounds.MaxY - 80));
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

            if (!EndScreen)
            {
                pumba.Update(elapsed);
                if (!pumba.isBurping)
                {
                    timon.Update(elapsed);
                }
                Bugs.Instance.updateBugs(elapsed);
                checkCollisions();
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SEVENTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Seventh");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
            ChangeSlide();
            base.Update(elapsed);
        }

        public override void Draw()
        {
            fondo.Draw(base.SpriteBatch);
            timon.Draw(base.SpriteBatch);
            pumba.Draw(base.SpriteBatch);
            Bugs.Instance.Draw(base.SpriteBatch);
            base.Draw();
        }

        private void checkCollisions()
        {
            foreach (Bug b in Bugs.Instance)
            {
                if (pumba.HaveColision(b))
                {
                    if (b.Posicion.Y >= pumba.Posicion.Y - 5
                        && b.Posicion.Y <= pumba.Center.Y + 5)
                    {
                        b.isEating = true;
                        Score++;
                        if (Score == 10)
                        {
                            pumba.isBurping = true;
                            Player.Instance.Play("Burp");
                        }
                        else
                        {
                            if (pumba.estado == Objects.CharsState.Idle)
                                pumba.isEating = true;
                            Player.Instance.Play("Gulp");
                        }
                                
                        if (b.actuaFrame == b.framesCount - 1)
                            EndScreen = true;
                    }
                }
            }
        }

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.FIFTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Fifth");

                Player.Instance.Stop();
                ScreenManager.TransitionTo("TransitionScreen", parameters);                
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SEVENTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Seventh");

                Player.Instance.Stop();
                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
        }

    }
}
