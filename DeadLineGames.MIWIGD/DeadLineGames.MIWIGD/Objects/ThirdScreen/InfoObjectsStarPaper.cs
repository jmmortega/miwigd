/*
 * Creador : Chema
 * Fecha Creacion: 16/05/2012
 * Notas
 * 
 * Clase que extiende el funcionamiento de InfoObjects
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
 * 
 * 1.1 06/06/2012
 * 
 * Se realiza la sobrecarga del objeto RemoveDisposed, para agregar los disparos.
 * 
 * 1.2 08/06/2012
 * 
 * Se agrega una propiedad para acceder más fácilmente al prota.
 * 
 * 1.3 30/06/2012
 *  
 * Se integra la propiedad Count
 * 
 * 1.4 08/07/2012
 * 
 * Se introduce el método encargado de la asignación de la posición de la nave a los enemigos de tipo SeekObject
 * 
 * 1.5 12/07/2012
 * 
 * Solventado el bug que estaba produciendo que las colisiones se produjeran dos veces.
 * Se agrega el cambio para el mega atack
 * Se agrega la posibilidad de añadir enemigos de manera estática.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Class.Objects;
using NamoCode.Game.Class.Colisions;
using NamoCode.Game.Utils;
using NamoCode.Game.Utils.Collections;
using NamoCode.Game.Class.Exceptions;


using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Objects.Buenos;
using StarPaper.Class.Objects.Enemies;

namespace StarPaper.Class.Objects
{
    public class InfoObjectsStarPaper : InfoObjects
    {
        #region Fields
        
        public Prota Protagonista
        {
            get
            {
                try
                {
                    return (Prota)base.Protas.GetElement("Prota");
                }
                catch (NotIncludedInList)
                {
                    return null;
                }
            }

            set
            {
                Prota newvalue = (Prota)base.Protas.GetElement("Prota");

                newvalue = value;
            }
        }

        public override int Count
        {
            get
            {
                return base.Count + Disparos.Instance.Count;
            }
        }

        private static MyList<Enemigo> m_malosTemp = new MyList<Enemigo>();

        public static MyList<Enemigo> MalosTemp
        {
            get { return InfoObjectsStarPaper.m_malosTemp; }
            set { InfoObjectsStarPaper.m_malosTemp = value; }
        }

        #endregion

        #region Methods

        protected override void AddAllObjects()
        {            
            foreach (AObject objeto in base.Malos)
            {
                base.Allobjects.Push(objeto);
            }

            foreach (AObject objeto in Disparos.Instance.DisparoMalos)
            {
                base.Allobjects.Push(objeto);
            }            
        }

        public override void Draw(SpriteBatch sprite)
        {
            Disparos.Instance.Draw(sprite);
            base.Draw(sprite);
        }
        
        public override void Update(TimeSpan elapsed)
        {
            Disparos.Instance.Update(elapsed);
            AsignGoodPosition();
            AddTempBadGuys();
            base.Update(elapsed);
        }

        /// <summary>
        /// Fuerza el disparo de los malos.
        /// </summary>
        public void Shot()
        {
            foreach(Enemies.Enemigo enemy in base.Malos)
            {
                enemy.Shot();
            }
        }

        /// <summary>
        /// Fuerza el disparo de la nave en cuestión.
        /// </summary>
        /// <param name="name">
        /// El nombre de la nave
        /// </param>
        public void Shot(string name)
        {
            (base.Malos.GetElement(name) as Enemies.Enemigo).Shot();
        }

        public override void Clear()
        {
            base.Clear();
            Disparos.Instance.Clear();
        }

        private void AsignGoodPosition()
        {
            foreach (AObject malo in base.Malos)
            {
                if (GodClass.IsContainsInterface(malo, typeof(ISeek)) == true)
                {
                    (malo as ISeek).SeekObject = Protagonista.Center;
                }
            }
        }

        private void AddTempBadGuys()
        {
            foreach (Enemigo malo in m_malosTemp)
            {
                base.Malos.Add(malo);
            }

            m_malosTemp.Clear();
        }

        #region Colisions

        public override List<InfoColission> CheckColissions()
        {
            List<InfoColission> colisiones = new List<InfoColission>();

            InfoColission colisionProta = CheckColisionProta();

            if (colisionProta != null)
            {
                colisiones.Add(colisionProta);
            }

            colisiones.AddRange(CheckColisionDisparos());

            return colisiones;

        }

        /// <summary>
        /// Comprueba las colisiones con el prota
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Solo vamos a permitir que por cada ciclo se produzca una colisión.
        /// </remarks>
        private InfoColission CheckColisionProta()
        {
            AddAllObjects();

            AObject objeto = null;

            if(base.Allobjects.Count > 0)
            {
                objeto = base.Allobjects.Pop();
            }

            if (this.Protagonista != null)
            {

                while (base.Allobjects.Count != 0)
                {
                    if (this.Protagonista.HaveColision(objeto) == true)
                    {
                        return new InfoColission(this.Protagonista, objeto);
                    }

                    objeto = base.Allobjects.Pop();
                }
            }

            return null;
        }

        private List<InfoColission> CheckColisionDisparos()
        {            
            Stack<Disparo> pilaTiros = Disparos.Instance.DisparoBuenos.ToStack();

            MyListElements<AObject> pilaMalos = (MyListElements<AObject>)base.Malos.Clone();
                                    
            List<InfoColission> colisiones = new List<InfoColission>();
                        
            while(pilaTiros.Count != 0)
            {                
                //En caso de que no haya más malos es lógico que dejaremos de hacer comprobaciones de colisiones.
                if (pilaMalos.Count > 0)
                {
                    InfoColission colision = CheckColision(pilaTiros.Pop(), pilaMalos);

                    if (colision != null)
                    {
                        colisiones.Add(colision);

                        ///Removemos de la lista el objeto que ya ha colisionado con el disparo.
                        pilaMalos.Remove(colision.GetObject(typeof(Enemigo)));
                    }
                }
                else
                {
                    pilaTiros.Clear();
                }
            }

            return colisiones;
        }

        private InfoColission CheckColision(AObject checkcolision,  MyListElements<AObject> objectsColision)
        {            
            foreach (AObject objeto in objectsColision)
            {
                if (checkcolision.HaveColision(objeto) == true)
                {
                    return new InfoColission(checkcolision, objeto);                    
                }
            }

            return null;             
        }
                        
        #endregion

        #region RemoveDisposed

        public override void RemoveDisposed()
        {
            Disparos.Instance.RemoveDisposed();
            base.RemoveDisposed();
        }
                        
        #endregion

        #region Screen Request

        /// <summary>
        /// Se limpia todo de disparos y enemigos, exceptuando los boss finales, que se le hacen cincuenta de daño.
        /// </summary>
        /// <returns>
        /// Devuele la cantidad de puntos obtenida.
        /// </returns>
        public int MegaAtack()
        {
            int pointsReturned = 0;
            Disparos.Instance.Clear();
            MyList<AObject> malosEleminados = new MyList<AObject>();

            foreach (Enemigo malo in base.Malos)
            {
                if (GodClass.IsContainsInterface(malo, typeof(IBoss)) == false)
                {
                    pointsReturned += malo.Points;
                    malosEleminados.Add(malo);
                }
                else
                {
                    malo.Shield -= 50;

                    if (malo.Shield <= 0)
                    {
                        pointsReturned += malo.Points;
                        malo.Kill();
                    }
                }
            }

            foreach (AObject malo in malosEleminados)
            {
                base.Malos.Remove(malo);
            }

            return pointsReturned;
        }

        #endregion

        #endregion
    }
}
