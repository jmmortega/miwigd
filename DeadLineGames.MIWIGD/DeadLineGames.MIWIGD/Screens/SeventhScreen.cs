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

        public SeventhScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            BasicTextures.FreeMemory();

            fondo = new AnimatedElement(base.Content.Load<Texture2D>("SeventhScreen/Sky"),
                "fondo",
                new Vector2(DesignOptions.Bounds.MinX, DesignOptions.Bounds.MinY),
                new FrameRateInfo());

            BasicTextures.CargarTextura("SeventhScreen/Mario", "Mario");
            mario = new Mario(new Vector2(DesignOptions.Bounds.MinX - BasicTextures.GetTexture("Mario").Width,
                SeventhMap.Instance.boundsFloor - (BasicTextures.GetTexture("Mario").Height / 4)));

            BasicTextures.CargarTextura("SeventhScreen/block", "Block");
            bloques = new Blocks();

            Player.Instance.Sounds.Clear();
            Player.Instance.Sounds.Add(base.Content.Load<Song>("SeventhScreen/Theme"), "Mario");
            Player.Instance.Sounds.Add(base.Content.Load<SoundEffect>("SeventhScreen/Jump"), "Jump");
            Player.Instance.RepeatMusic = true;
            Player.Instance.Play("Mario");

            base.Initialize();
        }

        public override void Update(TimeSpan elapsed)
        {
            mario.Update(elapsed);
            bloques.updateBlocks(elapsed, mario);
            base.Update(elapsed);
        }

        public override void Draw()
        {
            fondo.Draw(base.SpriteBatch);
            mario.Draw(base.SpriteBatch);
            bloques.Draw(base.SpriteBatch);
            SeventhMap.Instance.Draw(base.SpriteBatch);
            base.Draw();
        }

    }
}
