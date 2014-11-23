using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Design.Hud;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Objects;
using DeadLinesGames.MIWIGD.Objects.FirstScreen;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using DeadLineGames.MIWIGD.Objects.Common;
using NamoCode.Game.Class.Media;
using Microsoft.Xna.Framework.Media;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Ash Ketchum's house, monochrome. Ash Character and Mom Character, spoke with mother. (PopUpSpeaking required)
    /// Appear Oaklindo and speak to, in the classic screen of pokemon. (Required draw of oak with beard)
    /// </summary>
    public class FirstScreen : Screen
    {
        private DialogControl m_dialogControl;
        private Element m_pokemonHouseBackground;
        private AObjects m_furnitures;
        private AObjects m_characters;

        private Ash m_ash;
        private OakLindo m_oak;

        private bool m_readyToTalk;
        private bool m_isChangingScreen = false;

        private EnumMovement m_lookDirection;
        public EnumMovement LookDirection
        {
            get { return m_lookDirection; }
            set 
            {
                if (m_lookDirection == EnumMovement.None)
                {
                    m_lookDirection = value;
                }
            }
        }
        
        public FirstScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {                                 
            m_dialogControl = new DialogControl(
                "SampleDialog", new Vector2(90, 250), string.Empty, base.Content.Load<Texture2D>("FirstScreen/PokemonFrame"));
            
            m_pokemonHouseBackground = new Element(
                base.Content.Load<Texture2D>("FirstScreen/PokemonBackground"), "Background");
                                                                
            m_dialogControl.TimeToUpdateText = new TimeSpan(0, 0, 0, 0, 10);
            m_dialogControl.WaitToNextLine = true;
            m_dialogControl.Color = Color.Black;
            m_dialogControl.TextController.Color = Color.Black;
            m_dialogControl.Visible = false;            

            m_pokemonHouseBackground.Posicion = new Vector2(55, 40);

            CreateFurnitures();
            CreateCharacters();

            m_oak.OnFinishWalking += OakFinishWalking;

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<Song>("FirstScreen/PokemonTheme"), "PokemonTheme");
            Player.Instance.Play("PokemonTheme");
            
            base.Initialize();
        }

        private void OakFinishWalking(object sender, EventArgs e)
        {
            ChangeScreen();
        }

        private void CreateFurnitures()
        {
            m_furnitures = new AObjects();

            m_furnitures.Add(new Furniture(base.Content.Load<Texture2D>("FirstScreen/FurniturePokemon"), "Library", 
                new Vector2(60, 40)));
            m_furnitures.Add(new Furniture(base.Content.Load<Texture2D>("FirstScreen/TvPokemon"), "TV",
                new Vector2(263,83)));
            m_furnitures.Add(new Furniture(base.Content.Load<Texture2D>("FirstScreen/TablePokemon"), "Table",
                new Vector2(266 , 237)));
            m_furnitures.Add(new Furniture(base.Content.Load<Texture2D>("FirstScreen/CarpetPokemon"), "Carpet",
                new Vector2(194, 390)));

            //Yes I add to mom a furnitures lol!
            m_furnitures.Add(new Mom(base.Content.Load<Texture2D>("FirstScreen/AshMom"), "Mom",
                new Vector2(410, 210) , new FrameRateInfo(3,0) ,DesignOptions.Bounds));
        }

        private void CreateCharacters()
        {
            m_characters = new AObjects();

            m_characters.Add(new Ash(base.Content.Load<Texture2D>("FirstScreen/Ash"), "Ash",
                new Vector2(480, 75), new FrameRateInfo(6,0), DesignOptions.Bounds));


            m_characters.Add(new OakLindo(base.Content.Load<Texture2D>("FirstScreen/ProfOak"), "Oak",
                new Vector2(200 , 560), new FrameRateInfo(6,0) , DesignOptions.Bounds));
            
            m_ash = (Ash)m_characters.GetElement("Ash");
            m_oak = (OakLindo)m_characters.GetElement("Oak");
        }

        #region Update
        
        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);
            base.Input = InputState.GetInputState();
            
            m_dialogControl.Update(elapsed);

            if (m_dialogControl.Visible == true)
            {
                if ((base.Input.KeyboardState.IsKeyDown(Keys.Space) == true) ||
                    (base.Input.GamepadOne.IsButtonDown(Buttons.A) == true))
                {
                    if (m_dialogControl.isEndOfPharagraph == true)
                    {
                        m_dialogControl.NextLine();
                    }
                }
            }
            else
            {
                if (m_trackingAsh == false)
                {
                    CheckColission();
                    m_characters.Update(elapsed);
                }
            }

            if (((base.Input.KeyboardState.IsKeyDown(Keys.Space) == true) ||
                (base.Input.GamepadOne.IsButtonDown(Buttons.A) == true)) && m_readyToTalk == true)
            {
                if(m_dialogControl.Visible == false)
                {
                    var mom = (Mom)
                        m_furnitures.GetElement("Mom");

                    mom.TakeAnimation(m_lookDirection);
                    if (m_secondTime == false)
                    {
                        LoadFirstConversation();
                    }
                }                                
            }

            if (m_trackingAsh == true)
            {
                m_oak.PosicionAsh = m_ash.Posicion;
                m_oak.Update(elapsed);
                m_oak.DoMovement();
            }                        
        }

        private void CheckColission()
        {
            foreach (AObject furniture in m_furnitures)
            {
                if (m_ash.HaveColision(furniture) == true)
                {
                    m_ash.IsBounds = true;

                    if (furniture is Mom)
                    {                        
                        m_readyToTalk = true;
                        this.LookDirection = m_ash.Movement;
                    }
                }

                if (m_oak.HaveColision(furniture) == true)
                {
                    if (furniture is Mom)
                    {
                        ChangeScreen();
                    }
                }
            }

            ChangeSlide();
        }
        
        private void ChangeScreen()
        {
            if (m_isChangingScreen == false)
            {
                m_isChangingScreen = true;
                ScreenManager.TransitionTo("FirstScreenDetail", Color.Gray);
            }            
        }

        #endregion

        #region Draw

        public override void Draw()
        {
            base.Draw();
            
            m_pokemonHouseBackground.Draw(base.SpriteBatch);
            m_furnitures.Draw(base.SpriteBatch);
            m_characters.Draw(base.SpriteBatch);
            m_dialogControl.Draw(base.SpriteBatch);
        }

        #endregion

        #region Conversation

        private bool m_secondTime = false;
        private bool m_trackingAsh = false;

        private void LoadFirstConversation()
        {                        
            m_dialogControl.Visible = true;
            m_dialogControl.LoadText(Strings.DEVELOPERDIALOG);

            m_dialogControl.OnEndOfDialog += new DialogControl.EndOfDialog(DialogControlOnEndOfDialog);
        }

        private void DialogControlOnEndOfDialog(object sender, NamoCode.Game.Class.EventArguments.EndOfDialogEventArgs e)
        {
            if (m_secondTime == false)
            {                
                m_dialogControl.LoadText(Strings.MOMDIALOG);
                m_secondTime = true;
            }
            else
            {
                m_dialogControl.Visible = false;
                m_trackingAsh = true;
            }
        }

        
        
        #endregion

        #region ChangeScreen

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) || base.Input.KeyboardState.IsKeyDown(Keys.Q))
            {
                ScreenManager.TransitionTo("Menu");
                Player.Instance.Stop();
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) || base.Input.KeyboardState.IsKeyDown(Keys.W))
            {
                ScreenManager.TransitionTo("FirstScreenDetail");
                Player.Instance.Stop();
            }
        }

        #endregion


    }
}
