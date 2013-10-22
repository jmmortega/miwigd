/*
 * Creador : Chema
 * Fecha Creacion: 17/05/2012
 * Notas
 * 
 * Clase de dirección del juego.
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
 * 
 * 1.1 06/06/2012
 * 
 * Se integra la lógica de StoryBoard.
 * Se modifica el constructor para añadir la dificultad de juego.
 * Se agrega el evento de lectura de cabecera.
 * 
 * 1.2 08/06/2012
 * 
 * Se implementa la comprobación de colisiones.
 * Se agrega el objeto de InfoObjects dentro de la clase Director. 
 *      La verdad, que es el director el que debe ser manejado por la pantalla de juego y los objetos ser manejados por el director.       
 *      
 * Se sobrecarga el método update.
 * 
 * 1.3 11/06/2012
 * 
 * Se modifica el Determine Colission. Al comprobar los elementos primitivos o padres, no accede a las acciones a realizar.
 *  
 * 1.4 14/06/2012
 * 
 * Se alteran un poco los efectos de las colisiones. Integración de escudos, añadido de puntos, etc...
 * 
 * 1.5 29/06/2012
 * 
 * Se implementan los bonus.
 * 
 * 1.6 06/07/2012
 * 
 * Se crea la propiedad Dificultad.
 * Se integra el cálculo del Extend
 * 
 * 1.7 07/07/2012
 * 
 * Se agrega el evento GameOver
 * Se agrega el método a evento del StoryBoard indicando el final de fase y las comprobaciones de que el monstruo final ha sido eliminado.
 * 
 * 1.8 08/07/2012
 * 
 * Se integra la propiedad del tipo ProtaValues para almacenar los valores principales de la nave entre fase y fase.
 * 
 * 1.9 12/07/2012
 * 
 * Se añade el mega ataque
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using NamoCode.Game.Class.Design;
using NamoCode.Game.Class.Events;
using NamoCode.Game.Class.Colisions;
using NamoCode.Game.Class.Exceptions;
using NamoCode.Game.Utils;

using StarPaper.Class.Objects;
using StarPaper.Class.Objects.Buenos;
using StarPaper.Class.Objects.Enemies;
using StarPaper.Class.Objects.Enemies.ListaEnemigos;
using StarPaper.Class.Objects.Shoots;
using StarPaper.Class.Events.Director.StoryBoard;
using StarPaper.Utils;


namespace StarPaper.Class.Events.Director
{
    /// <summary>
    /// TODO: Implementar lo creado dentro de StoryBoard. 
    /// No olvidar llamar al update y al inicializar del StoryBoard.
    /// Recordar crear una llamada al evento de StoryBoard que indica los eventos producidos.
    /// Realizar la determinación de colisiones.
    /// </summary>
    public class DirectorStarPaper : NamoCode.Game.Class.Events.Director
    {

        #region Constructor

        public DirectorStarPaper(EnumDificultad dificultad , int extendPoints)
            :base()
        {

            base.Objetos = new InfoObjectsStarPaper();

            m_storyboard.OnNewStoryBoardObject += NewStoryBoardEvent;
            m_storyboard.OnReadStoryBoard += RequestScenarioLoad;
            m_storyboard.OnEndPhase += EndPhase;

            m_dificultad = dificultad;
            m_extendPoints = extendPoints;

            AssignDificult();
            
        }

        #region AssignDificult

        /// <summary>
        /// Modifica los valores por dificultad. 
        /// Añade el modificador de cantidad dentro del storyboard        
        /// </summary>
        private void AssignDificult()
        {
            ChangeQuantity();            
        }

        /// <summary>
        /// Cambia el modificador de cantidad del storyboard
        /// </summary>
        private void ChangeQuantity()
        {
            if (m_dificultad == EnumDificultad.SuperFacil)
            {
                m_storyboard.QuantityModifier = 0.25F;
            }
            else if (m_dificultad == EnumDificultad.Facil)
            {
                m_storyboard.QuantityModifier = 0.5F;
            }
            else if (m_dificultad == EnumDificultad.Normal)
            {
                m_storyboard.QuantityModifier = 1F;
            }
            else if (m_dificultad == EnumDificultad.Dificil)
            {
                m_storyboard.QuantityModifier = 2;
            }
            else if (m_dificultad == EnumDificultad.SuperDificil)
            {
                m_storyboard.QuantityModifier = 3;
            }

        }
        
        #endregion

        /// <summary>
        /// Carga el nuevo nivel.
        /// </summary>
        public virtual void LoadLevel(string filelevel)
        {
            m_storyboard.Initialize(filelevel);            
        }

        #endregion

        #region Fields

        private StoryBoardHelper m_storyboard = new StoryBoardHelper();

        private EnumDificultad m_dificultad = EnumDificultad.Normal;

        public EnumDificultad DificultadJuego
        {
            get
            {
                return m_dificultad;
            }
            set
            {
                m_dificultad = value;     
                AssignDificult();             
            }
        }

        public override float Vidas
        {
            get
            {
                try
                {
                    return (base.Objetos.Protas.GetElement("Prota") as Prota).Lifes;
                }
                    //Si se trata de este tipo de excepción devolvemos 0. El objeto prota no se encuentra y por lo tanto no hay vidas.
                catch(NotIncludedInList) 
                {
                    return 0;
                }                                                
            }            
        }

        private long m_points = 0;

        public override long Puntuacion
        {
            get { return m_points; }
        }
        
        public HubState HubValues
        {
            get         
            {
                    if((base.Objetos as InfoObjectsStarPaper).Protagonista != null)
                    {
                    //Introduzco el debug la cantidad de objetos que se encuentran en pantalla.
#if DEBUG                    
                    return new HubState(base.Objetos.Count, (int)(base.Objetos as InfoObjectsStarPaper).Protagonista.Shield, (int)this.Vidas);
#else
                return new HubState(m_points, (int)(base.Objetos as InfoObjectsStarPaper).Protagonista.Shield, (int)this.Vidas);
#endif
                    }
                    else
                    {
                        return new HubState(m_points, 0, (int)this.Vidas);
                    }
            }
        }

        private int m_swicthBonus = 0;

        private long m_extendPoints = 0;

        private int m_actualExtend = 0;

        /// <summary>
        /// Indica que la fase ha terminado por parte del StoryBoard.
        /// </summary>
        private bool m_endPhase = false;

        private string m_boss = string.Empty;

        private ProtaValues? m_protaValues = null;

        public ProtaValues ProtaValues
        {
            get 
            {
                if (m_protaValues == null)
                {
                    m_protaValues = (base.Objetos as InfoObjectsStarPaper).Protagonista.Values;
                }

                return m_protaValues.Value;
            }
            set { m_protaValues = value; }
        }
                                
        #endregion

        #region Events

        public delegate void HandleStoryBoardEventEnemy(StoryBoardEventEnemy enemy);

        public event HandleStoryBoardEventEnemy OnStoryBoardEventEnemy;

        public delegate void HandleStoryBoardEventBonus(StoryBoardEventBonus bonus);

        public event HandleStoryBoardEventBonus OnStoryBoardEventBonus;

        public delegate void HandleStoryBoardEventOther(StoryBoardEventOther other);

        public event HandleStoryBoardEventOther OnStoryBoardEventOther;

        public delegate void HandleLoadScenario(string scenarioName);

        public event HandleLoadScenario OnScenarioLoad;

        public delegate void HandleGameOver();

        public event HandleGameOver OnGameOver;

        public delegate void HandleEndPhase();

        public event HandleEndPhase OnEndPhase;

        #endregion

        #region Methods Events

        #region NewStoryBoardEvent

        private void NewStoryBoardEvent(AStoryBoardEvent evento)
        {
            //Determinamos de que evento se trata para lanzar uno u otro método

            if (evento.GetType() == typeof(StoryBoardEventEnemy))
            {
                EnemyEvent((StoryBoardEventEnemy)evento);
            }
            else if (evento.GetType() == typeof(StoryBoardEventBonus))
            {
                BonusEvent((StoryBoardEventBonus)evento);
            }
            else if (evento.GetType() == typeof(StoryBoardEventOther))
            {
                OtherEvent((StoryBoardEventOther)evento);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void RequestScenarioLoad(StoryBoardHeader header)
        {
            if (OnScenarioLoad != null)
            {
                OnScenarioLoad(header.ScenarioName);
            }
        }
                
        private void EnemyEvent(StoryBoardEventEnemy evento)
        {
            Enemigo badpeople = EnemiesListHelper.Instance[evento.Name];

            //badpeople.SetPosicion(evento.Posicion.X, evento.Posicion.Y);
            badpeople.Posicion = new Vector2(evento.Posicion.X, evento.Posicion.Y);

            base.Objetos.Malos.Add(badpeople);

            if (OnStoryBoardEventEnemy != null)
            {
                OnStoryBoardEventEnemy(evento);
            }
        }

        private void BonusEvent(StoryBoardEventBonus evento)
        {
            if (OnStoryBoardEventBonus != null)
            {
                OnStoryBoardEventBonus(evento);
            }
        }

        private void OtherEvent(StoryBoardEventOther evento)
        {
            if (OnStoryBoardEventOther != null)
            {
                OnStoryBoardEventOther((StoryBoardEventOther)evento);
            }
        }


        #endregion

        #region EndPhase

        private void EndPhase(string nameBoss)
        {
            m_endPhase = true;
            m_boss = nameBoss;
        }

        /// <summary>
        /// Localiza el monstruo final entre los objetivos.
        /// </summary>
        /// <returns></returns>
        private void SeekBoss()
        {
            try
            {
                base.Objetos.Malos.GetElement(m_boss);
            }
                ///En caso de que se produzca esta excepción es que el Final Boss ya está eliminado y por lo tanto se ha finalizado la fase.
            catch (NotIncludedInList)
            {
                if (OnEndPhase != null)
                {                    
                    OnEndPhase();
                }
            }
            catch (Exception e)
            {
                throw e;
            }            
        }
        
        #endregion

        #endregion

        #region Methods

        #region Colisions

        public override ResumeColisions DetermineColissions()
        {
            List<InfoColission> colisions = Objetos.CheckColissions(); 

            ResumeColisions infocolision = new ResumeColisions();

            foreach (InfoColission colision in colisions)
            {
                //La comprobación de colisión de Bonus, va por encima de la de los enemigos, ya que para el bonus tenemos como clase base la Clase Enemigos.
                if (colision.IsType(typeof(Prota)) && colision.IsType(typeof(IBonus)))
                {
                    ProtaVSBonus((Prota)colision.GetObject(typeof(Prota)) , (BonusObject)colision.GetObject(typeof(IBonus)));
                }
                else if (colision.IsType(typeof(Enemigo)) && colision.IsType(typeof(Prota)))
                {
                    ProtaVSEnemy((Prota)colision.GetObject(typeof(Prota)), (Enemigo)colision.GetObject(typeof(Enemigo)));                                                          
                }
                else if (colision.IsType(typeof(Enemigo)) && colision.IsType(typeof(IGoodGuyShoot)))
                {
                    DisparoProtaVSEnemy((Disparo)colision.GetObject(typeof(IGoodGuyShoot)), (Enemigo)colision.GetObject(typeof(Enemigo)));                                        
                }
                else if (colision.IsType(typeof(Prota)) && colision.IsType(typeof(Disparo)))
                {
                    ProtaVSDisparo((Prota)colision.GetObject(typeof(Prota)), (Disparo)colision.GetObject(typeof(Disparo)));                                        
                }
                
            }

            base.Objetos.RemoveDisposed();

            return infocolision;

        }

        /// <summary>
        /// Proceso de colisión entre prota y enemigo
        /// </summary>
        /// <param name="prota"></param>
        /// <param name="enemy"></param>
        private void ProtaVSEnemy(Prota prota , Enemigo enemy)
        {
            //Se destruye la nave enemiga y el prota recibe perdida de escudos.

            float protashield = prota.Shield;

            //El prota recibe tanto daño como escudos tenga.
            prota.Shield -= enemy.Shield * 2;

            //El enemigo recibe tanto daño como escudos tenga
            if (enemy.IsFinalBoss == false)
            {
                enemy.Shield -= protashield;
            }
            
            if (prota.Shield <= 0)
            {
                prota.Kill();               
            }

            if (enemy.Shield <= 0)
            {
                CalcBonus(enemy);                
                m_points += enemy.Points;
                enemy.Kill();
            }
            
        }

        private void DisparoProtaVSEnemy(Disparo shot, Enemigo enemy)
        {
            //Se resta el valor de daño del disparo del disparo del bueno, si las vidas son iguales o menores a 0 se lanza Kill.

            enemy.Shield -= shot.Damage;

            shot.Dispose();

            if (enemy.Shield <= 0)
            {
                CalcBonus(enemy);                
                m_points += enemy.Points;
                enemy.Kill();
            }                    
        }

        private void ProtaVSDisparo(Prota prota, Disparo shot)
        {
            //Se resta el valor de daño del disparo del malo al prota

            prota.Shield -= shot.Damage;

            shot.Kill();

            if (prota.Shield <= 0)
            {
                prota.Kill();
            }
        }

        private void ProtaVSBonus(Prota prota, BonusObject bonus)
        {
            prota.SetBonus(bonus);

            bonus.Kill();
        }

        #endregion

        #region Update

        public void DoStoryBoard(float elapsedtime)
        {
            m_storyboard.Update(elapsedtime);
        }

        public override void Update(TimeSpan elapsedtime)
        {
            DoStoryBoard((float)elapsedtime.TotalSeconds);

            base.Objetos.Update(elapsedtime);
            base.Objetos.DoMovement();
            base.Objetos.RemoveDisposed();

            CalcExtend();

            if (m_endPhase == true)
            {
                SeekBoss();
            }

            base.Update(elapsedtime);
        }

        #endregion

        #region Request Screen

        public override void NewGame()
        {
            base.NewGame();

            base.Objetos.Clear();
            m_storyboard.Reset();

            m_points = 0;
            m_actualExtend = 0;

            m_endPhase = false;

            base.Bonus = new Bonus((double)ReturnModifier());
        }

        public void NewLevel()
        {
            m_endPhase = false;

            base.Objetos.Clear();
        }

        public override bool EndGame()
        {
            if (this.Vidas <= 0)
            {
                if (OnGameOver != null)
                {
                    OnGameOver();
                }
                return true;
            }
            else
            {
                return false;
            }                
        }

        public void MegaAtack()
        {
            m_points += (base.Objetos as InfoObjectsStarPaper).MegaAtack();
            (base.Objetos as InfoObjectsStarPaper).Protagonista.Shield -= (100 - ReturnModifier()) / 2;

            if ((base.Objetos as InfoObjectsStarPaper).Protagonista.Shield <= 0)
            {
                (base.Objetos as InfoObjectsStarPaper).Protagonista.Kill();
            }
        }

        #endregion

        #region Bonus

        private void CalcBonus(Enemigo enemy)
        {            
            if (Bonus.HaveBonus() == true || enemy is BonusObject)                        
            {
                BonusObject bonus;

                    ///BonusDamage
                if (m_swicthBonus == 0)
                {
                    m_swicthBonus++;
                    bonus = new BonusObject
                        (BasicTextures.GetTexture("BonusDamage"), "BonusDamage", DesignOptions.Bounds, EnumBonusType.BonusDamage);                    
                }
                    ///BonusRapid
                else if (m_swicthBonus == 1)
                {
                    m_swicthBonus++;
                    bonus = new BonusObject
                        (BasicTextures.GetTexture("BonusRapid"), "BonusRapid", DesignOptions.Bounds, EnumBonusType.BonusRapid);
                }
                    ///BonusShot
                else 
                {
                    m_swicthBonus = 0;
                    bonus = new BonusObject
                        (BasicTextures.GetTexture("BonusShot"), "BonusShot", DesignOptions.Bounds, EnumBonusType.BonusShot);
                }

                bonus.SetPosicion(enemy.Posicion.X, enemy.Posicion.Y);

                base.Objetos.Malos.Add(bonus);

                
            }
        }
        
        private int ReturnModifier()
        {
            if (m_dificultad == EnumDificultad.SuperFacil)
            {
                return 60;
            }
            else if (m_dificultad == EnumDificultad.Facil)
            {
                return 40;
            }
            else if (m_dificultad == EnumDificultad.Normal)
            {
                return 20;
            }
            else if (m_dificultad == EnumDificultad.Dificil)
            {
                return 10;
            }
            else if (m_dificultad == EnumDificultad.SuperDificil)
            {
                return 5;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calcula el valor de extend.
        /// </summary>
        private void CalcExtend()
        {
            int extend = 0;

            if (m_points > 0)
            {
                extend = (int)(m_points / m_extendPoints);
            }

            if (extend != m_actualExtend)
            {
                int lifesIncrement = extend - m_actualExtend;

                (base.Objetos as InfoObjectsStarPaper).Protagonista.Lifes = (int)Vidas + lifesIncrement;

                m_actualExtend = extend;
            }
        }

        #endregion

        #endregion

    }
}
