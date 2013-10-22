/*
 * Creador : Chema 
 * Fecha Creacion: 10/07/2012
 * Notas
 * 
 * Enemigo triangulo naranja
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Utils;

using StarPaper.Class.Objects.Shoots.ListaDisparos;
using StarPaper.Utils;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class TrianguloNaranja : Enemigo
    {
        public TrianguloNaranja(Texture2D textura, string name, Bounds bounds)
            :base(textura , name , bounds)
        {
            base.Shield = 5;

            base.ShootLogic = new Shoots.ShootLogic(5, 0.017f);

            ///Primer baja y luego tira pa un lado al azar. Como es un pila tengo que añadir primero el último 
            ///movimiento para que (FIFO)
            
                        
            base.PatronesMovimiento.Push(
                new Events.MovementPatron(
                    new Vector4(
                        Azar.Instance.GetNumber(-5, 5 , -1 , 1), 
                        Azar.Instance.GetNumber(0, 5),
                        1000, 1000),
                    new Events.ExitBounds(false, false, false, false)));

            base.PatronesMovimiento.Push(
                new Events.MovementPatron(
                    new Vector4(0,0,50,50),
                    new Events.ExitBounds(false,false,false,false)));

            base.PatronesMovimiento.Push(
                new Events.MovementPatron(
                    new Vector4(0, 10, 0, 20),
                    new Events.ExitBounds(false, false, false, false)));
            

            base.StartShooting();

            base.TypeShot = new Disparo1(ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds , true , 10);
            
        }

        public override void Shot()
        {
            if (base.PatronesMovimiento.Count == 1)
            {
                base.Shot();
            }
            
        }
    }
}

