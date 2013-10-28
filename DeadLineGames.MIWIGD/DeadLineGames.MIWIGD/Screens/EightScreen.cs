using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Class.Media;
using DeadLineGames.MIWIGD.Objects.EighthScreen;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using DeadLineGames.MIWIGD.Objects.Common;
namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Monkey Island Screen. Insult Sword Fighting
    /// </summary>
    public class EightScreen : Screen
    {

        private AnimatedElement back;
        private ElementString text;
        private DialogGenerator dg;

        private Characters c;

        private Talk talking;
        private Talk lastTalk;

        private bool waitReply;
        
        public static Vector2 position;

        public EightScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {

            position = new Vector2(DesignOptions.Bounds.MinX + 160, DesignOptions.Bounds.MinY + 80);
                        
            //BasicTextures.FreeMemory();
                            
            BasicTextures.CargarTextura("EighthScreen/MButtons", "buttons");
            BasicTextures.CargarTextura("EighthScreen/start", "start");
            BasicTextures.CargarTextura("EighthScreen/idle", "idleeigth");
            BasicTextures.CargarTextura("EighthScreen/Guybrush/guybrush-speak", "guybrush-speak");
            BasicTextures.CargarTextura("EighthScreen/Pirate/chema-speak", "chema-speak");
            BasicTextures.CargarTextura("EighthScreen/Guybrush/guybrush-attack", "guybrush-attack");
            BasicTextures.CargarTextura("EighthScreen/Pirate/chema-attack", "chema-attack");
            BasicTextures.CargarTextura("EighthScreen/Guybrush/guybrush-win", "guybrush-win");
            BasicTextures.CargarTextura("EighthScreen/Pirate/chema-win", "chema-win");

            back = new AnimatedElement(base.Content.Load<Texture2D>("EighthScreen/background"),
                "back",
                new Vector2(DesignOptions.Bounds.MinX, DesignOptions.Bounds.MinY),
                new FrameRateInfo(1, 0, 1, false));
            
            c = new Characters(position);

            text = new ElementString(base.Content.Load<SpriteFont>("EighthScreen/MonkeyFont"),
                "text",
                Insults.FirstReply,
                new Vector2(DesignOptions.Bounds.MinX + 120,
                    back.Posicion.Y + back.Height + 10));
            text.Color = Color.DarkGreen;

            dg = new DialogGenerator(new Vector2(DesignOptions.Bounds.MinX + 40,
                    back.Posicion.Y + back.Height + 10));

            talking = Talk.None;
            lastTalk = Talk.None;

            waitReply = false;

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("EighthScreen/swords"), "Swords");

            base.Initialize();
        }

        protected override void GoBack()
        {
            ScreenManager.TransitionTo("Menu");
        }

        public override void Update(TimeSpan elapsed)
        {

            c.Update(elapsed);

            if (c.hasStarted && !c.isLoosed)
            {
                if (dg.firstTalking)
                {
                    dg.talkPirate(c, Talk.Insult);
                    if (!c.pIsTalking)
                    {
                        dg.Clear();
                        dg.updateSentences(c, Talk.Reply);
                    }
                    talking = Talk.Reply;
                }
                else
                {
                    if ((!c.gIsTalking && !c.pIsTalking)
                        && !c.isFighting && !c.isLoosing)
                    {
                        if (c.gEndTalking)
                        {
                            if (lastTalk == Talk.Insult)
                            {
                                if (waitReply)
                                {
                                    dg.Clear();
                                    c.pEndTalking = false;
                                    dg.talkPirate(c, lastTalk);
                                    waitReply = false;
                                }
                                if (c.pEndTalking)
                                {
                                    dg.checkStake(c, lastTalk);
                                    lastTalk = Talk.None;
                                }
                            }
                            else if (lastTalk == Talk.Reply)
                            {
                                dg.checkStake(c, lastTalk);
                                lastTalk = Talk.None;
                            }
                            else
                            {
                                if (c.pEndTalking)
                                {
                                    if (!dg.opsLoaded)
                                        dg.updateSentences(c, talking);

                                    if (talking == Talk.None)
                                    {
                                        dg.Clear();
                                        if (!dg.loosedStake)
                                        {
                                            talking = Talk.Insult;
                                        }
                                        else
                                        {
                                            talking = Talk.Reply;
                                            c.pEndTalking = false;
                                            dg.talkPirate(c, talking);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                int sentence = -1;
                if (dg.opsLoaded)
                {
                    if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.A)
                        || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.S))
                        sentence = 0;
                    if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.B)
                        || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.D))
                        sentence = 1;
                    if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.X)
                        || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.A))
                        sentence = 2;
                    if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.Y)
                        || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.W))
                        sentence = 3;
                    
                }

                if (talking != Talk.None && sentence != -1)
                {
                    if (dg.firstTalking && sentence == 0)
                    {
                        dg.reply = sentence;
                        dg.talkGuybrush(c, talking);
                        dg.checkStake(c, talking);
                    }
                    else
                    {
                        if ((!c.gIsTalking && !c.pIsTalking)
                            && !c.isFighting && !c.isLoosing)
                        {
                            dg.Clear();
                            if (talking == Talk.Insult)
                            {
                                dg.insult = sentence;
                                c.gEndTalking = false;
                                dg.talkGuybrush(c, talking);
                                lastTalk = Talk.Insult;
                                waitReply = true;
                            }
                            else if (talking == Talk.Reply)
                            {
                                dg.reply = sentence;
                                c.gEndTalking = false;
                                dg.talkGuybrush(c, talking);
                                lastTalk = Talk.Reply;
                            }
                        }
                    }
                    talking = Talk.None;
                    dg.opsLoaded = false;
                }
            }

            ChangeSlide();
            base.Update(elapsed);
        }

        public override void Draw()
        {
            back.Draw(base.SpriteBatch);
            //gb.Draw(base.SpriteBatch);
            dg.Draw(base.SpriteBatch);
            c.Draw(base.SpriteBatch);
            base.Draw();
        }

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.NINE_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Nineth");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SEVENTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Seventh");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
        }

    }
}
