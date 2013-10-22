/*
 * Creador : Chema 
 * Fecha Creacion: 17/05/2012
 * Notas
 * 
 * Creación de primer disparo llamado Disparo1
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * 1.1 14/06/2012
 * 
 * Se agrega el daño por disparo.
 * 
 * 1.2 28/06/2012
 * 
 * Se agrega un nuevo constructor para el disparo para indicar el patrón de movimiento del mismo. Si se va a realizar de arriba a abajo o de abajo a arriba.
 * 
 * 1.3 10/07/2012
 * 
 * Se añade un nuevo constructor que determina la velocidad del disparo.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Utils;

namespace StarPaper.Class.Objects.Shoots.ListaDisparos
{
    public class Disparo1 : Disparo
    {
        [Obsolete("Este constructor se encuentra obsoleto, es más eficiente usar (Texture,string,Bounds,bool)")]
        public Disparo1(Texture2D textura, string name, Bounds bounds)
            : base(textura, name, bounds)
        {
            base.PatronesMovimiento.Push
                (new Events.MovementPatron(
                    new Vector4(0, m_velocidad, 0, 1000),
                    new Events.ExitBounds(false, false, false, false)));

            base.Damage = 1;


        }

        private int m_velocidad = 3;
        private bool m_arriba = false;

        /// <summary>         
        /// </summary>
        /// <param name="textura"></param>
        /// <param name="name"></param>
        /// <param name="bounds"></param>
        /// <param name="arriba">
        /// Comienza el disparo desde arriba hacia abajo si el valor es true.
        /// </param>
        public Disparo1(Texture2D textura, string name, Bounds bounds , bool arriba)
            :base(textura , name , bounds)
        {

            Vector4 vector;

            m_arriba = arriba;

            if (arriba == true)
            {
                vector = new Vector4(0, m_velocidad, 0, 1000);
            }
            else
            {
                vector = new Vector4(0, -m_velocidad, 0, 1000);
            }

            base.PatronesMovimiento.Push
                   (new Events.MovementPatron(
                        vector,
                        new Events.ExitBounds(false, false, false, false)));

            base.Damage = 1;

            
        }

        public Disparo1(Texture2D textura, string name, Bounds bounds, bool arriba, int velocidad)
            :base(textura , name , bounds)
        {
            Vector4 vector;

            m_velocidad = velocidad;

            m_arriba = arriba;

            if (arriba == true)
            {
                vector = new Vector4(0, m_velocidad, 0, 1000);
            }
            else
            {
                vector = new Vector4(0, -m_velocidad, 0, 1000);
            }

            base.PatronesMovimiento.Push
                   (new Events.MovementPatron(
                        vector,
                        new Events.ExitBounds(false, false, false, false)));

            base.Damage = 1;
        }

        public override object Clone()
        {
            return new Disparo1(base.Texture, base.Name, base.Bounds, this.m_arriba , this.m_velocidad);
        }
    }
}
