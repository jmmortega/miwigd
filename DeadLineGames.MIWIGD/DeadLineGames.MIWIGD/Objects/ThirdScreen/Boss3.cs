/*
 * Creador : Chema
 * Fecha Creacion: 11/07/2012
 * Notas
 * 
 * Enemigo final 3
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
using StarPaper.Class.Objects.Enemies;
using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Shoots.ListaDisparos;
using StarPaper.Utils;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class Boss3 : EnemigoParteMovil , IBoss
    {
        #region Movement

        private MovementPatron m_abajo = new MovementPatron(
            new Vector4(0, 2, 0, 225), new ExitBounds(false, false, false, false));

        private MovementPatron m_arriba = new MovementPatron(
            new Vector4(0, -3, 0, 150), new ExitBounds(false, false, false, false));

        private EnumMovement m_movimiento = EnumMovement.Abajo;

        #endregion

        #region Constructor

        public Boss3(Texture2D texture, Texture2D movepart, string name, Bounds bounds)
            : base(texture,movepart,name,bounds)
        {

            base.Shield = 200;

            base.IsFinalBoss = true;

            base.Points = 2000;

            base.PatronesMovimiento.Push(new Events.MovementPatron(
                new Vector4(0, 1, 0, 200), new Events.ExitBounds(false, false, false, false)));

            base.TypeShot = new DisparoMovil
                (ShootTextures.Instance["Disparo1"], "Disparo", base.Bounds);

            base.ShootLogic = new Shoots.ShootLogic(10000 , 1f);

            base.StartShooting();

        }

        #endregion

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

                CalcMovePartPosition(value);
            }
        }

        private Vector2 m_movepartPosition2;

        private Vector2 m_movepartPosition3;

        private Vector2 m_movepartPosition4;

        private float m_waitToMovement = 1f;
        
        #endregion

        #region Methods

        protected override Vector2 CalcMovePartPosition(Vector2 position)
        {
            float _6heigth = base.Height / 6;

            base.MovePartPosition = new Vector2(position.X + base.Width / 2, position.Y + _6heigth);
            m_movepartPosition2 = new Vector2(position.X + base.Width / 2, position.Y + (_6heigth * 2) + 10);
            m_movepartPosition3 = new Vector2(position.X + base.Width / 2, position.Y + (_6heigth * 3) + 15);
            m_movepartPosition4 = new Vector2(position.X + base.Width / 2, position.Y + (_6heigth * 5));
            
            return new Vector2(-1, -1);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            spritebatch.Begin();

            spritebatch.Draw(base.MovePart, m_movepartPosition2, new Rectangle(0, 0, base.MovePart.Width, base.MovePart.Height), Color.White,
                base.MovePartRotation, new Vector2(0, base.MovePart.Height / 2), 1, SpriteEffects.None, 1);

            spritebatch.Draw(base.MovePart, m_movepartPosition3, new Rectangle(0, 0, base.MovePart.Width, base.MovePart.Height), Color.White,
                base.MovePartRotation, new Vector2(0, base.MovePart.Height / 2), 1, SpriteEffects.None, 1);

            spritebatch.Draw(base.MovePart, m_movepartPosition4, new Rectangle(0, 0, base.MovePart.Width, base.MovePart.Height), Color.White,
                base.MovePartRotation, new Vector2(0, base.MovePart.Height / 2), 1, SpriteEffects.None, 1);

            spritebatch.End();

        }

        public override void Update(TimeSpan elapsed)
        {
            if (base.PatronActual == null && base.PatronesMovimiento.Count == 0 && m_waitToMovement <= 0)
            {
                if(m_movimiento == EnumMovement.Abajo)
                {
                    base.PatronesMovimiento.Push((MovementPatron)m_abajo.Clone());
                    m_movimiento = EnumMovement.Arriba;
                }
                else if(m_movimiento == EnumMovement.Arriba)
                {
                    base.PatronesMovimiento.Push((MovementPatron)m_arriba.Clone());
                    m_movimiento = EnumMovement.Abajo;
                }
                m_waitToMovement = 1f;
            }
            else if (base.PatronActual == null && base.PatronesMovimiento.Count == 0)
            {
                m_waitToMovement -= (float)elapsed.TotalSeconds;
            }

            base.Update(elapsed);
        }
        
        public override void Shot()
        {       
            
            DisparoMovil cañon1 = (DisparoMovil)base.TypeShot.Clone();
            DisparoMovil cañon2 = (DisparoMovil)base.TypeShot.Clone();
            DisparoMovil cañon3 = (DisparoMovil)base.TypeShot.Clone();
            DisparoMovil cañon4 = (DisparoMovil)base.TypeShot.Clone();

            cañon1.Posicion = base.MovePartPosition;
            cañon2.Posicion = m_movepartPosition2;
            cañon3.Posicion = m_movepartPosition3;
            cañon4.Posicion = m_movepartPosition4;

            cañon1.SetMovement(base.MovePartRotation);
            cañon2.SetMovement(base.MovePartRotation);
            cañon3.SetMovement(base.MovePartRotation);
            cañon4.SetMovement(base.MovePartRotation);

            Disparos.Instance.Add(cañon1);
            Disparos.Instance.Add(cañon2);
            Disparos.Instance.Add(cañon3);
            Disparos.Instance.Add(cañon4);
              

            if (base.PatronActual == null && base.PatronesMovimiento.Count == 0)
            {
                Boss3Disparo1 megadisparo = new Boss3Disparo1
                    (ShootTextures.Instance["Disparo1Boss3"], "MegaDisparo", base.Bounds, EnumMovement.Izquierda);

                Boss3Disparo1 megadisparo2 = new Boss3Disparo1
                    (ShootTextures.Instance["Disparo1Boss3"], "MegaDisparo2", base.Bounds, EnumMovement.Derecha);

                megadisparo.Posicion = new Vector2(base.Posicion.X, base.Posicion.Y + base.Height / 2);
                megadisparo2.Posicion = new Vector2(base.Posicion.X + base.Width, base.Posicion.Y + base.Height / 2);

                Disparos.Instance.Add(megadisparo);
                Disparos.Instance.Add(megadisparo2);
            }

        }

        #endregion
    }
}
