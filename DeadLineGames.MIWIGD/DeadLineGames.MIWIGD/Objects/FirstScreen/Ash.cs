using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using NamoCode.Game.Utils;

namespace DeadLinesGames.MIWIGD.Objects.FirstScreen
{
    public class Ash : AObject
    {
        private bool m_isBounds;

        public bool IsBounds
        {
            get { return m_isBounds; }
            set 
            { 
                m_isBounds = value;

                if (value == true)
                {
                    this.BlockedMovement = this.Movement;
                }
                else
                {
                    this.BlockedMovement = EnumMovement.None;
                }
            }
        }
        private TimeSpan currenttimeToAnimate;

        private EnumMovement m_dirBlockedMovement;

        public EnumMovement BlockedMovement
        {
            get { return m_dirBlockedMovement; }
            set 
            {                
                m_dirBlockedMovement = value;                 
            }
        }

        private InputState m_input;

        public EnumMovement Movement
        {
            get { return m_movement; }
            set
            {
                if (m_movement != value)
                {
                    Animate();
                }
                
                m_movement = value;
            }
        }
                                
        public Ash(Texture2D texture, string name, Vector2 position , FrameRateInfo frameRate , Bounds bounds)
            : base(texture, name, position, frameRate , bounds)
        {
            base.Movimiento = 3;                        
        }

        private EnumMovement m_movement;

        

        
        
        public override void Update(TimeSpan elapsed)
        {
            m_input = InputState.GetInputState();
            
            CheckBounds();

            if (m_movement != EnumMovement.None)
            {
                currenttimeToAnimate += elapsed;
                if (currenttimeToAnimate.Milliseconds >= 250)
                {
                    Animate();
                    currenttimeToAnimate = new TimeSpan(0);
                }
            }

            GetDirection();

            if (this.BlockedMovement != this.Movement)
            {
                DoMovement();
            }
            else
            {
                this.IsBounds = false;
            }
                                                
            base.Update(elapsed);
        }

        public void GetDirection()
        {
            if (m_input.KeyboardState.IsKeyDown(Keys.Up) ||
                    m_input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickUp))
            {                
                this.Movement = EnumMovement.Arriba;
            }
            else if (m_input.KeyboardState.IsKeyDown(Keys.Down) ||
                m_input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickDown))
            {                
                this.Movement = EnumMovement.Abajo;
            }
            else if (m_input.KeyboardState.IsKeyDown(Keys.Left) ||
                m_input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickLeft))
            {                
                this.Movement = EnumMovement.Izquierda;
            }
            else if (m_input.KeyboardState.IsKeyDown(Keys.Right) ||
                m_input.GamepadOne.IsButtonDown(Buttons.LeftThumbstickRight))
            {                
                this.Movement = EnumMovement.Derecha;
            }
            else
            {                
                this.Movement = EnumMovement.None;
            }
        }

        public override void DoMovement()
        {
            if (this.IsBounds == false)
            {
                if (this.Movement == EnumMovement.Arriba)
                {
                    MoverArriba();
                }
                else if (this.Movement == EnumMovement.Izquierda)
                {
                    MoverIzquierda();
                }
                else if (this.Movement == EnumMovement.Derecha)
                {
                    MoverDerecha();
                }
                else if (this.Movement == EnumMovement.Abajo)
                {
                    MoverAbajo();
                }
                else
                {
                    Stay();
                }
            }
            else
            {
                Stay();
            }
        }

        private void Stay()
        {
            if (m_movement == EnumMovement.Arriba)
            {
                Frames.ChangeFrame(1);
            }
            else if (m_movement == EnumMovement.Izquierda)
            {
                Frames.ChangeFrame(3);
            }
            else if (m_movement == EnumMovement.Derecha)
            {
                base.SpriteEffect = SpriteEffects.FlipHorizontally;
                Frames.ChangeFrame(3);
            }
            else if (m_movement == EnumMovement.Abajo)
            {
                Frames.ChangeFrame(-1);
            }

        }

        private void Animate()
        {
            if (m_movement == EnumMovement.Arriba)
            {
                base.SpriteEffect = SpriteEffects.None;
                if (Frames.ActualFrame == 2)
                {
                    Frames.ChangeFrame(3);
                }
                else
                {
                    Frames.ChangeFrame(2);
                }
            }
            else if (m_movement == EnumMovement.Derecha)
            {
                base.SpriteEffect = SpriteEffects.FlipHorizontally;
                if (Frames.ActualFrame == 4)
                {
                    Frames.ActualFrame = 5;
                }
                else
                {
                    Frames.ActualFrame = 4;
                }
            }
            else if (m_movement == EnumMovement.Izquierda)
            {
                base.SpriteEffect = SpriteEffects.None;
                if (Frames.ActualFrame == 4)
                {
                    Frames.ActualFrame = 5;
                }
                else
                {
                    Frames.ActualFrame = 4;
                }
            }
            else if (m_movement == EnumMovement.Abajo)
            {
                base.SpriteEffect = SpriteEffects.None;
                if (Frames.ActualFrame == 0)
                {
                    Frames.ActualFrame = 1;
                }
                else
                {
                    Frames.ActualFrame = 0;
                }
            }
        }
                
        private void CheckBounds()
        {
            if ((base.Posicion.X > base.Bounds.ActiveMinX && base.Posicion.X < base.Bounds.ActiveMaxX) &&
                (base.Posicion.Y > base.Bounds.ActiveMinY && base.Posicion.Y < base.Bounds.ActiveMaxY))
            {
                m_isBounds = false;
            }
            else
            {
                base.Posicion = new Vector2(480, 75);
            }
        }        
    }
}
