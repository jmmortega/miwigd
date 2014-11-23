using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design.BackGrounds;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using StarPaper.Class.Events.Director;
using StarPaper;
using StarPaper.Class.Design;
using StarPaper.Class.Events.Director.StoryBoard;
using StarPaper.Class.Objects.Buenos;
using StarPaper.Class.Objects;
using StarPaper.Utils;
using DeadLineGames.MIWIGD.Objects.Common;
using StarPaper.Class.Objects.Enemies.ListaEnemigos;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Well and your artist...
    /// 
    /// A ship game, that use sprite to use in StarPaper. (Are a paint assets...)
    /// </summary>
    public class ThirdScreen : Screen
    {
                
         #region Consts

        private const float c_MegaAtackDelay = 5f;

        #endregion

        #region Fields

        private DirectorStarPaper m_director = 
                      new DirectorStarPaper(StarPaperOptions.Dificultad , StarPaperOptions.Extend);

        /// <summary>
        /// Se sobrecarga bounds, por si hay que realizar cambios de pantalla activa.
        /// Restar tamaño por la publi, hud del juego, etc...
        /// </summary>
        protected override Bounds Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
            }
        }

        private BackGround m_backGround = null;

        private GameHub m_gamehub = null;

        private AnimatedElement m_danger = null;

        private static bool m_IsNewGame = true;

        /// <summary>
        /// Determina si se va a cargar un nuevo juego o no.
        /// </summary>        
        public static bool IsNewGame
        {
            get { return ThirdScreen.m_IsNewGame; }
            set { ThirdScreen.m_IsNewGame = value; }
        }

        private static int m_level = 0;

        /// <summary>
        /// El nivel que vamos a cargar
        /// </summary>
        /// <remarks>
        /// El valor 0 esta reservado para el nivel de test.
        /// El valor -1 esta reservado para el nivel de horda.
        /// </remarks>
        public static int Level
        {
            get { return ThirdScreen.m_level; }
            set { ThirdScreen.m_level = value; }
        }

        private float m_megaAtackDelay;
        
        #endregion

        #region Windows Phone Fields

        #endregion

        #region Constructor

        public ThirdScreen(Game game)
            : base(game)            
        {
            //Agregamos los eventos director.
            m_director.OnStoryBoardEventBonus += NewBonus;
            m_director.OnStoryBoardEventEnemy += NewEnemy;
            m_director.OnStoryBoardEventOther += NewOther;
            m_director.OnScenarioLoad += ScenarioLoad;
            m_director.OnGameOver += GameOver;
            m_director.OnEndPhase += EndPhase;

            //Modificamos el valor de Bounds, para agregar los valores de Activo.
            base.Bounds = new Bounds(0, 480, 0, 800, 0, 480, 80, 720);

        }
        
        #endregion

        #region Initialize

        public override void Initialize()
        {            
            base.Initialize();

            //Inicializamos el bounds dentro del helper.
            EnemiesListHelper.Instance.Bounds = base.Bounds;

            m_megaAtackDelay = 0;
            m_gamehub = new GameHub("Hub", new Vector2(0, 0), new Vector2(0, 740), 60, base.Bounds.MaxX);

            if (m_IsNewGame == true)
            {
                NewGame();
            }
            else
            {
                LoadLevel();
            }                           
        }

        private void NewGame()
        {            
            m_director.NewGame();
                                                
            LoadLevel();
                        
        }

        private void LoadLevel()
        {            
            m_director.NewLevel();

            LoadProta();

            m_gamehub.Initialize(m_director.HubValues);

            m_director.LoadLevel("Content/ThirdScreen/Level" + m_level.ToString() + ".TXT");

            m_danger.Visible = false;

            m_backGround.Reset();

            Explosiones.Instance.Clear();
            
            m_director.DificultadJuego = StarPaperOptions.Dificultad;
        }

        #endregion

        #region MethodsEvents

        private void NewBonus(StoryBoardEventBonus bonus)
        {
            //Do nothing...
        }

        /// <summary>
        /// Incluye los nuevos enemigos.
        /// </summary>
        /// <param name="enemy"></param>
        private void NewEnemy(StoryBoardEventEnemy enemy)
        {
            if (enemy.IsFinalBoss == true)
            {
                m_danger.Visible = true;
            }
        }

        /// <summary>
        /// Como los eventos otros está pensando para cambios gráficos o situaciones especiales, tendremos que tratarlas
        /// cada una por cada cual...
        /// </summary>
        /// <param name="other"></param>
        private void NewOther(StoryBoardEventOther other)
        {
            //Do nothing...
        }

        /// <summary> 
        /// Carga el valor del escenario.
        /// </summary>
        /// <param name="scenarioName"></param>
        /// <remarks>
        /// Inicialmente dentro de este método no se ha contemplado la introducción de capas.
        /// Yo creo que la mejor opción es crear un diccionario de contenidos con el listado de capas y el nombre
        /// que se facilita sea el funcionamiento del mismo.
        /// </remarks>
        private void ScenarioLoad(string scenarioName)
        {
            if (scenarioName != string.Empty)
            {
                m_backGround = new BackGround
                    (Content.Load<Texture2D>("ThirdScreen/" + scenarioName), this.Bounds, 0, 10);                
            }
        }

        private void GameOver()
        {
            //Nothing...
            EndPhase();
        }

        private void EndPhase()
        {
            /*
            m_director.ProtaValues = (m_director.Objetos as InfoObjectsStarPaper).Protagonista.Values;

            LevelCompleteScreen.Level = m_level;

            ScreenManager.TransitionTo("LevelCompleteScreen");
             */

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(Consts.PARAMETERTITLE, Strings.FORTH_TITLE);
            parameters.Add(Consts.PARAMETERSCREEN, "Forth");

            ScreenManager.TransitionTo("TransitionScreen", parameters);

        }

        #endregion

        #region LoadContent

        public override void LoadContent()
        {            
            LoadTextures();
            
            base.LoadContent();
        }

        #region LoadTextures

        private void LoadTextures()
        {
            LoadTexturesEnemies();
            LoadTexturesShoots();
            LoadTexturesExplosions();
            LoadTexturesBonus();
            LoadTexturesOther();       
        }

        /// <remarks>
        /// Posibilidad de introducir un progressBar
        /// </remarks>
        private void LoadTexturesShoots()
        {
            ShootTextures.Instance.Add
                ("Disparo1", base.Content.Load<Texture2D>("ThirdScreen/Disparo1"));

            ShootTextures.Instance.Add
                ("GoodGuyShoot", base.Content.Load<Texture2D>("ThirdScreen/GoodGuyShoot"));

            ShootTextures.Instance.Add
                ("Disparo1Boss1", base.Content.Load<Texture2D>("ThirdScreen/Boss1Disparo1"));
                
            ShootTextures.Instance.Add
                ("Disparo2Boss1", base.Content.Load<Texture2D>("ThirdScreen/Boss1Disparo2"));

            ShootTextures.Instance.Add
                ("Disparo1Boss2", base.Content.Load<Texture2D>("ThirdScreen/Boss2Disparo1"));

            ShootTextures.Instance.Add
                ("Disparo1Boss3", base.Content.Load<Texture2D>("ThirdScreen/Boss3Disparo1"));
        }
        
        /// <remarks>
        /// Posibilidad de introducir un progressBar
        /// </remarks>
        private void LoadTexturesEnemies()
        {
            EnemiesTextures.Instance.Add
                ("TrianguloAmarillo", base.Content.Load<Texture2D>("ThirdScreen/TrianguloAmarillo"));

            EnemiesTextures.Instance.Add
                ("CirculoRojo", base.Content.Load<Texture2D>("ThirdScreen/CirculoRojo"));

            EnemiesTextures.Instance.Add
                ("RectanguloNaranja", base.Content.Load<Texture2D>("ThirdScreen/RectanguloNaranja"));

            EnemiesTextures.Instance.Add
                ("Boss1", base.Content.Load<Texture2D>("ThirdScreen/Boss1"));

            EnemiesTextures.Instance.Add
                ("TanqueChungo", base.Content.Load<Texture2D>("ThirdScreen/TanqueChungo"));

            EnemiesTextures.Instance.Add
                ("TanqueChungoM", base.Content.Load<Texture2D>("ThirdScreen/TanqueChungoM"));

            EnemiesTextures.Instance.Add
                ("TrianguloNaranja", base.Content.Load<Texture2D>("ThirdScreen/TrianguloNaranja"));

            EnemiesTextures.Instance.Add
                ("TrianguloVerde", base.Content.Load<Texture2D>("ThirdScreen/TrianguloVerde"));

            EnemiesTextures.Instance.Add
                ("TrianguloRosa", base.Content.Load<Texture2D>("ThirdScreen/TrianguloRosa"));

            EnemiesTextures.Instance.Add
                ("Boss2", base.Content.Load<Texture2D>("ThirdScreen/Boss2"));

            EnemiesTextures.Instance.Add
                ("Boss3", base.Content.Load<Texture2D>("ThirdScreen/Boss3"));

            EnemiesTextures.Instance.Add
                ("Boss3M", base.Content.Load<Texture2D>("ThirdScreen/Boss3M"));

            EnemiesTextures.Instance.Add
                ("Boss4", base.Content.Load<Texture2D>("ThirdScreen/Boss4"));
        }

        private void LoadProta()
        {
            //Texture2D textura = BasicTextures.CargarTextura("Sprites/Nave/Nave", "Prota");            

            Texture2D textura = BasicTextures.CargarTextura("ThirdScreen/Nave", "Prota");

            m_director.Objetos.Protas.Add
                (new Prota(textura, "Prota", new FrameRateInfo(1, 0), base.Bounds));
                
            (m_director.Objetos as InfoObjectsStarPaper).Protagonista.SetPosicion(230, 400);

            (m_director.Objetos as InfoObjectsStarPaper).Protagonista.Values = m_director.ProtaValues;
        }

        private void LoadTexturesExplosions()
        {
            Explosiones.Instance.Add(new Explosion(
                base.Content.Load<Texture2D>("ThirdScreen/Explosion1"),
                "Explosion1", new FrameRateInfo(15, 0.1f)));

            Explosiones.Instance.Add(new Explosion(
                base.Content.Load<Texture2D>("ThirdScreen/Explosion1"),
                "GigaExplosion", new FrameRateInfo(15, 0.1f), 10));
            
        }

        private void LoadTexturesBonus()
        {
            //Cargo las texturas de los bonus en el diccionario principal porque es una textura que se va a mantener durante todo el juego.

            BasicTextures.CargarTextura("ThirdScreen/BonusDamage", "BonusDamage");
            BasicTextures.CargarTextura("ThirdScreen/BonusRapid", "BonusRapid");
            BasicTextures.CargarTextura("ThirdScreen/BonusShot", "BonusShot");            
        }

        private void LoadTexturesOther()
        {

            ///Danger 
            BasicTextures.CargarTextura("ThirdScreen/Danger", "Danger");

            m_danger = new AnimatedElement(BasicTextures.GetTexture("Danger"), "Danger", new Vector2(50, 50),
                new FrameRateInfo(2, 1));

            m_danger.Visible = false;

            ///Life

            BasicTextures.CargarTextura("ThirdScreen/Life", "ProtaLife");
        }

        #endregion

        #region Private Methods LoadContent

        #endregion

        #endregion

        #region Update

        protected override void GoBack()
        {            
            base.Input = InputState.NullInput();
            base.Game.Exit();
        }
        
        public override void Update(TimeSpan elapsed)
        {
            if (base.Message == null)
            {
                CheckInput(elapsed);

                m_director.Update(elapsed);
                m_director.DetermineColissions();

                Explosiones.Instance.Update(elapsed);

                DoBackGround();

                m_gamehub.Update(elapsed, m_director.HubValues);

                m_danger.Update(elapsed);

                m_megaAtackDelay -= (float)elapsed.TotalSeconds;

                if (m_director.EndGame())
                {
                    GameOver();
                }
            }
            else
            {
                DoMessages();
            }

            ChangeSlide();

            base.Update(elapsed);
        }
        
        private void DoBackGround()
        {
            if (m_backGround != null)
            {
                m_backGround.DoMovement();
            }
        }

        private void DoMessages()
        {
            if (base.Message.DialogResult == NamoCode.Game.Class.Screens.Messages.ResultadoDialogo.Si)
            {
                GoBack();
                base.Message.Dispose();
                base.Message = null;
            }
            else if (base.Message.DialogResult == NamoCode.Game.Class.Screens.Messages.ResultadoDialogo.No)
            {
                base.Message.Dispose();
                base.Message = null;
            }

        }

        private void DoMegaAtack()
        {
            if (m_megaAtackDelay <= 0)
            {
                Explosiones.Instance.NewExplosion();
                m_director.MegaAtack();
                m_megaAtackDelay = c_MegaAtackDelay;
            }
        }

        #region Input

        public override void CheckInput(TimeSpan elapsed)
        {
            base.Input = InputState.GetInputState();
            ObtenerTeclado((float)elapsed.TotalSeconds);            
        }
        


        private void ObtenerTeclado(float elapsed)
        {
            Prota prota = (m_director.Objetos as InfoObjectsStarPaper).Protagonista;

            if (prota != null)
            {
                if (base.Input.KeyboardState.IsKeyDown(Keys.Left) ||
                    (base.Input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickLeft)))
                {
                    prota.MoverIzquierda();
                }
                if (base.Input.KeyboardState.IsKeyDown(Keys.Right)||
                    base.Input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    prota.MoverDerecha();
                }
                if (base.Input.KeyboardState.IsKeyDown(Keys.Up)||
                    base.Input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    prota.MoverArriba();
                }
                if (base.Input.KeyboardState.IsKeyDown(Keys.Down)||
                    base.Input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    prota.MoverAbajo();
                }

                if (base.Input.KeyboardState.IsKeyDown(Keys.Space)||
                    base.Input.GamepadOne.IsButtonDown(Buttons.A))
                {
                    prota.Shot(elapsed);
                }

                if (base.Input.KeyboardState.IsKeyDown(Keys.LeftAlt))
                {
                    DoMegaAtack();
                }
            }

            if (base.Input.KeyboardState.IsKeyDown(Keys.Escape))
            {
                GoBack();
            }

            
            
        }



        /// <summary>
        /// Realiza el movimiento del prota.
        /// </summary>
        /// <param name="movement"></param>
        private void DoMovementProta(Movement movement)
        {
            Prota prota = (m_director.Objetos as InfoObjectsStarPaper).Protagonista;

            if (prota != null)
            {
                if (movement.MovimientoEjeX == EnumMovement.Izquierda)
                {
                    prota.MoverIzquierda(movement.AceleracionX);
                }
                else if (movement.MovimientoEjeX == EnumMovement.Derecha)
                {
                    prota.MoverDerecha(movement.AceleracionX);
                }
                else if (movement.MovimientoEjeX == EnumMovement.None)
                {
                    prota.NoMovementX();
                }


                if (movement.MovimientoEjeY == EnumMovement.Arriba)
                {
                    prota.MoverArriba(movement.AceleracionY);
                }
                else if (movement.MovimientoEjeY == EnumMovement.Abajo)
                {
                    prota.MoverAbajo(movement.AceleracionY);
                }
            }
        }

        #endregion



        #endregion

        #region Draw

        public override void Draw()
        {                        
            if (m_backGround != null)
            {
                m_backGround.Draw(base.SpriteBatch);
            }

            m_director.Objetos.Draw(base.SpriteBatch);

            Explosiones.Instance.Draw(base.SpriteBatch);

            m_danger.Draw(base.SpriteBatch);

            m_gamehub.Draw(base.SpriteBatch);

            base.Draw();
        }

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) || base.Input.KeyboardState.IsKeyDown(Keys.Q))
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.SECOND_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Second");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) || base.Input.KeyboardState.IsKeyDown(Keys.W)) 
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.FORTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Forth");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
        }

        #endregion
    }
}
