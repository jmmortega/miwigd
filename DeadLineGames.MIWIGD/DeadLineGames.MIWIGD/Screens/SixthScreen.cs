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
        Pumba pumba;

        public SixthScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            BasicTextures.FreeMemory();

            fondo = new AnimatedElement(base.Content.Load<Texture2D>("SixthScreen/Background"),
                "fondo",
                new Vector2(DesignOptions.Bounds.MinX, DesignOptions.Bounds.MinY),
                new FrameRateInfo());

            BasicTextures.CargarTextura("SixthScreen/Pumba/idle", "idle");
            BasicTextures.CargarTextura("SixthScreen/Pumba/eating", "eating");
            BasicTextures.CargarTextura("SixthScreen/Pumba/startrun", "startrun");
            BasicTextures.CargarTextura("SixthScreen/Pumba/run", "run");
            BasicTextures.CargarTextura("SixthScreen/Pumba/turn", "turn");
            BasicTextures.CargarTextura("SixthScreen/Pumba/burp", "burp");
            pumba = new Pumba(new Vector2(285 - (BasicTextures.GetTexture("idle").Width / 4),
                DesignOptions.Bounds.MaxY - 80));

            base.Initialize();
        }

        public override void Update(TimeSpan elapsed)
        {
            pumba.Update(elapsed);
            base.Update(elapsed);
        }

        public override void Draw()
        {
            fondo.Draw(base.SpriteBatch);
            pumba.Draw(base.SpriteBatch);
            base.Draw();
        }

    }
}
