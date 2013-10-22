/*
 * Creador : Chema
 * Fecha Creacion: 06/06/2012
 * Notas
 * 
 * Clase helper que se encarga de crear las instancias de enemigos de una manera sencilla.
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
using NamoCode.Game.Utils.Collections;

using StarPaper.Utils;

namespace StarPaper.Class.Objects.Enemies.ListaEnemigos
{
    public class EnemiesListHelper
    {
        #region Singleton

        private static EnemiesListHelper m_instance = null;

        public static EnemiesListHelper Instance
        {
            get 
            {
                if (m_instance == null)
                {
                    m_instance = new EnemiesListHelper();
                }
                return EnemiesListHelper.m_instance; 
            }
            
        }

        protected EnemiesListHelper()
        { }

        #endregion

        #region Consts

        private const string c_TrianguloAmarillo = "TrianguloAmarillo";

        private const string c_CirculoRojo = "CirculoRojo";

        private const string c_RectanguloNaranja = "RectanguloNaranja";

        private const string c_Boss1 = "Boss1";

        private const string c_TanqueFijo = "TanqueChungo";

        private const string c_TrianguloNaranja = "TrianguloNaranja";

        private const string c_TrianguloVerde = "TrianguloVerde";

        private const string c_TrianguloRosa = "TrianguloRosa";

        private const string c_Boss2 = "Boss2";

        private const string c_Boss3 = "Boss3";

        private const string c_Boss4 = "Boss4";
        
        #endregion

        private Bounds m_bounds;

        public Bounds Bounds
        {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

        #region Indexer

        public Enemigo this[string name]
        {
            get
            {
                if (name == c_TrianguloAmarillo)
                {
                    return new TrianguloAmarillo
                        (EnemiesTextures.Instance[name], name, this.Bounds);
                }
                else if (name == c_CirculoRojo)
                {
                    return new CirculoRojo
                        (EnemiesTextures.Instance[name], name, this.Bounds);
                }
                else if (name == c_RectanguloNaranja)
                {
                    return new RectanguloNaranja
                        (EnemiesTextures.Instance[name], name, this.Bounds);
                }
                else if (name == c_Boss1)
                {
                    return new Boss1
                        (EnemiesTextures.Instance[name], name, this.Bounds);
                }
                else if (name == c_TanqueFijo)
                {
                    return new TanqueChungo
                        (EnemiesTextures.Instance[name], EnemiesTextures.Instance[name + "M"], name, this.Bounds);
                }
                else if (name == c_TrianguloNaranja)
                {
                    return new TrianguloNaranja
                        (EnemiesTextures.Instance[name], name, this.Bounds);
                }
                    //Verificamos así porque el storyboard llevará integrado los grados de inclinación de la nave.
                else if (name.Contains(c_TrianguloVerde))
                {
                    string value = name.Replace(c_TrianguloVerde, "");

                    if (value == string.Empty)
                    {
                        return new TrianguloVerde
                            (EnemiesTextures.Instance[name], name, this.Bounds);
                    }
                    else
                    {
                        return new TrianguloVerde
                            (EnemiesTextures.Instance[name.Replace(value, "")], name, this.Bounds, float.Parse(value));
                    }
                }
                else if (name.Contains(c_TrianguloRosa))
                {
                    string value = name.Replace(c_TrianguloRosa, "");

                    if (value == string.Empty)
                    {
                        return new TrianguloRosa
                            (EnemiesTextures.Instance[name], name, this.Bounds);
                    }
                    else
                    {
                        return new TrianguloRosa
                            (EnemiesTextures.Instance[name.Replace(value, "")], name, this.Bounds, float.Parse(value));
                    }
                }
                else if (name == c_Boss2)
                {
                    return new Boss2
                        (EnemiesTextures.Instance[name], name, this.Bounds);
                }
                else if (name == c_Boss3)
                {
                    return new Boss3
                        (EnemiesTextures.Instance[name], EnemiesTextures.Instance[name + "M"], name, this.Bounds);
                }
                else if (name == c_Boss4)
                {
                    return new Boss4
                        (EnemiesTextures.Instance[name], name, this.Bounds);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        #endregion
    }
}
