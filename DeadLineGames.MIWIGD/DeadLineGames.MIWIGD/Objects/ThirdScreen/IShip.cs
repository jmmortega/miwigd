/*
 * Creador : Chema
 * Fecha Creacion: 08/06/2012 
 * Notas
 * 
 * Interfaz que contiene las caracteristicas que comparten las naves
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
 * 
 * 1.1 12/06/2012
 * 
 * Se agrega el método Shot por Shot(float) Para introducir el valor de cadencia al disparo.
 * 
 * 1.2 14/06/2012
 * 
 * Se refactoriza el nombre de Life a Shield (Más adecuado)
*/

using System;

using StarPaper.Class.Objects.Shoots;

namespace StarPaper.Class.Objects
{
    interface IShip
    {
        float Shield { get; set; }

        Disparo TypeShot { get; set; }

        /// <summary>
        /// Realiza la muerte.
        /// </summary>
        void Kill();

        void Shot();

        /// <summary>
        /// Produce la acción de disparo.
        /// </summary>
        void Shot(float currentTime);
    }
}
