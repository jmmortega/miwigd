using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using DeadLineGames.MIWIGD.Objects.ForthScreen;
using DeadLineGames.MIWIGD.Objects.Common;

namespace DeadLineGames.MIWIGD.Screens
{
    /// <summary>
    /// Create a idea!
    /// 
    /// There are use the Gals Panic game. 
    /// </summary>
    /// <remarks>
    /// I think we add a litle easter egg. If press any button combination, change the screen and show the 
    /// X photo like classic Glass Panic
    /// </remarks>
    public class ForthScreen : Screen
    {
        #region Fields

        private Texture2D m_photo;
        private Color[,] m_coverRectangle;
        private Color[,] m_drawingLine;

        private Vector2 m_paintingPosition;

        private Pointer m_pointer;
        
        private Color c_CoverColor = new Color(0, 0, 200, 245);

        private List<Vector2> m_points;

        private bool isCleared = false;

        #endregion

        public ForthScreen(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            m_photo = base.Content.Load<Texture2D>("ForthScreen/Photo");

            m_coverRectangle = BasicTextures.GetPixels(base.Content.Load<Texture2D>("ForthScreen/Cover"));

            m_drawingLine = BasicTextures.CrearCuadrado(
                Color.Transparent,
                m_photo.Width,
                m_photo.Height);

            Texture2D pointerTexture = base.Content.Load<Texture2D>("ForthScreen/Pointer");

            m_pointer = new Pointer(
                pointerTexture,
                "Pointer",
                new Vector2(175 - pointerTexture.Width / 2, 50 - pointerTexture.Height / 2),
                new Bounds(175,455,50,425));

            m_pointer.InvalidMovement = EnumMovement.Izquierda;
            m_pointer.OnEndTravel += new Pointer.HandleEndTravel(EndTravelPointer);

            m_paintingPosition = new Vector2(175, 50);

            m_points = new List<Vector2>();
                        
            base.Initialize();
        }
        
        public override void Update(TimeSpan elapsed)
        {            
            base.Input = InputState.GetInputState();

            if (isCleared == false)
            {
                if (base.Input.KeyboardState.IsKeyDown(Keys.Up) ||
                    base.Input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    if (m_pointer.InvalidMovement != EnumMovement.Arriba)
                    {
                        m_pointer.MoverArriba();
                    }
                }
                else if (base.Input.KeyboardState.IsKeyDown(Keys.Left) ||
                    base.Input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    if (m_pointer.InvalidMovement != EnumMovement.Izquierda)
                    {
                        m_pointer.MoverIzquierda();
                    }
                }
                else if (base.Input.KeyboardState.IsKeyDown(Keys.Right) ||
                    base.Input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    if (m_pointer.InvalidMovement != EnumMovement.Derecha)
                    {
                        m_pointer.MoverDerecha();
                    }
                }
                else if (base.Input.KeyboardState.IsKeyDown(Keys.Down) ||
                    base.Input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    if (m_pointer.InvalidMovement != EnumMovement.Abajo)
                    {
                        m_pointer.MoverAbajo();
                    }
                }
                else
                {
                    if (m_pointer.InvalidMovement == EnumMovement.None)
                    {
                        m_pointer.Pull();
                    }
                }

                m_pointer.Update(elapsed);

                if (m_pointer.InvalidMovement == EnumMovement.None)
                {
                    PaintCover(m_pointer.Posicion);

                    AddPoint(m_pointer.Posicion);
                }
            }
            else
            {
                if (base.Input.KeyboardState.IsKeyDown(Keys.Space) == true ||
                    base.Input.GamepadOne.IsButtonDown(Buttons.A) == true)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add(Consts.PARAMETERTITLE, Strings.FIFTH_TITLE);
                    parameters.Add(Consts.PARAMETERSCREEN, "Fifth");

                    ScreenManager.TransitionTo("TransitionScreen", parameters);
                }
            }
            
            if (base.Input.KeyboardState.IsKeyDown(Keys.Escape) == true)
            {
                Game.Exit();
            }

            ChangeSlide();

            base.Update(elapsed);
        }

        private void AddPoint(Vector2 posicion)
        {            
            if (m_points.Count > 0)
            {
                var lastPoint = m_points.Last();

                if (lastPoint.X != posicion.X)
                {
                    AddPointsY(posicion);
                }
                else if (lastPoint.Y != posicion.Y)
                {
                    AddPointsX(posicion);
                }
            }
                                    
            m_points.Add(GetCanvasPosition(posicion));            
        }

