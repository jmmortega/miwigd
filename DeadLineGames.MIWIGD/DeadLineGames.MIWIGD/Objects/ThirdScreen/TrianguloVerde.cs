/*
 * Creador : Chema 
 * Fecha Creacion: 10/07/2012
 * Notas
 * 
 * Enemigo de tipo triangulo verde con un movimiento en el eje X seguido por uno del eje Y
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

using StarPaper.Class.Objects.Enemies;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class TrianguloVerde : Enemigo
    {

        //TODO: Recalcular las colisiones cuando el ángulo se ha visto modificado

        private float m_grados = 75;

        private const int c_Velocidad = 4;

        public TrianguloVerde(Texture2D textura, string name, Bounds bounds)
            : base(textura, name, bounds)
        {
            base.Shield = 5;
            base.Points = 500;
        }

        public TrianguloVerde(Texture2D textura, string name, Bounds bounds , float grados)
            :base(textura , name , bounds)
        {
            base.Shield = 5;
            base.Points = 500;
            m_grados = grados;
            
        }
        
        #region Fields

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

        #endregion

        #region Methods

        protected EnumCuadrante SetPatron()
        {
            Vector4 patron;

            Vector2 movimiento = GodClass.CalcVectorToAngle(MathHelper.ToRadians(m_grados)) * c_Velocidad;

            EnumCuadrante cuadrante = EnumCuadrante.None;

            //Sale del cuadrante superior izquierda.
            if (base.Posicion.X < base.Bounds.MaxX / 2 && base.Posicion.Y < base.Bounds.MaxY / 2)
            {
                patron = new Vector4(movimiento.X , movimiento.Y, 1000, 1000);
                base.Rotation = -m_grados;
                cuadrante = EnumCuadrante.SuperiorIzquierda;
            }
            //Sale del cuadrante superior derecha
            else if (base.Posicion.X > base.Bounds.MaxX / 2 && base.Posicion.Y < base.Bounds.MaxY / 2)
            {
                patron = new Vector4(-movimiento.X, movimiento.Y, 1000, 1000);
                base.Rotation = m_grados;
                cuadrante = EnumCuadrante.SuperiorDerecha;
            }
            //Sale del cuadrante inferior izquierda
            else if (base.Posicion.X < base.Bounds.MaxX / 2 && base.Posicion.Y > base.Bounds.MaxY / 2)
            {
                patron = new Vector4(movimiento.X, -movimiento.Y, 1000, 1000);
                base.Rotation = m_grados + 180;
                cuadrante = EnumCuadrante.InferiorIzquierda;
            }
            //Sale del cuadrante inferior derecha
            //Lo he marcado con else y no else if porque no quedan más posibilidades y para que no me marque advertencia de unused el Vector4
            else
            {
                patron = new Vector4(-movimiento.X, -movimiento.Y, 1000, 1000);
                base.Rotation = 180 - m_grados;
                cuadrante = EnumCuadrante.InferiorDerecha;
            }

            base.PatronesMovimiento.Push
                    (new Events.MovementPatron(
                        patron,
                        new Events.ExitBounds(false, false, false, false)));

            return cuadrante;

        }
        
        #endregion



    }
}
