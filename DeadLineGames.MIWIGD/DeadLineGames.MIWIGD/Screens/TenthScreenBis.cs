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
using DeadLineGames.MIWIGD.Objects.TenthScreen;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Marketing time!.
    /// 
    /// Only animation of Jake and Finn. Search font to adventure time to create a Marketing Time.
    /// </summary>
    public class TenthScreenBis : Screen
    {

        private Dancers dancers;
        private Title title;

        public TenthScreenBis(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            BasicTextures.FreeMemory();

            BasicTextures.CargarTextura("TenthScreen/dancing", "dancers");
            dancers = new Dancers(new Vector2(DesignOptions.Bounds.MinX,
                    DesignOptions.Bounds.MinY + 200 - (BasicTextures.GetTexture("dancers").Height / 16)));

            BasicTextures.CargarTextura("TenthScreen/MTitle", "MTitle");
            float CenterX = DesignOptions.Bounds.MaxX - BasicTextures.GetTexture("MTitle").Width - 10;
            float CenterY = DesignOptions.Bounds.MinY + 10;
            title = new Title(new Vector2(CenterX, CenterY));

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<Song>("TenthScreen/theme2"), "GangnamStyle");
            Player.Instance.Play("GangnamStyle", 0.70f);
            Player.Instance.RepeatMusic = true;

            base.Initialize();
        }


        protected override void GoBack()
        {
            ScreenManager.TransitionTo("Menu");
        }

        public override void Update(TimeSpan elapsed)
        {
            dancers.Update(elapsed);
            title.Update(elapsed);
            base.Update(elapsed);
        }

        public override void Draw()
        {
            base.SpriteBatch.GraphicsDevice.Clear(Color.White);
            dancers.Draw(base.SpriteBatch);
            title.Draw(base.SpriteBatch);
            base.Draw();
        }
    }
}
