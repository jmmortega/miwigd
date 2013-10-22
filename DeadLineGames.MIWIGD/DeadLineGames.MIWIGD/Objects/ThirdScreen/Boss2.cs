/*
 * Creador : Chema
 * Fecha Creacion: 11/07/2012
 * Notas
 * 
 * Clase para el Boss final de la fase 2
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
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;

using StarPaper.Class.Events;
using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Shoots.ListaDisparos;
using StarPaper.Utils;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class Boss2 : Enemigo , IBoss
    {
        #region Patrones de movimiento

        private MovementPatron m_patronIzquierda = new MovementPatron(
            new Vector4(-7,7,25,25) , new ExitBounds(false,false,false,false));

        private MovementPatron m_patronIzquierdaVuelta = new MovementPatron(
            new Vector4(7,-7,25,25) , new ExitBounds(false,false,false,false));

        private MovementPatron m_patronCentro = new MovementPatron(
            new Vector4(0,7,0,25) , new ExitBounds(false,false,false,false));

        private MovementPatron m_patronCentroVuelta = new MovementPatron(
            new Vector4(0,-7,0,25) , new ExitBounds(false,false,false,false));

        private MovementPatron m_patronDerecha = new MovementPatron(
            new Vector4(7, 7, 25, 25), new ExitBounds(false, false, false, false));

        private MovementPatron m_patronDerechaVuelta = new MovementPatron(
            new Vector4(-7, -7, 25, 25), new ExitBounds(false, false, false, false));

        private EnumMovement m_movimientoActual = EnumMovement.Izquierda;

        private bool m_vuelta = false;

        private float m_waitToMovement = 0.25f;

        #endregion

        public Boss2(Texture2D textura, string name, Bounds bounds)
            : base(textura, name, bounds)
        {
            base.Shield = 100;

            base.IsFinalBoss = true;

            base.PatronesMovimiento.Push(new Events.MovementPatron(
                new Vector4(0, 1, 0, 250), new Events.ExitBounds(false, false, false, false)));

            base.TypeShot = new Boss2Disparo1
                (ShootTextures.Instance["Disparo1Boss2"], "DisparoLaser", base.Bounds);

            base.ShootLogic = new ShootLogic(100, 0);

            base.Points = 1500;

        }


        #region Methods

        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);

            ///Lanzamos un nuevo patron de movimiento.
            if (base.PatronActual == null && base.PatronesMovimiento.Count == 0 && m_waitToMovement <= 0)
            {                
                if (m_movimientoActual == EnumMovement.Izquierda)
                {
                    if (m_vuelta == false)
                    {
                        base.PatronesMovimiento.Push((MovementPatron)m_patronIzquierda.Clone());
                    }
                    else
                    {
                        base.PatronesMovimiento.Push((MovementPatron)m_patronIzquierdaVuelta.Clone());
                    }
                }
                else if (m_movimientoActual == EnumMovement.None)
                {
                    if (m_vuelta == false)
                    {
                        base.PatronesMovimiento.Push((MovementPatron)m_patronCentro.Clone());
                    }
                    else
                    {
                        base.PatronesMovimiento.Push((MovementPatron)m_patronCentroVuelta.Clone());
                    }
                }
                else if (m_movimientoActual == EnumMovement.Derecha)
                {
                    if (m_vuelta == false)
                    {
                        base.PatronesMovimiento.Push((MovementPatron)m_patronDerecha.Clone());
                    }
                    else
                    {
                        base.PatronesMovimiento.Push((MovementPatron)m_patronDerechaVuelta.Clone());
                    }
                }

                m_vuelta = !m_vuelta;
                ChangeMovement();
                m_waitToMovement = 1f;
                base.ShootLogic = new ShootLogic(100, 0);
                base.StartShooting();
                
            }
            else if(base.PatronActual == null && base.PatronesMovimiento.Count == 0)
            {
                m_waitToMovement -= (float)elapsed.TotalSeconds;
            }
            
        }

        private void ChangeMovement()
        {
            if (m_movimientoActual == EnumMovement.Izquierda)
            {
                m_movimientoActual = EnumMovement.None;
            }
            else if (m_movimientoActual == EnumMovement.None)
            {
                m_movimientoActual = EnumMovement.Derecha;
            }
            else if (m_movimientoActual == EnumMovement.Derecha)
            {
                m_movimientoActual = EnumMovement.Izquierda;
            }
        }

        public override void Shot()
        {
            
            //Reutilizamos el disparo de laser del primer boss
            if(PatronActual == null)
            {
                Boss2Disparo1 laserabajo = (Boss2Disparo1)base.TypeShot.Clone();
                Boss2Disparo1 laserizq = (Boss2Disparo1)base.TypeShot.Clone();
                Boss2Disparo1 laserdrcha = (Boss2Disparo1)base.TypeShot.Clone();
                Boss2Disparo1 laserarriba = (Boss2Disparo1)base.TypeShot.Clone();

                laserabajo.DireccionDisparo = EnumMovement.Abajo;
                laserizq.DireccionDisparo = EnumMovement.Izquierda;
                laserdrcha.DireccionDisparo = EnumMovement.Derecha;
                laserabajo.DireccionDisparo = EnumMovement.Abajo;


                laserabajo.Posicion = new Vector2(base.Posicion.X + base.Width / 2, base.Posicion.Y + base.Height);
                laserizq.Posicion = new Vector2(base.Posicion.X, base.Posicion.Y + base.Height / 2);
                laserdrcha.Posicion = new Vector2(base.Posicion.X + base.Width, base.Posicion.Y + base.Height / 2);
                laserarriba.Posicion = new Vector2(base.Posicion.X + base.Width / 2, base.Posicion.Y);
                
                Disparos.Instance.Add(laserarriba);
                Disparos.Instance.Add(laserizq);
                Disparos.Instance.Add(laserabajo);
                Disparos.Instance.Add(laserdrcha);
            }
            
            
        }

        #endregion
    }
}
