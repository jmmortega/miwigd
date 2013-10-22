/*
 * Creador : Chema
 * Fecha Creacion: 06/06/2012
 * Notas
 * 
 * Los datos de la cabecera del storyboard.
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

namespace StarPaper.Class.Events.Director.StoryBoard
{
    public class StoryBoardHeader
    {

        #region Constructor

        public StoryBoardHeader(string scenarioname, float gametime)
        {
            if(m_scenarioName != "Null")
            {
                m_scenarioName = scenarioname;    
            }
            
            m_gametime = gametime;
        }

        #endregion

        #region Fields

        private string m_scenarioName = string.Empty;

        /// <summary>
        /// Nombre del escenario, normalmente el nombre de la textura que vamos a usar.
        /// </summary>
        public string ScenarioName
        {
            get { return m_scenarioName; }
            set { m_scenarioName = value; }
        }

        private float m_gametime = 0;

        /// <summary>
        /// El tiempo de juego expresado en segundos que durara la fase.
        /// </summary>
        /// <remarks>
        /// Una fase se compone por distintos eventos que finalmente terminan en un boss, siempre que haya un final boss se parara el tiempo de juego.
        /// </remarks>
        public float Gametime
        {
            get { return m_gametime; }
            set { m_gametime = value; }
        }

        #endregion
    }
}
