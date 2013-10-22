/*
 * Creador : Chema 
 * Fecha Creacion: 18/05/2012
 * Notas
 * 
 * Clase que trata un elemento de storyboard de tipo bonus.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StarPaper.Class.Events.Director.StoryBoard
{
    public class StoryBoardEventBonus : AStoryBoardEvent
    {

        public StoryBoardEventBonus(float second, float quantity, string name, Vector2 posicion, float delay)
            : base(second, quantity, name, posicion, delay)
        { }
    }
}
