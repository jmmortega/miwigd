/*
 * Creador : Chema 
 * Fecha Creacion: 08/07/2012
 * Notas
 * 
 * Almacena los valores para el prota. Escudo, vidas, bonus...
 * 
 * 
 * Cambios realizados.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarPaper.Class.Objects.Buenos
{
    public struct ProtaValues
    {
        #region Constructor

        public ProtaValues(ProtaValues values)
        {
            m_lifes = values.Lifes;
            m_shotDamage = values.ShotDamage;
            m_cadence = values.Cadence;
            m_numberShots = values.NumberShots;
            m_shield = values.Shield;
        }

        public ProtaValues(int lifes, float shotdamage, float cadence, int numberShots, float shield)
        {
            m_lifes = lifes;
            m_shotDamage = shotdamage;
            m_cadence = cadence;
            m_numberShots = numberShots;
            m_shield = shield;
        }

        #endregion

        #region Fields

        private int m_lifes;

        public int Lifes
        {
            get { return m_lifes; }
            set { m_lifes = value; }
        }

        private float m_shotDamage;

        public float ShotDamage
        {
            get { return m_shotDamage; }
            set { m_shotDamage = value; }
        }

        private float m_cadence;

        public float Cadence
        {
            get { return m_cadence; }
            set { m_cadence = value; }
        }

        private int m_numberShots;

        public int NumberShots
        {
            get { return m_numberShots; }
            set { m_numberShots = value; }
        }

        private float m_shield;

        public float Shield
        {
            get { return m_shield; }
            set { m_shield = value; }
        }

        #endregion
    }
}
