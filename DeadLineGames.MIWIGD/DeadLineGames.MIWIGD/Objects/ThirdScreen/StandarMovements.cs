/*
 * Creador : Chema 
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * Clase que agrupaa los movimientos standars de las distintas del juego. Está clase es estática para poder acceder a ella desde cualquier lugar.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase.
 * 
 * 1.1 17/05/2012
 * 
 * Se modifica los movimientos estandar y la clase se convierte en un ContentDictionary
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using NamoCode.Game.Utils.Collections;

namespace StarPaper.Class.Events
{
    public class StandarMovements 
    {

        #region ListMovements

        /// <summary>
        /// El movimiento del background escenario        
        /// </summary>
        public static Vector4 ScenarioMovement = new Vector4(0, 0, 99, 99);

        #endregion
    }
}
