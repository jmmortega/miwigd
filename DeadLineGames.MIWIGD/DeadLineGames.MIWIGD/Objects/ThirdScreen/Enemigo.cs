/*
 * Creador : Chema 
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * 
 * Clase padre principal de enemigo
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase
 * 
 * 1.1 16/05/2012
 * 
 * Convierto la clase a abstracta para evitar que se utilice directamente.
 * 
 * 1.2 31/05/2012
 * 
 * Se agrega una comprobación de liberación de memoria cuando el objeto sale fuera de la pantalla. 
 * Se sobrecarga la propiedad Posicion, para agregar agregar una variable booleana indicando que el objeto se encuentra fuera de los limites.
 * Se implementa el dispose correctamente.
 * 
 * 1.3 08/06/2012
 * 
 * Se introduce la propiedad points, para indicar los puntos que se adquieren por cada nave enemiga.
 * 
 * 1.4 10/06/2012
 * 
 * Se sobrecarga el método HaveColission para evitar que se realicen comprobaciones de colisiones innecesarias.
 * 
 * 1.5 29/06/2012
 * 
 * Cuando se realiza el método Kill, se solicita una explosión dentro del mismo.
 * 
 * 1.6 06/07/2012
 * 
 * Se añade check comprobando si el enemigo es final boss
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StarPaper.Class.Events;
using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Buenos;
using StarPaper.Class.Design;

using NamoCode.Game.Class.Objects;


using NamoCode.Game.Utils;

namespace StarPaper.Class.Objects.Enemies
{
    public abstract class Enemigo : AObject , IDisposable , IKillable, IShip
    {
        
        public Enemigo(Texture2D textura , string name , Bounds bounds)
            :base(textura , name , bounds)
        { }
        
        #region Fields

        private float m_shield = 0;

        /// <summary>
        /// La cantidad de vida
        /// </summary>
        /// <remarks>
        /// He preferido que sea un float, por si algún impacto no hace un daño completo.
        /// </remarks>
        public float Shield
        {
            get { return m_shield; }
            set { m_shield = value; }
        }
        
        private Disparo m_disparo;

        /// <summary>
        /// El tipo de disparo del Enemigo, la propiedad se encuentra protegida.
        /// </summary>
        public Disparo TypeShot
        {
            get { return m_disparo; }
            set { m_disparo = value; }
        }

        private ShootLogic m_shootLogic;

        /// <summary>
        /// La lógica del disparo.
        /// </summary>
        public ShootLogic ShootLogic
        {
            get { return m_shootLogic; }
            set { m_shootLogic = value; }
        }

        private Patrons m_patronesMovimiento = new Patrons();

        public Patrons PatronesMovimiento
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

        public override Vector2 Posicion
        {
            get
            {
               return base.Posicion;
            }
            set
            {
                //Si el valor es negativo, se encuentra fuera de los limites, indicamos que inicialmente el objeto esta fuera.
                if (value.X < 0 || value.Y < 0 || value.X > base.Bounds.ActiveMaxX || value.Y > base.Bounds.ActiveMaxY)
                {
                    m_firstBound = true;
                }

                base.Posicion = value;
            }
        }

        private int m_points = 0;

        public int Points
        {
            get { return m_points; }
            set { m_points = value; }
        }

        private bool m_isFinalBoss = false;

        public bool IsFinalBoss
        {
            get { return m_isFinalBoss; }
            set { m_isFinalBoss = value; }
        }
                        
        #endregion

        #region Methods

        #region Movement

        public override void DoMovement()
        {
            //Se ha podido comprobar que al disposear puede entrar aquí y producirse un nullreference por lo que hago una comprobación de null previa.
            if (m_patronesMovimiento != null) 
            {
                if (m_patronesMovimiento.Count != 0 && m_patronActual == null) //No hay patrón de movimiento
                {
                    DownLoadPatron();
                }
                else if (m_patronActual != null) //Hay un patrón de movimiento cargado.
                {
                    Vector2? vector = m_patronActual.GetMovement();

                    EnumMovement colision = EnumMovement.None;

                    /* TODO: Hay que comprobar la colisión detenidamente */
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
                        SetPosicion(base.Posicion.X + vector.Value.X, base.Posicion.Y + vector.Value.Y);
                    }
                    else
                    {
                        DownLoadPatron();
                    }
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
        /// <remarks>
        /// Hay un pequeño error o bug. Cuando el objeto choca contra una esquina, se realizan comprobaciones y solo puede devolver una esquina. Hay que realizar 
        /// modificaciones para que esto quede correcto.
        /// </remarks>
        public EnumMovement CheckBounds(Vector2? movimiento)
        {
            if (movimiento != null)
            {
                if (base.Posicion.Y - movimiento.Value.Y < base.Bounds.ActiveMinY)
                {
                    return EnumMovement.Arriba;
                }
                else if (base.Posicion.X + movimiento.Value.X + base.Width > base.Bounds.ActiveMaxX)
                {
                    return EnumMovement.Derecha;
                }
                else if (base.Posicion.Y + movimiento.Value.Y + base.Height > base.Bounds.ActiveMaxY)
                {
                    return EnumMovement.Abajo;
                }
                else if (base.Posicion.X + movimiento.Value.X < base.Bounds.ActiveMinX)
                {
                    return EnumMovement.Izquierda;
                }
                else
                {
                    return EnumMovement.None;
                }
            }
            else
            {
                return EnumMovement.None;
            }
        }

        #endregion

        #region IKillable

        /// <summary>
        /// Destrucción del objeto y disparo.
        /// </summary>
        /// <remarks>
        /// Inicialmente será el dispose del objeto, pero los hijos tendrán otras opciones, como por ejemplo la explosión.
        /// </remarks>
        public virtual void Kill()
        {
            Explosiones.Instance.NewExplosion(base.Posicion);
            this.Dispose();
        }

        #endregion

        #region Disparo

        /// <summary>
        /// Comienza la comprobación de los disparos.
        /// </summary>
        public virtual void StartShooting()
        {
            m_shootLogic.Start();
        }

        /// <summary>
        /// Para los disparos.
        /// </summary>
        public virtual void StopShooting()
        {
            m_shootLogic.Stop();
        }

        /// <summary>
        /// Verifica si se realiza un disparo.
        /// </summary>
        public virtual void CheckShooting(float elapsed)
        {
            if (m_shootLogic.HaveShot(elapsed) == true)
            {
                Shot();
            }
        }

        /// <summary>
        /// Verifica si se encuentra fuera del marco para liberar memoria.
        /// </summary>
        public virtual void CheckBoundToDispose()
        {
            //Comprobamos si inicialmente se encuentra fuera de los limites de la 
            if (m_firstBound == true)
            {
                if(base.Posicion.X + base.Texture.Width > 0 || 
                    base.Posicion.Y + base.Texture.Height > 0 || 
                    base.Posicion.X - base.Texture.Width < base.Bounds.ActiveMaxX || 
                    base.Posicion.Y -base.Texture.Height < base.Bounds.ActiveMaxY)
                {
                    m_firstBound = false;
                }
            }
            else
            {
                float x = base.Posicion.X;
                float y = base.Posicion.Y;

                if (base.Posicion.X + base.Texture.Width < 0 || 
                    base.Posicion.Y + base.Texture.Height < 0 || 
                    base.Posicion.X - base.Texture.Width > base.Bounds.ActiveMaxY || 
                    base.Posicion.Y - base.Texture.Height > base.Bounds.ActiveMaxY)
                {
                    this.Dispose();
                }
            }
        }

        /// <summary>
        /// Lanza el disparo
        /// </summary>
        public virtual void Shot()
        {
            if (m_disparo != null)
            {
                Disparo disparoactual = (Disparo)m_disparo.Clone();

                disparoactual.Posicion = base.Posicion;

                Disparos.Instance.Add(disparoactual);

                m_shootLogic.ChangeAmount(-1);
            }
        }

        
        
        /// <remarks>
        /// Este método no es demasiado útil en la clase enemigo tiene mayor sentido dentro de la del Prota.
        /// </remarks>
        public virtual void Shot(float currentTime)
        {
            CheckShooting(currentTime);
        }

        public override bool HaveColision(AObject objeto)
        {
            if (objeto.GetType() == typeof(Prota) || GodClass.IsContainsInterface(objeto , typeof(IGoodGuyShoot)) == true)
            {
                return base.HaveColision(objeto);
            }
            else
            {
                return false;
            }            
        }

        #endregion

        #region GeneralMethods

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }

        public override void Update(TimeSpan elapsed)
        {
            DoMovement();
            CheckShooting((float)elapsed.TotalSeconds);
            CheckBoundToDispose();
            base.Update(elapsed);
        }        

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                if (m_patronesMovimiento != null)
                {
                    m_patronesMovimiento.Clear();
                }

                m_patronesMovimiento = null;

                if (m_disparo != null)
                {
                    m_disparo.Dispose();
                }

                if (m_patronActual != null)
                {
                    m_patronActual.Dispose();
                }

                this.Shield = 0;
            }

            base.Dispose(disposing);
        }

        ~Enemigo()
        {
            this.Dispose(false);
        }


        #endregion



        #endregion





    }
}
