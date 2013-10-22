/*
 * Creador : Chema 
 * Fecha Creacion: 18/05/2012
 * Notas
 * 
 * Clase abstracta que engloba las principales funcionalidades de los elementos de un storyboard.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase
 * 
 * 1.1 30/05/2012
 * 
 * Se integra el método de Equals dentro de la clase.
 * Se modifica el tipo de quantity de int a float.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StarPaper.Class.Events.Director.StoryBoard
{
    public abstract class AStoryBoardEvent
    {
        public AStoryBoardEvent(float second, float quantity, string name, Vector2 posicion, float delay)
        {
            m_second = second;
            m_quantity = quantity;
            m_name = name;
            m_posicion = posicion;
            m_delay = delay;

        }

        #region Fields

        private float m_second = 0;

        public float Second
        {
            get { return m_second; }
            set { m_second = value; }
        }

        private float m_quantity = 0;

        public float Quantity
        {
            get { return m_quantity; }
            set { m_quantity = value; }
        }

        private string m_name = string.Empty;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private Vector2 m_posicion;

        public Vector2 Posicion
        {
            get { return m_posicion; }
            set { m_posicion = value; }
        }

        private float m_delay;

        public float Delay
        {
            get { return m_delay; }
            set { m_delay = value; }
        }


        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            AStoryBoardEvent storyboardEvent = (AStoryBoardEvent)obj;

            if (this.Name == storyboardEvent.Name && this.Second == storyboardEvent.Second)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

    }
}
