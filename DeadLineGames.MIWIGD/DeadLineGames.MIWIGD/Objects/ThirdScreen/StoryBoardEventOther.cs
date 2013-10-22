/*
 * Creador : Chema 
 * Fecha Creacion: 18/05/2012
 * Notas
 * 
 * Clase que trata un elemento de storyboard de tipo otro.
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

using NamoCode.Game.Class.Events;

namespace StarPaper.Class.Events.Director.StoryBoard
{
    public class StoryBoardEventOther : AStoryBoardEvent
    {
        public StoryBoardEventOther(float second, float quantity, string name, Vector2 posicion, float delay , IVerb verb)
            : base(second, quantity, name, posicion, delay)
        {
            m_verb = verb;
        }

        private IVerb m_verb;

        public IVerb Verb
        {
            get { return m_verb; }
            set { m_verb = value; }
        }

    }
}
