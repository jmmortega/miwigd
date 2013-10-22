/*
 * Creador : Chema 
 * Fecha Creacion: 13/06/2012
 * Notas
 * 
 * Clase para un hud de juego.
 * 
 * Cambios realizados.
 * 
 * 1.0 
 * 
 * Creación de la clase.
 * 
 * 1.1 29/06/2012
 * 
 * Se introduce una comprobación en las propiedades de tipo Value para evitar las asignaciones innecesarias.
 * 
 * 1.2 08/07/2012
 * 
 * Se modifica la carga de la textura de la vida
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NamoCode.Game.Class.Design;
using NamoCode.Game.Class.Design.Hud;
using NamoCode.Game.Utils;

using StarPaper.Class.Events.Director;

namespace StarPaper.Class.Design
{
    public class GameHub : GroupElement
    {
        #region Constructor

        public GameHub(string name , Vector2 incremento , Vector2 posicion , float height , float width)
            :base(name , incremento , posicion, height , width)
        {         

            m_shield = new ProgressBar
                ("ShieldBar", new Vector2(base.Posicion.X + 20 , base.Posicion.Y + 15), 20, 100);
                        
            m_vidas = new GroupElement
                ("Vidas", new Vector2(20, 0), new Vector2(base.Posicion.X + 125, base.Posicion.Y + 10));

            m_puntos = new GroupElement
                ("Puntos", new Vector2(100, 0), new Vector2(base.Posicion.X + 175, base.Posicion.Y + 10));
                        
            base.Add(m_shield);
            base.Add(m_vidas);
            base.Add(m_puntos);


            base.ColorMarco = Color.Gray;
            base.Marco = true;            
        }
        
        #endregion

        #region Fields

        private ProgressBar m_shield;

        private GroupElement m_vidas;

        private GroupElement m_puntos;

        private int m_valueShield = 0;

        public int ValueShield
        {
            get { return m_valueShield; }

            set 
            {
                if (m_valueShield != value)
                {
                    m_shield.Value = value;
                    m_valueShield = value;
                }
            }
        }

        private int m_valueVidas = 0;

        public int ValueVidas
        {
            get { return m_valueVidas; }

            set 
            {
                if (m_valueVidas != value)
                {
                    m_valueVidas = value;
                    RefreshVidas();
                }
            }
        }

        private long m_valuePuntos = 0;

        public long ValuePuntos
        {
            get { return m_valuePuntos; }
            set
            {
                if (m_valuePuntos != value)
                {
                    (m_puntos.GetElement("Points") as ElementString).LabelContent = value.ToString();
                    m_valuePuntos = value;
                }
            }
        }
            
        #endregion

        #region Methods


        #region Initialize 

        /// <summary>
        /// Inicializa el GameHub
        /// </summary>
        /// <param name="vidas">
        /// La cantidad de vidas que tiene tenemos
        /// </param>
        /// <param name="puntos">
        /// La cantidad de puntos con la que se inicializa.
        /// </param>
        public void Initialize(int vidas , long puntos , int shield)
        {
            m_valuePuntos = puntos;
            m_valueShield = shield;
            m_valueVidas = vidas;

            InitializeVidas();
            InitializePoints();
            InitializeShield();
                                               
        }

        public void Initialize(HubState hubvalues)
        {
            this.Initialize(hubvalues.Lifes, hubvalues.Points, hubvalues.Shield);
        }
        
        private void InitializeVidas()
        {
            Texture2D vidas = BasicTextures.GetTexture("ProtaLife");

            for (int i = 0; i < m_valueVidas; i++)
            {
                m_vidas.Add(vidas, "Vida", new Vector2(1f, 1f));
            }
        }

        private void InitializePoints()
        {
            SpriteFont fuente = BasicTextures.CargarFuente("ThirdScreen/Segoe14Bold");

            m_puntos.Add(fuente, "LabelPoints", "Points:");
            m_puntos.Add(fuente, "Points" , m_valuePuntos.ToString());
        }

        private void InitializeShield()
        {
            m_shield.Value = m_valueShield;
        }

        
        #endregion

        public override void Draw(SpriteBatch sprite)
        {           
            base.Draw(sprite);            
        }

        public override void Update(TimeSpan elapsedtime)
        {
            m_shield.Update(elapsedtime);            
        }

        public void Update(TimeSpan elapsedtime, HubState state)
        {
            this.Update(elapsedtime);

            this.ValuePuntos = state.Points;
            this.ValueShield = state.Shield;
            this.ValueVidas = state.Lifes;
        }

        private void RefreshVidas()
        {
            m_vidas.Clear();
            InitializeVidas();
        }
            
        #endregion

    }
}
