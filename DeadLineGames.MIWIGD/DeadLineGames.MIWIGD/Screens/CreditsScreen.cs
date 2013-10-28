using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;


namespace DeadLineGames.MIWIGD.Screens
{
    public class CreditsScreen : Screen
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Video video;
        VideoPlayer player;
        Texture2D videoTexture;

        public CreditsScreen(Game game)
            :base(game)
        { }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(base.Game.GraphicsDevice);

            spriteBatch = new SpriteBatch(base.Game.GraphicsDevice);
            video = Content.Load<Video>("Credits/video");
            player = new VideoPlayer();

            

            base.LoadContent();            
        }

        public override void Update(TimeSpan elapsed)
        {
            // Permite salir del juego
            Game1.TvOn = false;

            base.Input = InputState.GetInputState();

            if(base.Input.GamepadOne.IsButtonDown(Buttons.Start) == true)
            {
                Game.Exit();
            }
            // TODO: agregue aquí su lógica de actualización
            if (player.State == MediaState.Stopped)
            {
                player.IsLooped = false;
                player.Play(video);
            }

            base.Update(elapsed);
        }

        public override void Draw()
        {
            base.Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: agregue aquí el código de dibujo
            // Only call GetTexture if a video is playing or paused
            if (player.State != MediaState.Stopped)
                videoTexture = player.GetTexture();

            // Drawing to the rectangle will stretch the 
            // video to fill the screen
            Rectangle screen = new Rectangle(base.Game.GraphicsDevice.Viewport.X,
                base.Game.GraphicsDevice.Viewport.Y,
                base.Game.GraphicsDevice.Viewport.Width,
                base.Game.GraphicsDevice.Viewport.Height);

            // Draw the video, if we have a texture to draw.
            if (videoTexture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoTexture, screen, Color.White);
                spriteBatch.End();
            }
            
            base.Draw();
        }
        
    }
}
