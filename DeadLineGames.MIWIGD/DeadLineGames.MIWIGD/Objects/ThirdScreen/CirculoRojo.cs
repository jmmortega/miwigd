/*
 * Creador : Chema
 * Fecha Creacion: 06/06/2012
 * Notas
 * 
 * Enemigo que es un circulo rojo
 * 
 * Este enemigo cae para abajo sin hacer daño.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase.
 * 
 * 1.1 14/06/2012
 * 
 * Se incluye dentro del constructor la cantidad de puntos que se obtiene por el enemigo.
 * 
 * 1.2 28/06/2012
 * 
 * Se introduce el método SetPatron para modificar el patrón de movimiento.
 * 
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
using StarPaper.Class.Events;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class CirculoRojo : Enemigo
    {
        public CirculoRojo(Texture2D textura, string name, Bounds bounds)
            :base(textura, name , bounds)
        {            
            base.ShootLogic = new Shoots.ShootLogic(0, 0);

            base.StartShooting();

            base.TypeShot = null;

            base.Points = 200;
            base.Shield = 2;
           
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

        public void SetPatron()
        {
            Vector4 patron;

            if (base.Posicion.Y < base.Bounds.MaxY / 2)
            {
                patron = new Vector4(0, 3, 0, 1000);
            }
            else
            {
                patron = new Vector4(0, -3, 0, 1000);
            }

            base.PatronesMovimiento.Push
                (new Events.MovementPatron(
                    patron,
                    new Events.ExitBounds(false, false, false, false)));
        }
    }
}
