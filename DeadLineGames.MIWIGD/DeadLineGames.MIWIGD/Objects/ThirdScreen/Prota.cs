/*
 * Creador : Chema 
 * Fecha Creacion: 07/06/2012
 * Notas
 * 
 * Clase que maneja a la nave del jugador
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
 * 
 * 1.1 12/06/2012
 * 
 * Se incorpora el disparo.
 * Se incluye la capacidad de cadencia dentro del disparo. 
 * 
 * 1.2 14/06/2012
 * 
 * Se ha modificado el nombre de Lifes por Shield dentro de la interfaz 
 * Se ha incluido el nombre de Lifes, para las vidas del personaje.
 * 
 * 1.3 30/06/2012
 * 
 * Se agrega un método para asignar el valor del bonus.
 * Se añade la propiedad NumberShots para indicar la cantidad de disparos que se producen.
 * 
 * 1.4 07/07/2012
 * 
 * Se reduce un valor de bonus, cuando se muere.
 * Se limita el daño a 10 como máximo en el bonus.
 * 
 * 1.5 08/07/2012
 * 
 * Se integra una nueva propiedad ProtaValues para asignar los valores al comienzo de una nueva fase.
 * 
 * 1.6 11/07/2012
 * 
 * Cuando mueres todos los valores de bonus vuelven a 0
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Class.Objects;
using NamoCode.Game.Utils;

using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Shoots.ListaDisparos;

using StarPaper.Utils;

namespace StarPaper.Class.Objects.Buenos
{    
    public class Prota : AObject , IDisposable , IShip
    {

        #region Consts

        ///El frame inicial
        private const int c_normalFrame = 2;

        #endregion

        #region Constructor

        public Prota(Texture2D textura, string name, FrameRateInfo frames, Bounds bounds)
            : base(textura, name, frames, bounds)
        {
            base.Movimiento = 5;

            this.TypeShot = new GoodGuyShoot(ShootTextures.Instance["GoodGuyShoot"], "GoodGuyShoot", base.Bounds);

            ///Marcamos como frame actual el 2 que es el frame de origen.
            //base.ActualFrame = c_normalFrame;
        }

                                                
        #endregion

        #region Fields

        private float m_shield = 100;

        /// <summary>
        /// No representa las vidas. Representa el valor de escudos.
        /// </summary>
        public float Shield
        {
            get
            {
                return m_shield;
            }
            set
            {
                m_shield = value;
            }
        }

        private int m_lifes = 3;

        public int Lifes
        {
            get { return m_lifes; }
            set { m_lifes = value; }
        }

        private Disparo m_disparo = null;

        public Disparo TypeShot
        {
            get { return m_disparo; }
            set { m_disparo = value; }
        }

        
        private float m_cadence = 0.2f;

        /// <summary>
        /// La cadencia entre disparo y disparo. Se expresa en segundos.
        /// </summary>
        public float Cadence
        {
            get { return m_cadence; }
            set { m_cadence = value; }
        }

        private float m_nextShot = 0f;

        private int m_numberShots = 1;

        public int NumberShots
        {
            get { return m_numberShots; }
            set { m_numberShots = value; }
        }

        private ProtaValues m_values;

        public ProtaValues Values
        {
            get 
            {
                return new ProtaValues(
                    m_lifes,
                    m_disparo.Damage,
                    m_cadence,
                    m_numberShots,
                    m_shield);
            }
            set 
            { 
                m_values = value;
                m_lifes = value.Lifes;
                m_disparo.Damage = value.ShotDamage;
                m_cadence = value.Cadence;
                m_numberShots = value.NumberShots;
                m_shield = value.Shield;
                

            }
        }

        /// <summary>
        /// Enumeración para indicar el movimiento del personaje, para el control del sprite sheet 
        /// </summary>
        private EnumMovement m_movimiento = EnumMovement.None;
        
        #endregion
        
        #region Methods

        public void Shot()
        {
            for (int i = 0; i < m_numberShots; i++)
            {
                Disparo disparoactual = (Disparo)this.TypeShot.Clone();

                disparoactual.SetPosicion(base.Posicion.X + (base.Width / (i + 1)  - TypeShot.Width), base.Posicion.Y);

                Disparos.Instance.Add(disparoactual);
            }
        }

        public void Shot(float currenttime)
        {
            if (m_nextShot <= 0)
            {
                m_nextShot = m_cadence;

                this.Shot();
            }
            else
            {
                m_nextShot -= currenttime;
            }
            
        }
                
        public void Kill()
        {
            m_lifes--;

            m_disparo.Damage = 1;

            m_numberShots = 1;

            m_cadence = 0.2f;
            
            if (m_lifes <= 0)
            {
                this.Dispose();
            }
            else
            {
                m_shield = 100;
                base.SetPosicion(base.Bounds.ActiveMaxX / 2, base.Bounds.ActiveMaxY - base.Height);
            }
        }

        public void SetBonus(IBonus bonus)
        {
            if (bonus.BonusType == EnumBonusType.BonusDamage)
            {
                if (m_disparo.Damage < 10)
                {
                    m_disparo.Damage = m_disparo.Damage + 1;
                }
            }
            else if (bonus.BonusType == EnumBonusType.BonusRapid)
            {
                if (m_cadence >= 0.1)
                {
                    m_cadence = m_cadence - 0.01f;
                }
            }
            else if (bonus.BonusType == EnumBonusType.BonusShot)
            {
                if (m_numberShots <= 5)
                {
                    m_numberShots = m_numberShots + 1;
                }
            }
        }

        #region Movement

        public override void MoverIzquierda()
        {
            m_movimiento = EnumMovement.Izquierda;
            if (base.Posicion.X - base.Movimiento > base.Bounds.ActiveMinX)
            {                
                base.MoverIzquierda();
            }
        }

        public override void MoverIzquierda(float aceleracion)
        {            
            if (aceleracion > 0)
            {
                m_movimiento = EnumMovement.Izquierda;
            }

            if (base.Posicion.X - base.Movimiento > base.Bounds.ActiveMinX)
            {
                base.MoverIzquierda(aceleracion);
            }
        }

        public override void MoverDerecha()
        {
            m_movimiento = EnumMovement.Derecha;

            if (base.Posicion.X + base.Movimiento + base.Width < base.Bounds.ActiveMaxX)
            {
                base.MoverDerecha();
            }
        }

        public override void MoverDerecha(float aceleracion)
        {
            if (aceleracion > 0)
            {
                m_movimiento = EnumMovement.Derecha;
            }

            if (base.Posicion.X + base.Movimiento + base.Width < base.Bounds.ActiveMaxX)
            {
                base.MoverDerecha(aceleracion);
            }
        }
        
        public override void MoverAbajo(float aceleracion)
        {
            if (base.Posicion.Y + base.Movimiento + base.Height < base.Bounds.ActiveMaxY)
            {
                base.MoverAbajo(aceleracion);
            }
        }

        public override void MoverAbajo()
        {
            if (base.Posicion.Y + base.Movimiento + base.Height < base.Bounds.ActiveMaxY)
            {
                base.MoverAbajo();
            }
        }

        public override void MoverArriba()
        {
            if (base.Posicion.Y - base.Movimiento > base.Bounds.ActiveMinY)
            {
                base.MoverArriba();
            }
        }

        public override void MoverArriba(float aceleracion)
        {
            if (base.Posicion.Y - base.Movimiento > base.Bounds.ActiveMinY)
            {
                base.MoverArriba(aceleracion);
            }
        }

        public void NoMovementX()
        {
            m_movimiento = EnumMovement.None;
        }

        #endregion

        protected override void DrawAnimated(SpriteBatch spritebatch)
        {
            base.DrawSingle(spritebatch);

        }

        public override void Update(TimeSpan elapsed)
        {
            
        }

        #endregion

        #region IDisposable

        public new void Dispose()
        {
            Dispose(true);
        }

        ~Prota()
        {
            Dispose(false);
        }

        protected new void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                if (m_disparo != null)
                {
                    m_disparo.Dispose();
                }

                base.Dispose(disposing);
            }
        }
        
        #endregion

       

        
    }
}
