/*
 * Creador : Chema 
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * La clase que determina que tipo de disparo tiene cada objeto.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase.
 * 
 * 1.1 16/05/2012
 * 
 * Se agrega las funciones de movimiento
 * 
 * 1.2 17/05/2012
 * 
 * Se encapsula los patrones de movimiento.
 * 
 * 1.3 06/06/2012
 * 
 * Se introducen las distintas comprobaciones para disposear el objeto cuando se encuentra fuera de pantalla.
 * Se implementa el método Dispose.
 * 
 * 1.4 11/06/2012
 * 
 * Se integran chequeos de colisiones. Para evitar comprobaciones innecesarias.
 * 
 * 1.5 12/06/2012
 * 
 * Se añade la comprobación para retirar colisiones entre disparos buenos y el prota.
 * 
 * 1.6 19/06/2012
 * 
 * Se modifica el método ChecksBoundToDispose. No estaba liberando la memoria
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

using StarPaper.Class.Events;
using StarPaper.Class.Objects.Enemies;
using StarPaper.Class.Objects.Buenos;

namespace StarPaper.Class.Objects.Shoots
{
    public abstract class Disparo : AObject , IKillable , IMyCloneable
    {
        public Disparo(Texture2D textura, string name, Bounds bounds)
            : base(textura, name, bounds)
        { }

        #region Fields

        private float m_damage;

        /// <summary>
        /// El daño que realiza este disparo.
        /// </summary>
        public float Damage
        {
            get { return m_damage; }
            set { m_damage = value; }
        }

        private Patrons m_patronesMovimiento = new Patrons();

        protected Patrons PatronesMovimiento
        {
            get { return m_patronesMovimiento; }
            set { m_patronesMovimiento = value; }
        }

        private MovementPatron m_patronActual;

        protected MovementPatron PatronActual
        {
            get { return m_patronActual; }
            set { m_patronActual = value; }
        }

        private bool m_firstBound = false;

        /// <summary>
        /// Indica que el objeto se encuentra inicialmente fuera de los limites.
        /// </summary>
        protected bool FirstBound
        {
            get { return m_firstBound; }
            set { m_firstBound = value; }
        }


        #endregion

        #region Methods

        #region Movement

        public override void DoMovement()
        {
            //TODO: Implementar el método DoMovement usando la clase Patrons...
            //No olvidarse de explosiones.

            if (m_patronesMovimiento.Count != 0 && m_patronActual == null) //No hay patrón de movimiento
            {
                DownLoadPatron();
            }
            else if (m_patronActual != null) //Hay un patrón de movimiento cargado.
            {
                Vector2? vector = m_patronActual.GetMovement();

                EnumMovement colision = EnumMovement.None;

                //Verificamos si hay que hacer comprobación en los limites.
                if (m_patronActual.ExitBounds.IsRequiredCheck == true)
                {
                    colision = CheckBounds(vector);

                    if (colision != EnumMovement.None)
                    {
                        if (m_patronActual.ExitBounds.HaveEffect == true)
                        {
                            m_patronActual.ChangeMovement(colision);

                            vector = m_patronActual.ChangeMovement(colision, vector);
                        }
                    }
                }

                if (vector != null)
                {
                    base.SetPosicion(base.Posicion.X + vector.Value.X, base.Posicion.Y + vector.Value.Y);
                }
                else
                {
                    DownLoadPatron();
                }

            }
        }

        /// <summary>
        /// Recupera un patron de movimiento de la lista de patrones.
        /// </summary>
        private void DownLoadPatron()
        {
            if (m_patronesMovimiento.Count != 0)
            {
                m_patronActual = m_patronesMovimiento.Pop();
            }
            else
            {
                m_patronActual = null;
            }
        }

        /// <summary>
        /// Comprueba si el movimiento agregado produce que se choque contra la pantalla.
        /// En ese caso devuelve el vector de posición alterado para que no se salga.
        /// </summary>
        /// <param name="vector">
        /// El vector original sin comprobar
        /// </param>
        /// <returns>
        /// Devuelve una enumeración de movimiento, aquí indica por donde se produció el la salida del limite.
        /// </returns>
        public EnumMovement CheckBounds(Vector2? movimiento)
        {
            if (base.Posicion.Y - movimiento.Value.Y < base.Bounds.MinY)
            {
                return EnumMovement.Arriba;
            }
            else if (base.Posicion.X + movimiento.Value.X + base.Width > base.Bounds.MaxX)
            {
                return EnumMovement.Derecha;
            }
            else if (base.Posicion.Y + movimiento.Value.Y + base.Height > base.Bounds.MaxY)
            {
                return EnumMovement.Abajo;
            }
            else if (base.Posicion.X + -movimiento.Value.X < base.Bounds.MinX)
            {
                return EnumMovement.Izquierda;
            }
            else
            {
                return EnumMovement.None;
            }
        }

        public virtual void CheckBoundToDispose()
        {

            float x = base.Posicion.X;
            float y = base.Posicion.Y;

            if (base.Posicion.X + base.Texture.Width < 0 ||
                base.Posicion.Y + base.Texture.Height < 0 ||
                base.Posicion.X - base.Texture.Width > base.Bounds.MaxX ||
                base.Posicion.Y - base.Texture.Height > base.Bounds.MaxY)
            {
                this.Dispose();
            }
            
        }

        #endregion

        #region General Methods

        public override void Draw(SpriteBatch spritebatch)
        {
           base.Draw(spritebatch);
        }

        public override void Update(TimeSpan elapsed)
        {
            DoMovement();
            CheckBoundToDispose();
            base.Update(elapsed);
        }

        public override bool HaveColision(AObject objeto)
        {
            ///Comprobamos que las colisiones entre disparos no produzcan nada.
            if(GodClass.IsContainsFather(objeto.GetType() , this.GetType()))
            {                
                return false;
            }
            else if((GodClass.IsContainsInterface(this , typeof(IGoodGuyShoot)) == false) && (GodClass.IsContainsFather(objeto.GetType() , typeof(Enemigo))))
            {
                return false;
            }
            else if ((GodClass.IsContainsInterface(this, typeof(IGoodGuyShoot)) == true) && (GodClass.IsContainsFather(objeto.GetType(), typeof(Prota))))
            {
                return false;
            }
            else
            {
                return base.HaveColision(objeto);
            }
            
        }

        #endregion

        public virtual void Kill()
        {
            this.Dispose();
        }

        #endregion

        #region IMyclonable
                
        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {           
            m_patronesMovimiento.Clear();

            if (m_patronActual != null)
            {
                m_patronActual.Dispose();
            }

            base.Dispose(disposing);
        }

        ~Disparo()
        {
            Dispose(false);
        }

        #endregion
    }
}
