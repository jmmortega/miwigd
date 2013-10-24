using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Design.Hud;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework.Graphics;
using DeadLineGames.MIWIGD.Objects.Common;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NamoCode.Game.Class.Media;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Detail screen for Oaklindo speak monologue
    /// </summary>
    public class FirstScreenDetail : Screen
    {       
        private DialogControl m_dialogControl;        
        private Element m_oakLindo;
        
        private int m_dialogSequence = 0;

        public FirstScreenDetail(Game game)
            : base(game)
        {            
            m_oakLindo = new Element(
                base.Content.Load<Texture2D>("FirstScreen/ProfessorOakDetail"), "ProfDetail",
                new Vector2(275, 80));

            m_dialogControl = new DialogControl(
                "SampleDialog", new Vector2(90, 250), Strings.FIRSTOAK, base.Content.Load<Texture2D>("FirstScreen/PokemonFrame"));

            m_dialogControl.Visible = true;
            m_dialogControl.TextController.Color = Color.Black;
            m_dialogControl.WaitToNextLine = true;
            m_dialogControl.Color = Color.Black;

            m_dialogControl.OnEndOfDialog += new DialogControl.EndOfDialog(DialogEnd);
            m_dialogControl.OnTextLoaded += new DialogControl.TextLoaded(TextLoaded);

            
        }

        public override void Initialize()
        {
            
            base.Initialize();
        }

        private void TextLoaded(object sender, EventArgs e)
        {
            m_dialogSequence++;
        }

        private void DialogEnd(object sender, NamoCode.Game.Class.EventArguments.EndOfDialogEventArgs e)
        {            
            if (m_dialogSequence == 0)
            {
                m_dialogControl.LoadText(Strings.SECONDOAK);
            }
            else if (m_dialogSequence == 1)
            {
                m_dialogControl.LoadText(Strings.THIRDOAK);
            }
            else if (m_dialogSequence == 2)
            {
                m_dialogControl.LoadText(Strings.FORTHOAK);
            }
            else if (m_dialogSequence == 3)
            {
                m_dialogControl.LoadText(Strings.FIFTHOAK);
            }
            else if (m_dialogSequence == 4)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                parameters.Add(Consts.PARAMETERTITLE , Strings.SECOND_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN , "Second");

                ScreenManager.TransitionTo("TransitionScreen", parameters);

                Player.Instance.Stop();
            }
        }

        private bool m_pressed = false;        

        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);

            m_dialogControl.Update(elapsed);

            base.Input = InputState.GetInputState();

            if (((base.Input.KeyboardState.IsKeyDown(Keys.Space) == true) ||
                (base.Input.GamepadOne.IsButtonDown(Buttons.A) == true)) && m_pressed == false)
            {
                m_pressed = true;
                m_dialogControl.NextLine();                
            }
            else if((base.Input.KeyboardState.IsKeyUp(Keys.Space) == true) &&
                (base.Input.GamepadOne.IsButtonUp(Buttons.A) == true))
            {
                m_pressed = false;
            }

            ChangeSlide();
        }

        public override void Draw()
        {
            (base.Game as Game1).ClearColor = Color.White;
            m_oakLindo.Draw(base.SpriteBatch);
            m_dialogControl.Draw(base.SpriteBatch);
            

            base.Draw();
        }

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) == true)
            {
                ScreenManager.TransitionTo("First");
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SECOND_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Second");

                ScreenManager.TransitionTo("TransitionScreen", parameters);                
            }
        }
    }
}
