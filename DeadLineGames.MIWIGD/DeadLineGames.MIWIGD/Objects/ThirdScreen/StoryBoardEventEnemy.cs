/*
 * Creador : Chema 
 * Fecha Creacion: 18/05/2012
 * Notas
 * 
 * Clase que trata un elemento de storyboard de tipo enemigo.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase
 * 
 * 1.1 06/07/2012
 * 
 * Se agrega una propiedad adicional al constructor, para indicar la dificultad. En SuperDificil, añadimos más vida a los enemigos, además de que los monstruos
 * finales tienen vida.
 * 
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace StarPaper.Class.Events.Director.StoryBoard
{
    public class StoryBoardEventEnemy : AStoryBoardEvent
    {
        public StoryBoardEventEnemy(float second , float quantity , string name , Vector2 posicion , float delay , bool final)
            :base(second , quantity , name , posicion , delay)
        {
            m_isFinalBoss = final;
        }

        public StoryBoardEventEnemy(float second, float quantity, string name, Vector2 posicion, float delay, bool final, float quantitylife)
            :base(second, quantity , name , posicion , delay)
        {
            m_AddLife = quantitylife;
            m_isFinalBoss = final;
        }


        private bool m_isFinalBoss = false;

        public bool IsFinalBoss
        {
            get { return m_isFinalBoss; }
            set { m_isFinalBoss = value; }
        }

        private float m_AddLife = 0;

        public float AddLife
        {
            get { return m_AddLife; }
            set { m_AddLife = value; }
        }
    }
}
