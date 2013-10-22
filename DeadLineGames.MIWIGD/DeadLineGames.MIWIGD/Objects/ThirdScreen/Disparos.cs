/*
 * Creador : Chema
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * Manejador de los disparos que se realizan en el juego.
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
 * 
 * 1.1 06/06/2012
 * 
 * Se incluye el método RemoveDisposed similar al de AObjects
 * 
 * 1.2 30/06/2012
 * 
 * Se realiza distinción entre listas, los disparos se van a añadir a una lista de disparos general y luego a otra para disparos de buenos y disparos de malos.
 *      Se sobrecarga el método Add
 *      
 * 1.3 02/07/2012
 * 
 * Se realiza la sobrecarga del método Clear para el limpiado de las listas de DisparosBuenos y DisparosMalos.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Utils.Collections;
using NamoCode.Game.Class.Exceptions;

namespace StarPaper.Class.Objects.Shoots
{
    
    /// <remarks>
    /// 06/06/2012
    /// Posteriormente se te puede ocurrir la idea de que este objeto herede de AObjects, como se me ha ocurrido ahora mismo. La razón es no. Se está realizando
    /// Singleton sobre el mismo, por lo tanto requiere de una instancia y es un coñazo en objetos complejos.
    /// Ya sé que en el fondo no deja tener implementado lo que tiene AObjects. 
    /// </remarks>
    public class Disparos : MyListElements<Disparo> , IElements<Disparo>
    {
        #region Singleton

        protected Disparos()
        { }

        private static Disparos m_instance = null;

        public static Disparos Instance
        {
            get 
            {
                if (m_instance == null)
                {
                    m_instance = new Disparos();
                }
                return Disparos.m_instance; 
            }            
        }

        #endregion

        #region Fields

        private MyListElements<Disparo> m_disparoBuenos = new MyListElements<Disparo>();

        public MyListElements<Disparo> DisparoBuenos
        {
            get { return m_disparoBuenos; }
            set { m_disparoBuenos = value; }
        }

        private MyListElements<Disparo> m_disparoMalos = new MyListElements<Disparo>();

        public MyListElements<Disparo> DisparoMalos
        {
            get { return m_disparoMalos; }
            set { m_disparoMalos = value; }
        }

        #endregion

        #region Methods

        public new void Add(Disparo disparo)
        {
            base.Add(disparo);

            if (disparo is IGoodGuyShoot)
            {
                m_disparoBuenos.Add(disparo);
            }
            else
            {
                m_disparoMalos.Add(disparo);
            }

        }

        public void DoMovement()
        {
            foreach (Disparo disparo in this)
            {
                disparo.DoMovement();
            }
        }

        public void RemoveDisposed()
        {
            MyList<Disparo> disparos = new MyList<Disparo>();

            foreach (Disparo shot in this)
            {
                if (shot.Disposed == true)
                {
                    disparos.Add(shot);
                }
            }

            foreach (Disparo shot in disparos)
            {
                this.Remove(shot);

                if (shot is IGoodGuyShoot)
                {
                    m_disparoBuenos.Remove(shot);
                }
                else
                {
                    m_disparoMalos.Remove(shot);
                }
            }
        }
        
        #endregion


        #region IElements

        public void Draw(SpriteBatch sprite)
        {
            foreach (Disparo disparo in this)
            {
                disparo.Draw(sprite);
            }
        }

        public Disparo GetElement(string nameelement)
        {
            Disparo elemento = this.Find(nameelement);

            if (elemento != null)
            {
                return elemento;
            }
            else
            {
                throw new NotIncludedInList();
            }      
        }

        public string Remove(int idelement)
        {
            string namereturned = string.Empty;

            if (idelement < this.Count())
            {
                namereturned = this[idelement].Name;

                this.RemoveAt(idelement);
            }

            return namereturned;
        }

        public string Remove(string namelement)
        {
            List<Disparo> elements = this.Contains(namelement);

            string namereturned = elements[elements.Count - 1].Name;

            this.Remove(elements[elements.Count() - 1]);

            return namereturned;  
        }

        public List<string> ToListNames()
        {
            List<string> names = new List<string>();

            foreach (var elemento in this)
            {
                names.Add(elemento.Name);
            }

            return names;   
        }

        public void Update(TimeSpan elapsed)
        {
            foreach (Disparo disparo in this)
            {
                disparo.Update(elapsed);
            }
        }

        public Disparo GetChecked(int x, int y)
        {
            throw new NotImplementedException("Este elemento no es necesario implementarlo en esta clase.");
        }

        public new void Clear()
        {
            m_disparoBuenos.Clear();
            m_disparoMalos.Clear();
            base.Clear();
        }

        #endregion


        public int GetId(string element)
        {
            throw new NotImplementedException();
        }

        public void Update(TimeSpan elapsed, Microsoft.Xna.Framework.Vector2 position)
        {
            throw new NotImplementedException();
        }
    }
}
