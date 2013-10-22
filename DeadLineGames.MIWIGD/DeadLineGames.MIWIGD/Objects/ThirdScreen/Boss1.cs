/*
 * Creador : Chema
 * Fecha Creacion: 06/07/2012
 * Notas
 * 
 * Clase para el Boss final de la fase 1
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

using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Shoots.ListaDisparos;
using StarPaper.Utils;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class Boss1 : Enemigo , IBoss
    {
        public Boss1(Texture2D texture, string name, Bounds bounds)
            : base(texture, name, bounds)
        {
            base.Shield = 30;

            base.Points = 1000;

            ShotActual = true;

            base.StartShooting();

            base.PatronesMovimiento.Push(new Events.MovementPatron(
                new Vector4(0, 1, 0, 200), new Events.ExitBounds(false, false, false, false)));

            base.IsFinalBoss = true;
        }


        private const float c_PosLaser1X = 15;
        private const float c_PosLaser1Y = 157;

        private const float c_PosLaser2X = 110;
        private const float c_PosLaser2Y = 157;

        /// <summary>
        /// Si el valor esta en false, será el rayo y si está en true será el disparo normal.
        /// </summary>
        private bool m_shotActual = false;

        protected bool ShotActual
        {            
            set 
            {
                m_shotActual = value;
                if (value == true)
                {
                    base.ShootLogic = new ShootLogic(1000, 0.3f);
                    base.StartShooting();
                }
                else
                {
                    base.ShootLogic = new ShootLogic(1000, 0);
                    base.StartShooting();
                }
            }
        }

        public override void Shot()
        {
            if (m_shotActual == true)
            {
                base.TypeShot = new Boss1Disparo1(
                    ShootTextures.Instance["Disparo1Boss1"], "Disparo1Boss1", DesignOptions.Bounds);

                Boss1Disparo1 disparo = (Boss1Disparo1)base.TypeShot.Clone();
                Boss1Disparo1 disparo2 = (Boss1Disparo1)base.TypeShot.Clone();

                ShotActual = !disparo.ChangePatron(false);

                disparo2.ChangePatron(true);

                disparo.Posicion = new Vector2(base.Posicion.X + base.Width / 2, base.Posicion.Y + base.Height / 2);
                disparo2.Posicion = new Vector2(base.Posicion.X + base.Width / 2, base.Posicion.Y + base.Height / 2);

                Disparos.Instance.Add(disparo);
                Disparos.Instance.Add(disparo2);
                
            }
            else
            {
                base.TypeShot = new Boss1Disparo2(
                    ShootTextures.Instance["Disparo2Boss1"], "Disparo2Boss1", DesignOptions.Bounds);

                Disparo laser1 = (Disparo)base.TypeShot.Clone();

                Disparo laser2 = (Disparo)base.TypeShot.Clone();

                laser1.Posicion = new Vector2(base.Posicion.X + c_PosLaser1X, base.Posicion.Y + c_PosLaser1Y);
                laser2.Posicion = new Vector2(base.Posicion.X + c_PosLaser2X, base.Posicion.Y + c_PosLaser2Y);

                Disparos.Instance.Add(laser1);
                Disparos.Instance.Add(laser2);

            }            
        }

        /// <summary>
        /// Tiempo que dura el disparo por laser.
        /// </summary>
        private float m_currentLaser = 2f;

        public override void Update(TimeSpan elapsed)
        {
            if (m_shotActual == false)
            {
                m_currentLaser -= (float)elapsed.TotalSeconds;

                if (m_currentLaser <= 0)
                {
                    ShotActual = true;
                    m_currentLaser = 2f;
                }                
            }

            base.Update(elapsed);
        }


    }
}
