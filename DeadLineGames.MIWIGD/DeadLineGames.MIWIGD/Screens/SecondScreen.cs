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

        private Link link;
        private OldMan oldMan;
        private Sword sword;
        private List<AnimatedElement> fires;

        public SecondScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            link = new Link(new Vector2(323, DesignOptions.Bounds.MaxY - 32));
            oldMan = new OldMan(new Vector2(319, DesignOptions.Bounds.MinY + 140));
            sword = new Sword(new Vector2(328, oldMan.Posicion.Y + oldMan.Height + 20));
            fires = new List<AnimatedElement>();
            fires.Add(new AnimatedElement(base.Content.Load<Texture2D>("SecondScreen/Fire"),
                "Fire1",
                new Vector2(oldMan.Posicion.X - oldMan.Width - 50, oldMan.Posicion.Y),
                new FrameRateInfo(2, 0.30f)));
            fires.Add(new AnimatedElement(base.Content.Load<Texture2D>("SecondScreen/Fire"),
                "Fire2",
                new Vector2(oldMan.Posicion.X + oldMan.Width + 50, oldMan.Posicion.Y),
                new FrameRateInfo(2, 0.30f)));
            base.Initialize();
        }

        protected override void GoBack()
        {
            ScreenManager.TransitionTo("Menu");
        }

        public override void Update(TimeSpan elapsed)
        {
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.Back))
                this.GoBack();

            link.Update(elapsed);

            fires.ForEach(x => x.Update(elapsed));

            checkCollisions();
            //base.Update(elapsed);
        }

        public override void Draw()
        {
            Map.Instance.Draw(base.SpriteBatch);
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
                sword.SetPosicion(link.Posicion.X - 5, link.Posicion.Y - sword.Height);
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
