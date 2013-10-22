using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;

namespace DeadLinesGames.MIWIGD.Objects.FirstScreen
{
    public class OakLindo : AObject
    {
        public Vector2 PosicionAsh { get; set; }

        public delegate void FinishWalking(object sender, EventArgs e);
        public event FinishWalking OnFinishWalking;


        private int m_sequence;
                
        public OakLindo(Texture2D texture, string name, Vector2 posicion , FrameRateInfo frameRate, Bounds bounds)
            : base(texture, name, posicion, frameRate , bounds)
        {
            base.Movimiento = 3;            
        }

        private EnumMovement m_movement;
        private TimeSpan currenttimeToAnimate;

        public override void Update(TimeSpan elapsed)
        {
            if (m_movement != EnumMovement.None)
            {
                currenttimeToAnimate += elapsed;
                if (currenttimeToAnimate.Milliseconds >= 250)
                {
                    Animate();
                    currenttimeToAnimate = new TimeSpan(0);
                }
            }
            base.Update(elapsed);
        }

        public override void DoMovement()
        {            
            if (m_sequence == 0)
            {
                if (base.Posicion.Y > 350)
                {
                    MoverArriba();
                    m_movement = EnumMovement.Arriba;
                }
                else
                {
                    m_sequence++;
                }
            }
            else if (m_sequence == 1)
            {
                if (PosicionAsh.X - 5 < base.Posicion.X || base.Posicion.X > PosicionAsh.X + 5)
                {
                    m_sequence++;
                }
                else if (PosicionAsh.X > base.Posicion.X)
                {
                    MoverDerecha();
                    m_movement = EnumMovement.Derecha;
                }
                else if (PosicionAsh.X < base.Posicion.X)
                {
                    MoverIzquierda();
                    m_movement = EnumMovement.Izquierda;
                }
            }
            else if (m_sequence == 2)
            {
                if (PosicionAsh.Y < base.Posicion.Y - base.Height)
                {
                    MoverArriba();
                    m_movement = EnumMovement.Arriba;
                }
                else
                {
                    m_sequence++;
                }
            }
            else if (m_sequence == 3)
            {
                Frames.ChangeFrame(2);

                if (OnFinishWalking != null)
                {
                    OnFinishWalking(this, new EventArgs());
                }
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
    }
}
