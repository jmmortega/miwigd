using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NamoCode.Game.Class.Design.BackGrounds;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Screens;
using DeadLineGames.MIWIGD.Screens;

namespace DeadLineGames.MIWIGD
{
    /// <summary>
    /// Tipo principal del juego
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private BackGround m_Fondo;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            //graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;

            DesignOptions.Bounds = new Bounds(50, 620, 35, 435, 50, 620, 35, 435);

            BasicTextures.GraphicsManager = graphics;
            BasicTextures.ContentManager = Content;            
        }

        /// <summary>
        /// Permite que el juego realice la inicialización que necesite para empezar a ejecutarse.
        /// Aquí es donde puede solicitar cualquier servicio que se requiera y cargar todo tipo de contenido
        /// no relacionado con los gráficos. Si se llama a base.Initialize, todos los componentes se enumerarán
        /// e inicializarán.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: agregue aquí su lógica de inicialización
            m_Fondo = new BackGround(
                   base.Content.Load<Texture2D>("Common/tv"),
                   DesignOptions.Bounds);

            String g = Strings.SecondScreenDangerous;

            base.Initialize();

            ScreenManager.TransitionTo("Menu");
        }

        /// <summary>
        /// LoadContent se llama una vez por juego y permite cargar
        /// todo el contenido.
        /// </summary>
        protected override void LoadContent()
        {
            // Crea un SpriteBatch nuevo para dibujar texturas.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content para cargar aquí el contenido del juego
            ScreenManager.AddScreen("Menu", new MenuScreen(this));
            ScreenManager.AddScreen("First", new FirstScreen(this));
            ScreenManager.AddScreen("Second", new SecondScreen(this));
            ScreenManager.AddScreen("Seventh", new SeventhScreen(this));
        }

        /// <summary>
        /// UnloadContent se llama una vez por juego y permite descargar
        /// todo el contenido.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: descargue aquí todo el contenido que no pertenezca a ContentManager
        }

        /// <summary>
        /// Permite al juego ejecutar lógica para, por ejemplo, actualizar el mundo,
        /// buscar colisiones, recopilar entradas y reproducir audio.
        /// </summary>
        /// <param name="gameTime">Proporciona una instantánea de los valores de tiempo.</param>
        protected override void Update(GameTime gameTime)
        {
            // Permite salir del juego
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: agregue aquí su lógica de actualización
            ScreenManager.CurrentScreen.Update(gameTime.ElapsedGameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Se llama cuando el juego debe realizar dibujos por sí mismo.
        /// </summary>
        /// <param name="gameTime">Proporciona una instantánea de los valores de tiempo.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: agregue aquí el código de dibujo
            ScreenManager.CurrentScreen.Draw();
            m_Fondo.Draw(this.spriteBatch);

            base.Draw(gameTime);
        }
    }
}
