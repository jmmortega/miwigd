/*
 * Creador : Chema
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * Clase explosión hija de AnimatedElement
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase.
 * 
 * 1.1 16/05/2012
 * 
 * Se añade la capacidad de clonarse.
 * 
 * 1.2 29/06/2012
 * 
 * Se modifica el constructor de Explosion 
 *      Anteriormente (Texture2D , string , Vector2 , FrameRateInfo)
 *      Ahora (Texture2D , string , FrameRateInfo)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;

namespace StarPaper.Class.Design
{
    public class Explosion : AnimatedElement , IMyCloneable
    {
        public Explosion(Texture2D textura, string name , FrameRateInfo frames)
            : base(textura, name, new Vector2(0,0), frames)
        { }

        public Explosion(Texture2D textura, string name, FrameRateInfo frames, float Scale)
            :base(textura, name , new Vector2(0,0) , frames)
        {
            base.Escalado = new Vector2(Scale, Scale);
        }
        
        private bool m_finishExplosion = false;

        /// <summary>
        /// Determina si la animación de la explosión a finalizado.
        /// </summary>
        public bool FinishExplosion
        {
            get { return m_finishExplosion; }
            set { m_finishExplosion = value; }
        }

        public override void Update(TimeSpan elapsedtime)
        {
            if (base.Frames.ActualFrame == base.Frames.FrameCount -1)
            {
                m_finishExplosion = true;
            }
            base.Update(elapsedtime);
        }

        public object Clone()
        {
            return new Explosion(base.Textura, base.Name, base.Frames);
        }
    }
}
