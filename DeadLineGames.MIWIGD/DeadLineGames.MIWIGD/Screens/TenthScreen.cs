using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Media;
using Microsoft.Xna.Framework.Media;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Marketing time!.
    /// 
    /// Only animation of Jake and Finn. Search font to adventure time to create a Marketing Time.
    /// </summary>
    public class TenthScreen : Screen
    {

        private AnimatedElement back;

        public TenthScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {

            back = new AnimatedElement(base.Content.Load<Texture2D>("TenthScreen/back"),
                "Background",
                new Vector2(DesignOptions.Bounds.MinX, DesignOptions.Bounds.MinY),
                new FrameRateInfo(1, 0.30f, 5, true));

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<Song>("TenthScreen/theme"), "Will");
            Player.Instance.Play("Will");
            Player.Instance.RepeatMusic = true;

            base.Initialize();
        }


        protected override void GoBack()
        {
            ScreenManager.TransitionTo("Menu");
        }

        public override void Update(TimeSpan elapsed)
        {
            back.Update(elapsed);
            base.Update(elapsed);
        }

        public override void Draw()
        {
            back.Draw(base.SpriteBatch);
            base.Draw();
        }
    }
}