        private void AddPointsX(Vector2 position)
        {
            Vector2 canvasPos = GetCanvasPosition(position);
            Vector2 lastPosition = m_points.Last();
            Vector2 startPosition = GetCanvasPosition(m_pointer.StartPosition);

            if (startPosition.X < canvasPos.X)
            {
                for (int x = (int)startPosition.X; x < canvasPos.X; x++)
                {
                    m_points.Add(new Vector2(x, canvasPos.Y));
                }
            }
            else if(startPosition.X != canvasPos.X)
            {
                for (int x = (int)canvasPos.X; x < startPosition.X; x++)
                {
                    m_points.Add(new Vector2(x, canvasPos.Y));
                }
            }
        }

        private void AddPointsY(Vector2 position)
        {
            Vector2 canvasPos = GetCanvasPosition(position);
            Vector2 lastPosition = m_points.Last();
            Vector2 startPosition = GetCanvasPosition(m_pointer.StartPosition);

            if (startPosition.Y < canvasPos.Y)
            {
                for (int y = (int)startPosition.Y; y < canvasPos.Y; y++)
                {
                    m_points.Add(new Vector2(canvasPos.X, y));
                }
            }
            else if (startPosition.Y != canvasPos.Y)
            {
                for (int y = (int)canvasPos.Y; y < startPosition.Y; y++)
                {
                    m_points.Add(new Vector2(canvasPos.X, y));
                }
            }
        }

        private void PaintCover(Vector2 posicion)
        {
            Vector2 realPosition = GetCanvasPosition(posicion);

            if (realPosition.X > 0 && realPosition.Y > 0 &&
                realPosition.X < m_photo.Width && realPosition.Y < m_photo.Height)
            {
                //m_coverRectangle[(int)realPosition.X , (int)realPosition.Y] = Color.White;
                m_drawingLine[(int)realPosition.X, (int)realPosition.Y] = Color.White;
            }
        }

        private void Discover(Vector2 startPosition, Vector2 endPosition)
        {
            Vector2 realPosition = GetCanvasPosition(startPosition);
                                        
            foreach (Vector2 p in m_points)
            {
                if (p.X < m_photo.Width && p.Y < m_photo.Height)
                {
                    m_coverRectangle[(int)p.X, (int)p.Y] = Color.Transparent;
                }
            }
             
                        
            m_points.Clear();
            isCleared = isClear();
        }

        private bool isClear()
        {
            int discoverPoints = 0;

            for (int x = 0; x < m_photo.Width; x++)
            {
                for (int y = 0; y < m_photo.Height; y++)
                {
                    if (m_coverRectangle[x, y] == Color.Transparent)
                    {
                        discoverPoints++;
                    }
                }
            }

            int totalPoints = (int)((m_photo.Width * m_photo.Height) * 0.7);

            if(discoverPoints >= totalPoints)
            {
                m_coverRectangle = BasicTextures.CrearCuadrado(Color.Transparent,m_photo.Width , m_photo.Height);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ClearCover()
        {
            m_drawingLine = BasicTextures.CrearCuadrado(
                Color.Transparent,
                m_photo.Width,
                m_photo.Height);
        }

        public override void Draw()
        {
            base.SpriteBatch.Begin();

            base.SpriteBatch.Draw(m_photo, m_paintingPosition, Color.White);

            base.SpriteBatch.Draw(BasicTextures.CreateTexture(m_coverRectangle), m_paintingPosition, Color.White);

            base.SpriteBatch.Draw(BasicTextures.CreateTexture(m_drawingLine), m_paintingPosition, Color.White);
            
            base.SpriteBatch.End();

            m_pointer.Draw(base.SpriteBatch);

            base.Draw();
        }

        #region Events

        private void EndTravelPointer(object sender, EventArgs e)
        {
            if ((m_pointer.StartPosition.X != m_pointer.LastPosition.X) ||
                (m_pointer.StartPosition.Y != m_pointer.LastPosition.Y))
            {
                Discover(m_pointer.StartPosition, m_pointer.LastPosition);

                m_pointer.StartPosition = Vector2.Zero;
                m_pointer.LastPosition = Vector2.Zero;
            }
            else
            {
                ClearCover();
            }
            
        }
        
        #endregion

        #region Utils

        private Vector2 GetCanvasPosition(Vector2 posicion)
        {
            return new Vector2(((posicion.X - m_paintingPosition.X) + m_pointer.Width / 2),
                ((posicion.Y - m_paintingPosition.Y) + m_pointer.Height / 2));
        }

        #endregion

        private void ChangeSlide()
        {
            base.Input = InputState.GetInputState();

            if (base.Input.GamepadOne.IsButtonDown(Buttons.LeftShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.THIRD_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Third");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
            else if (base.Input.GamepadOne.IsButtonDown(Buttons.RightShoulder) == true)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(Consts.PARAMETERTITLE, Strings.FIFTH_TITLE);
                parameters.Add(Consts.PARAMETERSCREEN, "Fifth");

                ScreenManager.TransitionTo("TransitionScreen", parameters);
            }
        }
    }
}
