/*
 * Creador : Chema
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * Indica la lógica de los disparos de las naves enemigas.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la estructura.
 * 
 * 1.1 10/07/2012
 * 
 * Se retira la reducción de balas del método HaveShot. 
 * Hay veces que se hacen comprobaciones adicionales para el disparo, por lo que se debe restar de Shot
 * Se agrega un método llamado ChangeAmount
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarPaper.Class.Objects.Shoots
{
    public struct ShootLogic
    {
        public ShootLogic(int amount, float cadencia)
        {
            m_amount = amount;
            m_cadencia = cadencia;
            m_currentTime = m_cadencia;
            m_shooting = false;
        }
        
        private int m_amount;

        /// <summary>
        /// Cantidad de disparos lanzados.
        /// En caso de que el valor sea negativo, los disparos serán infinitos.
        /// </summary>
        public int Amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }

        private float m_cadencia;

        /// <summary>
        /// El tiempo entre disparo y disparo.
        /// </summary>
        public float Cadencia
        {
            get { return m_cadencia; }
            set { m_cadencia = value; }
        }

        private float m_currentTime;

        private bool m_shooting;

        /// <summary>
        /// Comienza los disparos.
        /// </summary>
        public void Start()
        {
            m_shooting = true;
        }

        /// <summary>
        /// Para los disparos
        /// </summary>
        public void Stop()
        {
            m_shooting = false;
        }

        /// <summary>
        /// Determina si se realiza un nuevo disparo.
        /// </summary>
        /// <param name="elapsedtime"></param>
        /// <returns>
        /// Devuelve true en caso de que se produzca un disparo.
        /// </returns>
        public bool HaveShot(float elapsedtime)
        {            
            if (m_shooting == true)
            {
                if (m_amount != 0)
                {
                    m_currentTime -= elapsedtime;

                    ///Utilizo los milisegundos en vez de los segundos, porque será más común que el disparo se produzca en fracciones muy breves.
                    if (m_currentTime <= 0)
                    {
                        m_currentTime = m_cadencia;                        
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Stop(); //Si no queda munición paramos de realizar disparos.
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void ChangeAmount(int value)
        {
            m_amount = m_amount + value;
        }
    }
}
