/*
 * Creador : Chema 
 * Fecha Creacion: 10/07/2012
 * Notas
 * 
 * Enemigo de tipo Triangulo Rosa, con las capacidades de movimiento idénticas a la del triangulo verde, pero con el añadido de que realiza disparos.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase
 *  
 * 
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
    public class TrianguloRosa : TrianguloVerde
    {
        public TrianguloRosa(Texture2D textura, string name, Bounds bounds)
            :base(textura , name , bounds)
        {
            base.Points = 700;   
        }

        public TrianguloRosa(Texture2D textura, string name, Bounds bounds, float grados)
            : base(textura, name, bounds, grados)
        {
            base.Points = 700;
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
                SetShot(SetPatron());
            }
        }

        private void SetShot(EnumCuadrante cuadrante)
        {
            if(cuadrante == EnumCuadrante.SuperiorDerecha || cuadrante == EnumCuadrante.SuperiorIzquierda)
            {
                base.TypeShot = new Disparo1(ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds , true);
            }
            else
            {
                base.TypeShot = new Disparo1(ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds , false);
            }


            base.ShootLogic = new Shoots.ShootLogic(10, 0.5f);

            base.StartShooting();
        }
    }
}
