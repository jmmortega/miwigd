/*
 * Creador : Chema 
 * Fecha Creacion: 14/06/2012
 * Notas
 * 
 * Estructura que incluye información respeto al hub.
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

namespace StarPaper.Class.Events.Director
{
    public struct HubState
    {
        #region Constructor

        public HubState(long points, int shield, int lifes)
        {
            m_points = points;
            m_shield = shield;
            m_lifes = lifes;
        }

        #endregion

        #region Fields

        private long m_points;

        public long Points
        {
            get { return m_points; }
            set { m_points = value; }
        }

        private int m_shield;

        public int Shield
        {
            get { return m_shield; }
            set { m_shield = value; }
        }

        private int m_lifes;

        public int Lifes
        {
            get { return m_lifes; }
            set { m_lifes = value; }
        }

        #endregion

    }
}
