using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using DeadLineGames.MIWIGD.Objects.SeventhScreen;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Class.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using DeadLineGames.MIWIGD.Objects.Common;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Super Mario Bros 1-1 background three blocks ? when jump show the model business
    /// </summary>
    public class SeventhScreen : Screen
    {

        private AnimatedElement fondo;
        private Mario mario;
        private Blocks bloques;
        private ElementString m_label;

        public SeventhScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            //BasicTextures.FreeMemory();

            fondo = new AnimatedElement(base.Content.Load<Texture2D>("SeventhScreen/Sky"),
                "fondo",
                new Vector2(DesignOptions.Bounds.MinX, DesignOptions.Bounds.MinY),
                new FrameRateInfo());

            BasicTextures.CargarTextura("SeventhScreen/Mario", "Mario");
            mario = new Mario(new Vector2(DesignOptions.Bounds.MinX - BasicTextures.GetTexture("Mario").Width,
                SeventhMap.Instance.boundsFloor - (BasicTextures.GetTexture("Mario").Height / 4)));

            BasicTextures.CargarTextura("SeventhScreen/Coin", "Coin");
            BasicTextures.CargarTextura("SeventhScreen/block", "Block");
            bloques = new Blocks();
            bloques.OnCoinUp += new Blocks.HandleCoinUp(bloques_OnCoinUp);

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<Song>("SeventhScreen/Theme"), "Mario");
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("SeventhScreen/Jump"), "Jump");
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("SeventhScreen/Moneda"), "Coin");
            Player.Instance.RepeatMusic = false;
            Player.Instance.Play("Mario");

            mario.OnMarioIsOut += new Mario.HandleMarioIsOut(MarioIsOut);

            m_label = new ElementString(base.Content.Load<SpriteFont>("SeventhScreen/smw18"), "LabelExplaining");
            m_label.Posicion = new Vector2(285, 220);
            
            base.Initialize();
        }

        private void bloques_OnCoinUp(object sender, EventArgs e)
        {
            Coin coin = (Coin)sender;

            if (coin.Name == "Coin_Block_0")
            {
                m_label.LabelContent = Strings.FIRSTCOIN;
            }
            else if (coin.Name == "Coin_Block_1")
            {
                m_label.LabelContent = Strings.SECONDCOIN;
            }
            else if (coin.Name == "Coin_Block_2")
            {
                m_label.LabelContent = Strings.THIRDCOIN;
            }

        }

        private void MarioIsOut(object sender, EventArgs e)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(Consts.PARAMETERTITLE, Strings.EIGHT_TITLE);
            parameters.Add(Consts.PARAMETERSCREEN, "Eight");

            ScreenManager.TransitionTo("TransitionScreen", parameters);
            Player.Instance.Stop();
        }

        public override void Update(TimeSpan elapsed)
        {
            mario.Update(elapsed);
            bloques.updateBlocks(elapsed, mario);
            ChangeSlide();
            base.Update(elapsed);            
        }

        public override void Draw()
        {
            fondo.Draw(base.SpriteBatch);
            mario.Draw(base.SpriteBatch);
            bloques.Draw(base.SpriteBatch);
            SeventhMap.Instance.Draw(base.SpriteBatch);
            m_label.Draw(base.SpriteBatch);
            base.Draw();
        }

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SIXTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Sixth");

                Player.Instance.Stop();
                ScreenManager.TransitionTo("TransitionScreen", parameters);                
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.EIGHT_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Eight");

                Player.Instance.Stop();
                ScreenManager.TransitionTo("TransitionScreen", parameters);                
            }
        }

    }
}
