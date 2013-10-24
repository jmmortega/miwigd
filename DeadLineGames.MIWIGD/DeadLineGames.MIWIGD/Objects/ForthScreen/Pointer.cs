using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;

namespace DeadLineGames.MIWIGD.Objects.ForthScreen
{
    public class Pointer : AObject
    {
        public delegate void HandleEndTravel(object sender, EventArgs e);
        public event HandleEndTravel OnEndTravel;


        private Bounds m_bounds;

        private EnumMovement m_invalidMovement;

        public EnumMovement InvalidMovement
        {
            get { return m_invalidMovement; }
            set 
            {           
                if(value == EnumMovement.None)
                {
                    if (m_startPosition == Vector2.Zero)
                    {
                        m_startPosition = base.Posicion;
                    }
                }

                if (value != EnumMovement.None)
                {                    
                    m_lastPosition = base.Posicion;
                    this.LastInvalidMovement = value;                    
                }

                if (m_invalidMovement != value && m_startPosition != Vector2.Zero && m_lastPosition != Vector2.Zero)
                {
                    if (OnEndTravel != null)
                    {
                        OnEndTravel(this, new EventArgs());
                    }                    
                }

                m_invalidMovement = value;                                
            }
        }

        private EnumMovement m_lastInvalidMovement;

        public EnumMovement LastInvalidMovement
        {
            get { return m_lastInvalidMovement; }
            set { m_lastInvalidMovement = value; }
        }

        private Vector2 m_startPosition;

        public Vector2 StartPosition
        {
            get { return m_startPosition; }
            set { m_startPosition = value; }
        }

        private Vector2 m_lastPosition;

        public Vector2 LastPosition
        {
            get { return m_lastPosition; }
            set { m_lastPosition = value; }
        }
                                                
        public Pointer(Texture2D textura, String name, Vector2 posicion , Bounds bounds)
            : base(textura, name, posicion)
        {
            base.Movimiento = 1;
            m_bounds = bounds;            
        }

        public override void Update(TimeSpan elapsed)
        {
            if (base.Posicion.X + (base.Texture.Width / 2)  < m_bounds.MinX)
            {
                base.Posicion = new Vector2(m_bounds.MinX - (base.Texture.Width / 2), base.Posicion.Y);
                this.InvalidMovement = EnumMovement.Izquierda;
            }
            else if (base.Posicion.X > m_bounds.MaxX - (base.Texture.Width / 2))
            {
                base.Posicion = new Vector2(m_bounds.MaxX - base.Texture.Width / 2, base.Posicion.Y);
                this.InvalidMovement = EnumMovement.Derecha;
            }
            else if (base.Posicion.Y + (base.Texture.Height / 2) < m_bounds.MinY)
            {
                base.Posicion = new Vector2(base.Posicion.X, m_bounds.MinY - (base.Texture.Height / 2));
                this.InvalidMovement = EnumMovement.Arriba;
            }
            else if (base.Posicion.Y > m_bounds.MaxY - base.Texture.Height / 2)
            {
                base.Posicion = new Vector2(base.Posicion.X, m_bounds.MaxY - (base.Texture.Height / 2));
                this.InvalidMovement = EnumMovement.Abajo;
            }
            else
            {
                this.InvalidMovement = EnumMovement.None;
            }

            base.Update(elapsed);
        }

        public void Pull()
        {
            if (LastInvalidMovement == EnumMovement.Izquierda)
            {
                base.MoverIzquierda();
            }
            else if (LastInvalidMovement == EnumMovement.Derecha)
            {
                base.MoverDerecha();
            }
            else if (LastInvalidMovement == EnumMovement.Abajo)
            {
                base.MoverAbajo();
            }
            else if (LastInvalidMovement == EnumMovement.Arriba)
            {
                base.MoverArriba();
            }
        }
    }
}
