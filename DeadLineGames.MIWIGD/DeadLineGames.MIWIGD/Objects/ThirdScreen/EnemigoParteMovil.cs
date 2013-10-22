/*
 * Creador : Chema  
 * Fecha Creacion: 19/06/2012
 * Notas
 * 
 * Enemigo que contiene una parte que puede poseer movilidad.
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Shoots.ListaDisparos;
using StarPaper.Class.Objects.Enemies;

using NamoCode.Game.Utils;

namespace StarPaper.Class.Objects.Enemies
{
    public class EnemigoParteMovil : Enemigo , ISeek
    {
        #region Constructor

        public EnemigoParteMovil(Texture2D texture, Texture2D movepart , string name, Bounds bounds)
            : base(texture, name, bounds)
        { 
            m_movePart = movepart;                    
        }

        #endregion

        #region Fields

        private Texture2D m_movePart;

        protected Texture2D MovePart
        {
            get { return m_movePart; }
            set { m_movePart = value; }
        }

        private Vector2 m_movePartPosition;

        protected Vector2 MovePartPosition
        {
            get { return m_movePartPosition; }
            set { m_movePartPosition = value; }
        }

        public override Vector2 Posicion
        {
            get
            {
                return base.Posicion;
            }
            set
            {
                base.Posicion = value;

                m_movePartPosition = CalcMovePartPosition(value);
            }
        }


        /// <summary>
        /// La posición del vector para los calculos del angulo de rotación.
        /// </summary>
        protected virtual Vector2 VectorCalculo
        {
            get
            {
                return new Vector2(base.Center.X + m_movePart.Width, base.Center.Y + m_movePart.Height / 2);
            }
        }

        private float m_rotation = 1;

        protected float MovePartRotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        private Vector2 m_seekObject;

        public Vector2 SeekObject
        {
            get { return m_seekObject; }
            set { m_seekObject = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calcula los valores para la posición de la parte móvil
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected virtual Vector2 CalcMovePartPosition(Vector2 position)
        {
            return new Vector2(base.Posicion.X + this.Width / 2, base.Posicion.Y + this.Height / 2 - m_movePart.Height / 2);
        }

        /// <summary>
        /// Calcula la rotación que tiene que tener nuestra parte móvil para seguir al objetivo
        /// </summary>
        /// <param name="vector">
        /// Es el vector donde se encuentra nuestro objetivo
        /// </param>        
        protected virtual void CalcRotation()
        {
            m_rotation = (float)GodClass.AngleBetween(this.SeekObject, this.Center);
        }

        public override void Shot()
        {
            DisparoMovil disparoactual = (DisparoMovil)base.TypeShot.Clone();

            //TODO: Para mejorar esto lo razonable, sería calcular la posición donde se encuentra actualmente la boca del cañón.
            disparoactual.Posicion = this.Center;

            disparoactual.SetMovement((double)m_rotation);

            Disparos.Instance.Add(disparoactual);

        }

        public override void SetPosicion(float x, float y)
        {
            this.Posicion = new Vector2(x, y);
        }


        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);

            CalcRotation();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
           
            base.Draw(spritebatch);

            spritebatch.Begin();
            
            spritebatch.Draw(m_movePart, m_movePartPosition, new Rectangle(0, 0, m_movePart.Width, m_movePart.Height), Color.White,
                m_rotation, new Vector2(0, m_movePart.Height / 2), 1, SpriteEffects.None, 1);

            spritebatch.End();
        }

        #endregion

    }
}
