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
        private Pumba pumba;
        private Timon timon;

        private bool EndScreen;

        private int Score;

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

            cargarTimon();
            cargarPumba();

            BasicTextures.CargarTextura("SixthScreen/Bugs/Bugs", "Bugs");

            Score = 0;
            EndScreen = false;

            base.Initialize();
        }

        public void cargarTimon()
        {
            BasicTextures.CargarTextura("SixthScreen/Timon/idle", "Tidle");
            BasicTextures.CargarTextura("SixthScreen/Timon/run", "Trun");
            BasicTextures.CargarTextura("SixthScreen/Timon/turn", "Tturn");
            timon = new Timon(new Vector2(285 - (BasicTextures.GetTexture("Tidle").Width),
                DesignOptions.Bounds.MinY - BasicTextures.GetTexture("Tidle").Height + 161));
        }

        public void cargarPumba()
        {
            BasicTextures.CargarTextura("SixthScreen/Pumba/idle", "idle");
            BasicTextures.CargarTextura("SixthScreen/Pumba/eating", "eating");
            BasicTextures.CargarTextura("SixthScreen/Pumba/startrun", "startrun");
            BasicTextures.CargarTextura("SixthScreen/Pumba/run", "run");
            BasicTextures.CargarTextura("SixthScreen/Pumba/turn", "turn");
            BasicTextures.CargarTextura("SixthScreen/Pumba/burp", "burp");
            pumba = new Pumba(new Vector2(285 - (BasicTextures.GetTexture("idle").Width / 4),
                DesignOptions.Bounds.MaxY - 80));
        }

        public override void Update(TimeSpan elapsed)
        {
            if (!EndScreen)
            {
                timon.Update(elapsed);
                pumba.Update(elapsed);
                Bugs.Instance.updateBugs(elapsed);
                checkCollisions();
            }
            base.Update(elapsed);
        }

        public override void Draw()
        {
            fondo.Draw(base.SpriteBatch);
            timon.Draw(base.SpriteBatch);
            pumba.Draw(base.SpriteBatch);
            Bugs.Instance.Draw(base.SpriteBatch);
            base.Draw();
        }

        private void checkCollisions()
        {

            foreach (Bug b in Bugs.Instance)
            {
                if (pumba.HaveColision(b))
                {
                    if(pumba.estado == Objects.CharsState.Idle)
                        pumba.isEating = true;
                    if (b.Posicion.Y >= pumba.Posicion.Y - 5
                        && b.Posicion.Y <= pumba.Posicion.Y + 5)
                    {
                        b.isEating = true;
                        Score++;
                        if (b.actuaFrame == b.framesCount - 1)
                            EndScreen = true;
                    }
                }
            }

        }

    }
}
