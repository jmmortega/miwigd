/*
 * Creador : Chema 
 * Fecha Creacion: 17/05/2012
 * Notas
 * 
 * Creación de primer enemigo llamado TrianguloAmarillo
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * 1.1 06/06/2012
 * 
 * Se agrega constructor para implementar mediante el EnemiesListHelper.
 * 
 * 1.2 14/06/2012
 * 
 * Se incluye los puntos que da el enemigo.
 * Se incluye los valores de shield de los enemigos.
 * 
 * 1.3 28/06/2012
 * 
 * Se agrega el valor para SetPatron, para que el patrón de movimiento se modifique pediendo de donde salga la figura.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Utils;

using StarPaper.Utils;
using StarPaper.Class.Objects.Shoots.ListaDisparos;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class TrianguloAmarillo : Enemigo
    {
        [Obsolete("Este constructor ya no se usa. Debido a que ahora se instancian los enemigos mediante el helper")]
        public TrianguloAmarillo(Texture2D textura , string name , Bounds bounds , Vector2 posicion)
            :base(textura , name , bounds)
        {
            base.Posicion = posicion;

            base.PatronesMovimiento.Push
                (new Events.MovementPatron(
                    new Vector4(5, 2, 1000, 1000),
                    new Events.ExitBounds(true, true, true, true)));

            base.ShootLogic = new Shoots.ShootLogic(10, 1);

            base.StartShooting();

            base.TypeShot = new Disparo1(ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds , true);            
        }

        /// <summary>
        /// No se instancia la posición porque está es instanciada desde el helper.
        /// </summary>
        /// <param name="textura"></param>
        /// <param name="name"></param>
        /// <param name="bounds"></param>
        public TrianguloAmarillo(Texture2D textura, string name, Bounds bounds)
            :base(textura , name , bounds)
        {                        
            base.ShootLogic = new Shoots.ShootLogic(10, 1);

            base.StartShooting();

            base.TypeShot = new Disparo1(ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds , true);

            base.Points = 400;
            base.Shield = 4;
        }

        public override Vector2 Posicion
        {
            get
            {
               return base.Posicion;
            }
            set
            {
                base.Posicion = value;
                SetPatron();
            }
        }

        private void SetPatron()
        {
            Vector4 patron;

                //Sale del cuadrante superior izquierda.
            if (base.Posicion.X < base.Bounds.MaxX / 2 && base.Posicion.Y < base.Bounds.MaxY / 2)
            {
                patron = new Vector4(5, 2, 1000, 1000);
            }
                //Sale del cuadrante superior derecha
            else if (base.Posicion.X > base.Bounds.MaxX / 2 && base.Posicion.Y < base.Bounds.MaxY / 2)
            {
                patron = new Vector4(-5, 2, 1000, 1000);                 
            }
                //Sale del cuadrante inferior izquierda
            else if (base.Posicion.X < base.Bounds.MaxX / 2 && base.Posicion.Y > base.Bounds.MaxY / 2)
            {
                patron = new Vector4(5, -2, 1000, 1000);
            }
                //Sale del cuadrante inferior derecha
                    //Lo he marcado con else y no else if porque no quedan más posibilidades y para que no me marque advertencia de unused el Vector4
            else
            {
                patron = new Vector4(-5, -2, 1000, 1000);
            }

            base.PatronesMovimiento.Push
                    (new Events.MovementPatron(
                        patron,
                        new Events.ExitBounds(true, true, true, true)));
        }
    }
}
