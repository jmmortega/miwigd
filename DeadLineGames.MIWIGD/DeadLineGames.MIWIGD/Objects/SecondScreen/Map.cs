using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Utils.Stream;
using NamoCode.Game.Class.Objects;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeadLineGames.MIWIGD.Objects.SecondScreen
{
    class Map : AObjects
    {

        public const int WIDTHLENGTH = 19;
        public const int HEIGHTLENGTH = 10;
        public const int FLOORWIDTH = 30;
        public const int FLOORHEIGHT = 40;

        #region Singleton

        public Map()
            : base()
        {
            LoadMap();
        }

        private static volatile Map m_instance = null;
        private static readonly object padlock = new object();

        public static Map Instance
        {
            get
            {
                lock (padlock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new Map();
                    }

                    return Map.m_instance;
                }
            }

        }

        #endregion

        private void LoadMap()
        {

            List<string> lines = MyStream.Instance.ReadContent("Content/SecondScreen/0.map");

            BasicTextures.CargarTextura("SecondScreen/Wall", "wall");

            if (lines.Count == HEIGHTLENGTH)
            {
                for (int y = 0; y < HEIGHTLENGTH; ++y)
                {
                    for (int x = 0; x < WIDTHLENGTH; ++x)
                    {
                        // to load each tile.
                        char tileType = lines[y][x];
                        if (tileType == 'W')
                        {
                            int posX = DesignOptions.Bounds.MinX + (x * FLOORWIDTH);
                            int posY = DesignOptions.Bounds.MinY + (y * FLOORHEIGHT);
                            this.Add(new Wall(new Vector2(posX, posY)));
                        }
                    }
                }
            }
        }
    }
}
