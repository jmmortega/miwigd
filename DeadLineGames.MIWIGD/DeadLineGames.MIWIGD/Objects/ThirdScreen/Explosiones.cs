/*
 * Creador : Chema
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * Clase que maneja las explosiones del juego
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase
 * 
 * 1.1 29/06/2012
 * 
 * Se modifica el método Add para hacerlo más acorde al nuevo constructor de Explosion
 * Se le añade al método NewExplosion un parametro de tipo Vector2. Tanto al que no recibía parametros, como al que recibía
 *      un string
 * Solventado el error de StackOverflow de la clase. m_ExplosionesActivas deja de ser un tipo Explosiones para ser un tipo
 *      MyListElements<Explosion>
 *      
 * 1.2 02/07/2012
 * 
 * Se sobrecarga el Clear para que se haga un limpiado de las Explosiones activas y no de los tipos de explosiones cargadas.
 * 
 * 1.3 12/07/2012
 * 
 * Se introduce el método GigaExplosion
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using NamoCode.Game.Utils.Collections;

namespace StarPaper.Class.Design
{
    /// <summary>    
    /// Esta clase engloba y maneja todas las explosiones.
    /// </summary>   
    /// <remarks>
    /// La clase funciona de la siguiente manera:
    /// Se engloban todas las explosiones dentro de una lista estática, de esa manera podemos cargar el contenido de las texturas de explosiones en cualquier momento.
    /// Dentro hay un método que solicita una explosión. Cuando se lanza este método se integra en otra lista que produce los efectos necesarios.
    /// </remarks>
    public class Explosiones : MyListElements<Explosion> , IDisposable
    {
        #region Singleton

        protected Explosiones()
        { }

        private static Explosiones m_instance = null;

        public static Explosiones Instance
        {                    
            get 
            {
                if (m_instance == null)
                {
                    m_instance = new Explosiones();
                }
                return Explosiones.m_instance; 
            }            
        }

        #endregion

        #region Fields

        private MyListElements<Explosion> m_explosionesActivas = new MyListElements<Explosion>();

        public int CountExplosionesActivas
        {
            get
            {
                return m_explosionesActivas.Count;
            }
        }
        
        #endregion


        #region Methods

        public void Add(Texture2D textura, string name , FrameRateInfo frames)
        {
            this.Add(new Explosion(textura, name, frames));
        }

        /// <summary>
        /// Se produce una nueva explosión al azar de la lista.
        /// </summary>
        public void NewExplosion(Vector2 posicion)
        {
            int number = Azar.Instance.GetNumber(0, this.Count-1);

            Explosion explosion = (Explosion)this[number].Clone();

            explosion.Posicion = posicion;

            m_explosionesActivas.Add(explosion);

        }

        /// <summary>
        /// Agrega una nueva explosion. Eligiendo la textura que se va a producir para la misma.
        /// </summary>        
        /// <param name="name">
        /// Carga el nombre de la lista.
        /// </param>
        /// <param name="posicion">
        /// La posición que tiene que contener la nueva explosión.
        /// </param>
        public void NewExplosion(string name , Vector2 posicion)
        {
            Explosion newexplosion = this.Find(name);

            if (newexplosion != null)
            {
                Explosion explosion = (Explosion)newexplosion.Clone();

                explosion.Posicion = posicion;

                m_explosionesActivas.Add(explosion);
            }                            
        }

        /// <summary>
        /// Método para la Giga Explosion
        /// </summary>
        public void NewExplosion()
        {
            Explosion gigaExplosion = this.Find("GigaExplosion");

            if (gigaExplosion != null)
            {
                Explosion exp = (Explosion)gigaExplosion.Clone();

                //exp.Posicion = new Vector2(DesignOptions.Bounds.MaxX / 2, DesignOptions.Bounds.MaxY / 2);

                exp.Escalado = new Vector2(10, 10);

                m_explosionesActivas.Add(exp);
            }
        }

        /// <summary>
        /// Dibuja una explosión al azar.
        /// </summary>
        /// <param name="sprite"></param>
        public void Draw(SpriteBatch sprite)
        {
            foreach (Explosion explosion in m_explosionesActivas)
            {
                explosion.Draw(sprite);
            }
        }

        public void Update(TimeSpan elapsedtime)
        {
            List<Explosion> explosiontoremove = new List<Explosion>();

            foreach (Explosion explosion in m_explosionesActivas)
            {
                if (explosion.FinishExplosion == false)
                {
                    explosion.Update(elapsedtime);
                }
                else
                {
                    explosiontoremove.Add(explosion);
                }
            }

            foreach (Explosion explosion in explosiontoremove)
            {
                m_explosionesActivas.Remove(explosion);
                explosion.Dispose();
            }            
        }

        public new void Clear()
        {
            m_explosionesActivas.Clear();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            base.Clear();
        }

        ~Explosiones()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
