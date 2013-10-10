using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using DeadLineGames.MIWIGD.Objects.SecondScreen;
using NamoCode.Game.Class.Design;
using DeadLineGames.MIWIGD.Objects;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Media;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Zelda Screen to get the first sword in Zelda. Take de object, and appear a string with detail the technology
    /// that use. (Classic sound of Zelda)
    /// 
    /// The "weapons" are XNA+MonoGame | Unity | Cocos.
    /// 
    /// Allow exit to room and show other string with others technologies.
    /// </summary>
    public class SecondScreen : Screen
    {

        private string FIRSTLINE = "IT'S DANGEROUS TO GO\n"
            + "  ALONE! TAKE THIS.";
        
        private const int TIMETOSAYGOODBYE = 100;
        private const int PARPADEO = 2;

        private Link link;
        private OldMan oldMan;
        private Sword sword;
        private List<AnimatedElement> fires;
        private ElementString type;

        private int timeTyping;
        private int charType;
        private int goodByeOldMan;

        public SecondScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            
            oldMan = new OldMan(new Vector2(319, DesignOptions.Bounds.MinY + 145));
            sword = new Sword(new Vector2(328, oldMan.Posicion.Y + oldMan.Height + 20));
            fires = new List<AnimatedElement>();
            fires.Add(new AnimatedElement(base.Content.Load<Texture2D>("SecondScreen/Fire"),
                "Fire1",
                new Vector2(oldMan.Posicion.X - oldMan.Width - 60, oldMan.Posicion.Y),
                new FrameRateInfo(2, 0.30f)));
            fires.Add(new AnimatedElement(base.Content.Load<Texture2D>("SecondScreen/Fire"),
                "Fire2",
                new Vector2(oldMan.Posicion.X + oldMan.Width + 60, oldMan.Posicion.Y),
                new FrameRateInfo(2, 0.30f)));
            link = new Link(new Vector2(323, DesignOptions.Bounds.MaxY - 32), oldMan.Posicion.Y + oldMan.Height);

            type = new ElementString(base.Content.Load<SpriteFont>("SecondScreen/Arcade30"), 
                "TextTyping",
                "",
                new Vector2(DesignOptions.Bounds.MinX + 120, DesignOptions.Bounds.MinY + 80));
            type.Font.Spacing = 3.5f;
            charType = 0;
            timeTyping = 0;
            goodByeOldMan = 1;

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("SecondScreen/Typing"), "Typing");
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("SecondScreen/ItemObtained"), "Obtained");
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("SecondScreen/ItemReceived"), "Received");

            base.Initialize();
        }

        protected void GoMenu()
        {
            ScreenManager.TransitionTo("Menu");
        }

        public override void Update(TimeSpan elapsed)
        {
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.Back))
                this.GoMenu();

            if (charType < FIRSTLINE.Length)
            {
                if (timeTyping % 4 == 0)
                {
                    type.LabelContent += FIRSTLINE[charType];
                    Player.Instance.Play("Typing", 0.70f);
                    charType++;
                }
                timeTyping++;
            }
            else
            {
                link.Update(elapsed);

                if (link.HaveSword)
                {
                    if (goodByeOldMan % TIMETOSAYGOODBYE == 0)
                        oldMan.Dispose();
                    else
                    {
                        if (goodByeOldMan % PARPADEO == 0)
                            oldMan.Visible = !oldMan.Visible;
                        goodByeOldMan++;
                    }
                }

                fires.ForEach(x => x.Update(elapsed));

                checkCollisions();
            }
            //base.Update(elapsed);
        }

        public override void Draw()
        {
            Map.Instance.Draw(base.SpriteBatch);
            type.Draw(base.SpriteBatch);
            oldMan.Draw(base.SpriteBatch);
            sword.Draw(base.SpriteBatch);
            fires.ForEach(x => x.Draw(base.SpriteBatch));
            link.Draw(base.SpriteBatch);
            base.Draw();
        }

        public void checkCollisions()
        {

            if (link.HaveColision(sword))
            {
                link.catchSword();
                sword.SetPosicion(link.Posicion.X - 3, link.Posicion.Y - sword.Height);
                oldMan.Opacity = 0.85f;
                type.Visible = false;
                Player.Instance.Play("Obtained", 0.60f);
                Player.Instance.Play("Received", 0.60f);
            }
            else
            {
                List<Direction> collisions = new List<Direction>();
                bool colExists = false;
                foreach (Wall w in Map.Instance)
                {
                    if (w.HaveColision(link))
                    {
                        foreach (Direction d in link.blockedDir)
                        {
                            if (d == link.dir)
                            {
                                colExists = true;
                                break;
                            }
                        }
                        if (!colExists)
                            collisions.Add(link.dir);
                    }
                }

                if (!colExists)
                {
                    if (collisions.Count > 0)
                        link.blockedDir = collisions;
                    else
                        link.blockedDir.Clear();
                }
            }
        }

    }
}
