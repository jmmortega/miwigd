/*
 * Creador : Chema 
 * Fecha Creacion: 12/07/2012
 * Notas
 * 
 * Final boss cuarto
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

using StarPaper.Class.Events;
using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Shoots.ListaDisparos;
using StarPaper.Utils;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class Boss4 : Enemigo , IBoss , ISeek
    {

        #region Movement

        private MovementPatron m_firstIzquierda = new MovementPatron(
            new Vector4(-3, 0, 25, 0), new ExitBounds(false, false, false, false));

        private MovementPatron m_izquierda = new MovementPatron(
            new Vector4(-3, 0, 75, 0), new ExitBounds(false, false, false, false));

        private MovementPatron m_derecha = new MovementPatron(
            new Vector4(3, 0, 75, 0), new ExitBounds(false, false, false, false));

        private EnumMovement m_movimiento = EnumMovement.Derecha;

        private bool m_first = true;

        private float m_waitToMovement = 1f;

        #endregion

        #region Fields

        private Vector2 m_seekObject;

        public Vector2 SeekObject
        {
            get
            {
                return m_seekObject;
            }
            set
            {
                m_seekObject = value;
            }
        }

        #endregion

        #region Constructor

        public Boss4(Texture2D texture, string name, Bounds bounds)
            :base(texture , name , bounds)
        {
            base.IsFinalBoss = true;

            base.Points = 2500;

            base.Shield = 150;

            base.PatronesMovimiento.Push(new Events.MovementPatron(
                new Vector4(0, 1, 0, 200), new Events.ExitBounds(false, false, false, false)));
            

            base.TypeShot = new DisparoMovil
                (ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds);

            base.ShootLogic = new Shoots.ShootLogic(10000, 0.5f);

            base.StartShooting();
        }

        #endregion

        #region Methods

        public override void Shot()
        {
            (base.TypeShot as DisparoMovil).SetMovement(GodClass.AngleBetween(m_seekObject, base.Posicion));

            base.Shot();

            Enemigo circulo = EnemiesListHelper.Instance["CirculoRojo"];

            circulo.Posicion = base.Posicion;

            circulo.Points = 0;

            InfoObjectsStarPaper.MalosTemp.Add(circulo);
        }

        public override void Update(TimeSpan elapsed)
        {
            if (m_first == true)
            {
                m_first = false;
                base.PatronesMovimiento.Push(m_firstIzquierda);
            }
            else
            {
                if (base.PatronActual == null && base.PatronesMovimiento.Count == 0 && m_waitToMovement <= 0)
                {
                    if (m_movimiento == EnumMovement.Izquierda)
                    {
                        base.PatronesMovimiento.Push((MovementPatron)m_izquierda.Clone());
                        m_movimiento = EnumMovement.Derecha;
                    }
                    else if (m_movimiento == EnumMovement.Derecha)
                    {
                        base.PatronesMovimiento.Push((MovementPatron)m_derecha.Clone());
                        m_movimiento = EnumMovement.Izquierda;
                    }
                    m_waitToMovement = 1f;
                }
                else if (base.PatronActual == null && base.PatronesMovimiento.Count == 0)
                {
                    m_waitToMovement -= (float)elapsed.TotalSeconds;
                }
            }
            base.Update(elapsed);
        }

        #endregion


       
    }
}
