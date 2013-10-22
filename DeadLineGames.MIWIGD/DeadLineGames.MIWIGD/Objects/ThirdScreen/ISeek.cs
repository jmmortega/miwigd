/*
 * Creador :  Chema 
 * Fecha Creacion: 08/07/2012
 * Notas
 * 
 * Interfaz que indica los enemigos que tienen la capacidad de seguir al prota.
 * 
 * Cambios realizados.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StarPaper.Class.Objects.Enemies
{
    public interface ISeek
    {
        Vector2 SeekObject
        { get; set; }
    
    }
}
