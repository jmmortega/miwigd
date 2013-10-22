/*
 * Creador : Chema
 * Fecha Creacion: 28/06/2012
 * Notas
 * 
 * �nemigo rect�ngulo naranja.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creaci�n de la clase.
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
    public class RectanguloNaranja : Enemigo
    {
        public RectanguloNaranja(Texture2D textura, string name, Bounds bounds)
            : base(textura, name, bounds)
        {
            ///TODO: Terminar la l�gica del rectangulo naranja, similar a la del tri�ngulo amarillo. 
            ///Recordar que este enemigo, est� pensado para que salga por arriba de la pantalla y por abajo, por lo 
            ///que la l�gica de disparo difiere dependiendo de su posici�n, lo mismo ocurre con el patr�n de movimiento.
            ///Teniendo en cuenta que la posici�n se le va a pasar por propiedad, sobrecargar la propiedad Posicion

            base.ShootLogic = new Shoots.ShootLogic(10, 0.2f);

            base.StartShooting();

            base.Points = 500;

            base.Shield = 5;

            
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

        /// <summary>
        /// Agrega el patr�n de movimiento y el de disparo seg�n la posici�n indicada.
        /// </summary>
        private void SetPatron()
        {

            Vector4 patron = new Vector4(0, 0, 0, 0);

            //Marcamos el patr�n de movimiento 
                //Si es mayor a la mitad va hacia la izquierda
            if (base.Posicion.X > base.Bounds.MaxX / 2)
            {                
                patron = new Vector4(-5, 0, 1000, 0);                        
            }
                //Si es menor a la mitad va hacia la derecha
            else if (base.Posicion.X < base.Bounds.MaxX / 2)
            {
                patron = new Vector4(5, 0, 1000, 0);
            }

            base.PatronesMovimiento.Push(new MovementPatron(patron, new ExitBounds(false, false, false, false)));

            //Marcamos el patr�n de disparo.
                //Va por arriba.
            if (base.Posicion.Y < base.Bounds.MaxY / 2)
            {
                base.TypeShot = new Disparo1(ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds , true);   
            }
                //Va por abajo
            else if (base.Posicion.Y > base.Bounds.MaxY / 2)
            {
                base.TypeShot = new Disparo1(ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds, false);   
            }
        }



    }
}
