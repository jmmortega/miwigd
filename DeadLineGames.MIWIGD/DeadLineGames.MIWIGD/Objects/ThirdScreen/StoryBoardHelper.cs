/*
 * Creador : Chema 
 * Fecha Creacion: 18/05/2012
 * Notas
 * 
 * Creación de la clase de StoryBoard. Clase que se encargara de realizar la lectura de los ficheros de fase e identificar la información.
 * 
 * Cambios realizados.
 * 
 * 1.0
 * 
 * Creación de la clase
 * 
 * 1.1 30/05/2012
 * 
 * Se realiza la implementación de la integración de datos por StoryBoard
 * 
 * 1.2 31/05/2012
 * 
 * Se cambia el nombre de la clase de StoryBoard a StoryBoardHelper para evitar errores con el espacio de nombres.
 * 
 * 1.3 06/06/2012
 * 
 * Se implementa la posibilidad de integrar comentarios dentro del storyboard.
 * Se retiran dos propiedades ScenarioGame y GameTime, estos se han agregado a la clase StoryBoardHeader y ahora se ponen
 *      esos campos
 * Se incluye un evento para cuando se lee la cabecera del storyboard.
 * Se modifica el método GetHeader, realizando la instanciación de StoryBoardHeader
 * 
 * 1.4 12/06/2012
 * 
 * Se solventa el bug de Cultura. Este interpretaba el separador decimal del delay de distintas maneras.
 * 
 * 1.5 02/07/2012
 * 
 * Se agrega el método Reset dentro del StoryBoard, para que no mantenga los valores entre un juego y otro.
 * 
 * 1.6 07/07/2012
 * 
 * Se añade la notificación para indicar que el nivel se ha acabado.
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using NamoCode.Game.Class.Events;

using NamoCode.Game.Utils;
using NamoCode.Game.Utils.Collections;
using NamoCode.Game.Utils.Stream;



namespace StarPaper.Class.Events.Director.StoryBoard
{
    /// <summary>
    /// Clase de storyboard.
    /// La lógica de una fase se compone de la persistencia de un fichero de texto que léido implementa una clase storyboard. Al inicio de cada fase se creará 
    /// una clase de este tipo que irán realizando los eventos indicados en la hoja. Dentro de la carpeta de documentación se incluye la estructura del fichero.
    /// En el momento que llegue un evento, se ejecutará. Ën el caso de los enemigos se ejecutará un delay si hay más de un enemigo, para que aparezcan de una manera 
    /// limpia dentro del juego.
    /// </summary>
    public class StoryBoardHelper : IDisposable
    {

        /*TODO:
         * Crear la implementación para la lectura del fichero e identificación aquí
         * Implementar el update con los distintos eventos y sus funcionalidades. No olvidar el delay, va a ser jodio...        
         * Implementar que avise cuando se termine la fase. La última interacción el final boss, no se puede realizar hasta que el resto de eventos se hayan ejecutado.
         * Implementar el quantity modifier y el speed modifier correctamente.
        */

        #region Events

        /// <summary>
        /// Delegado a evento
        /// </summary>
        /// <param name="evento"></param>
        public delegate void NewStoryBoardObjectHandle(AStoryBoardEvent evento);

        /// <summary>
        /// Evento que se produce cuando un objeto aparece. Esta creado para que se levante en la clase Director.
        /// </summary>
        public event NewStoryBoardObjectHandle OnNewStoryBoardObject;
        
        public delegate void ReadStoryBoardHeaderHandle(StoryBoardHeader cabecera);

        /// <summary>
        /// Evento que se produce cuando se ha terminado de leer la cabecera.
        /// </summary>
        public event ReadStoryBoardHeaderHandle OnReadStoryBoard;

        /// <summary>
        /// Notifica que el tiempo de duración de la fase a terminado y que el final boss ha aparecido, por parte del StoryBoard la fase a terminado.
        /// </summary>
        public delegate void HandleEndPhase(string nameBoss);

        public event HandleEndPhase OnEndPhase;

        #endregion

        #region Fields

        private StoryBoardHeader m_Header = null;

        public StoryBoardHeader Header
        {
            get { return m_Header; }
            set { m_Header = value; }
        }

        private float m_currentGameTime = 0;

        /// <summary>
        /// El tiempo que queda restante de la fase expresado en segundos.
        /// </summary>
        public float CurrentGameTime
        {
            get { return m_currentGameTime; }
            set { m_currentGameTime = value; }
        }

        private bool m_StopTime = false;

        /// <summary>
        /// Indica si el tiempo ha sido parado por alguna razón
        /// </summary>
        public bool StopTime
        {
            get { return m_StopTime; }
            set { m_StopTime = value; }
        }

        private float m_QuantityModifier = 1;

        /// <summary>
        /// Multiplicador de cantidad, mediante este modificar se cambian los valores de los enemigos.
        /// Se usa principalmente cuando hay cambios en la dificultad.
        /// También se utiliza para los bonus
        /// </summary>
        /// <remarks>
        /// Super Fácil - 0.25
        /// Fácil - 0.5 
        /// Normal - 1
        /// Dificil - 2
        /// Super Dificil - 3
        /// </remarks>
        public float QuantityModifier
        {
            get { return m_QuantityModifier; }
            set { m_QuantityModifier = value; }
        }

        private float m_SpeedModifier = 1;

        /// <summary>
        /// Modificador de la velocidad de juego.
        /// </summary>
        public float SpeedModifier
        {
            get { return m_SpeedModifier; }
            set { m_SpeedModifier = value; }
        }

        /// <summary>
        /// Siendo el float el valor de segundos y el AStoryBoard, el evento de StoryBoard
        /// </summary>
        private MyListTuple<float, AStoryBoardEvent> m_storyboardEvents = new MyListTuple<float, AStoryBoardEvent>();

        private string m_nameBoss = string.Empty;

        #endregion

        #region Methods

        #region Initialize

        /// <summary>
        /// Inicializa el storyBoard.
        /// </summary>
        /// <param name="filename"></param>
        public void Initialize(string filename)
        {
            m_currentGameTime = 0;
            
            LoadStoryBoard(filename);            
        }

        public void Reset()
        {
            m_currentGameTime = 0;

            m_storyboardEvents.Clear();

            m_QuantityModifier = 1;

            m_SpeedModifier = 1;

        }

        #endregion

        #region LoadStoryBoard

        private void LoadStoryBoard(string filename)
        {
            List<string> lines = GodClass.CleanNulls(RemoveComentaries(MyStream.Instance.ReadContent(filename)));

            if (lines.Count > 0)
            {
                GetHeader(lines[0]);

                lines.RemoveAt(0);
            }

            foreach (string line in lines)
            {                
                float seconds = float.Parse(line.Split(':')[0]);

                GetRestLine(line.Split(':')[1], seconds);
            }
        }

        /// <summary>
        /// Lee la cabecera del fichero.
        /// </summary>
        /// <param name="header"></param>
        private void GetHeader(string header)
        {
            string[] values = header.Split('|');

            m_Header = new StoryBoardHeader(values[0] , float.Parse(values[1]));

            if (OnReadStoryBoard != null)
            {
                OnReadStoryBoard(m_Header);
            }
        }

        private void GetRestLine(string restline , float seconds)
        {
            //M;A;3;15,15;0.15|M;B;4;0,20;0.20|B;X;1;0,0;0|O;D;1;0,0;D;0

            string[] values = restline.Split('|');

            foreach (string value in values)
            {
                string[] storyboardobject = value.Split(';');

                string type = storyboardobject[0];

                if (type == "M") //Malo
                {
                    GetBadBoys(storyboardobject, seconds);
                }
                else if (type == "B") //Bonus
                {
                    GetBonus(storyboardobject, seconds);
                }
                else if (type == "O") //Otros
                {
                    GetOther(storyboardobject, seconds);
                }
                else if (type == "F") //Final
                {                    
                    GetBoss(storyboardobject, seconds);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Elimina las líneas que le preceden && que es el indicativo de que se trata de una línea con comentarios
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private List<string> RemoveComentaries(List<string> lista)
        {            
            for(int i = 0 ; i < lista.Count - 1 ; i++)
            {
                if (lista[i].Contains("&&") == true)
                {
                    lista.Remove(lista[i]);
                }
            }

            return lista;
        }
        
        #region Gets

        /// <summary>
        /// Obtiene los valores para ese chico malo :P
        /// </summary>
        /// <param name="storyboardobject"></param>
        /// <param name="seconds"></param>
        private void GetBadBoys(string[] storyboardobject, float seconds)
        {
            float quantity = 1;

            if (GodClass.UniqueParse(storyboardobject[4]) > 0)
            {
                quantity = m_QuantityModifier;
            }

            StoryBoardEventEnemy enemy = new StoryBoardEventEnemy
                (
                seconds,
                float.Parse(storyboardobject[2]) * quantity,
                storyboardobject[1],
                GodClass.String2Vector2(storyboardobject[3]),
                GodClass.UniqueParse(storyboardobject[4]),
                false);


            m_storyboardEvents.Add(new MyTuple<float, AStoryBoardEvent>(seconds, enemy));
        }

        private void GetBonus(string[] storyboardobject, float seconds)
        {
            //B;X;1;0,0;0

            StoryBoardEventBonus bonus = new StoryBoardEventBonus
            (
            seconds,
            float.Parse(storyboardobject[2]) / m_QuantityModifier,
            storyboardobject[1],
            GodClass.String2Vector2(storyboardobject[3]),
            GodClass.UniqueParse(storyboardobject[4])
            );

            m_storyboardEvents.Add(new MyTuple<float, AStoryBoardEvent>(seconds, bonus));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storyboardobject"></param>
        /// <param name="seconds"></param>
        /// <remarks>
        /// Los eventos no se ven modificados por el modificador de cantidad como ocurre con enemigos y bonus.
        /// </remarks>
        private void GetOther(string[] storyboardobject, float seconds)
        {
            //O;D;1;0,0;D;0

            StoryBoardEventOther other = new StoryBoardEventOther
            (
            seconds,
            int.Parse(storyboardobject[2]),
            storyboardobject[1],
            GodClass.String2Vector2(storyboardobject[3]),
            GodClass.UniqueParse(storyboardobject[5]),
            StarPaperVerbs.SearchVerb(storyboardobject[4])
            );

            m_storyboardEvents.Add(new MyTuple<float, AStoryBoardEvent>(seconds, other));

        }

        private void GetBoss(string[] storyboardobject, float seconds)
        {
            StoryBoardEventEnemy enemy = new StoryBoardEventEnemy
                (
                seconds,
                int.Parse(storyboardobject[2]),
                storyboardobject[1],
                GodClass.String2Vector2(storyboardobject[3]),
                GodClass.UniqueParse(storyboardobject[4]),
                true,
                m_QuantityModifier);

            m_nameBoss = enemy.Name;

            m_storyboardEvents.Add(new MyTuple<float, AStoryBoardEvent>(seconds, enemy));
        }
        
        #endregion

        #endregion

        #region Update
        
        public void Update(float elapsedtime)
        {
            m_currentGameTime += elapsedtime * m_SpeedModifier;

            ///Chequeamos que el tiempo de
            if (Header.Gametime >= m_currentGameTime)
            {
                // De esta manera estaba realizando la descarga de todos los elementos, hay que hacerlo de manera distinta
                ///Obtenemos los objetos que se encuentren dentro de ese valor.
                MyListTuple<float, AStoryBoardEvent> m_lista = m_storyboardEvents.MinusEqualsAll(m_currentGameTime);

                for (int i = 0; i < m_lista.Count; i++)
                {
                    //Eliminamos uno de la cantidad.
                    m_lista[i].Item2.Quantity--;

                    //Agregamos el tiempo de transición al tiempo de aparición.
                    m_lista[i].Item2.Second += m_lista[i].Item2.Delay;
                    m_lista[i].Item1 += m_lista[i].Item2.Delay;

                    //Levantamos el evento del objeto en cuestión que se ha lanzado.
                    if (OnNewStoryBoardObject != null)
                    {
                        OnNewStoryBoardObject(m_lista[i].Item2);
                    }
                }

                //Eliminamos los elementos que tienen un valor de 0 o menor
                EliminarElementos(m_lista);
            }
            else
            {
                if (OnEndPhase != null)
                {
                    OnEndPhase(m_nameBoss);
                }
            }
        }

        private void EliminarElementos(MyListTuple<float, AStoryBoardEvent> m_lista)
        {
            foreach (MyTuple<float, AStoryBoardEvent> elemento in m_lista)
            {
                if (elemento.Item2.Quantity <= 0)
                {
                    m_storyboardEvents.Remove(elemento);
                }
            }
        }

        #endregion

        #endregion

        #region IDisposable

        public void Dispose()
        {
            m_storyboardEvents.Clear();
        }

        #endregion

    }
}
