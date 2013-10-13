using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using DeadLineGames.MIWIGD.Objects.SeventhScreen;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Super Mario Bros 1-1 background three blocks ? when jump show the model business
    /// </summary>
    public class SeventhScreen : Screen
    {

        private Mario mario;

        public SeventhScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            BasicTextures.FreeMemory();

            BasicTextures.CargarTextura("SeventhScreen/Mario", "Mario");
            mario = new Mario(new Vector2(DesignOptions.Bounds.MinX - BasicTextures.GetTexture("Mario").Width,
                SeventhMap.Instance.boundsFloor - (BasicTextures.GetTexture("Mario").Height / 4)));

            base.Initialize();
        }

        public override void Update(TimeSpan elapsed)
        {
            mario.Update(elapsed);
            base.Update(elapsed);
        }

        public override void Draw()
        {
            mario.Draw(base.SpriteBatch);
            SeventhMap.Instance.Draw(base.SpriteBatch);
            base.Draw();
        }

    }
}
