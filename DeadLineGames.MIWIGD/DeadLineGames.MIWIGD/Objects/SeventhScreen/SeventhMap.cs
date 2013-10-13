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

namespace DeadLineGames.MIWIGD.Objects.SeventhScreen
{
    class SeventhMap : AObjects
    {

        private const int WIDTHLENGTH = 57;
        private const int HEIGHTLENGTH = 7;
        private const int FLOORWIDTH = 10;
        private const int FLOORHEIGHT = 10;

        public int boundsFloor = 0;

        #region Singleton

        public SeventhMap()
            : base()
        {
            BasicTextures.CargarTextura("SeventhScreen/F1", "F1");
            BasicTextures.CargarTextura("SeventhScreen/F2", "F2");
            BasicTextures.CargarTextura("SeventhScreen/F3", "F3");
            BasicTextures.CargarTextura("SeventhScreen/F4", "F4");

            LoadMap();
        }

        private static volatile SeventhMap m_instance = null;
        private static readonly object padlock = new object();

        public static SeventhMap Instance
        {
            get
            {
                lock (padlock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new SeventhMap();
                    }

                    return SeventhMap.m_instance;
                }
            }

        }

        #endregion

        private void LoadMap()
        {

            List<string> lines = MyStream.Instance.ReadContent("Content/SeventhScreen/0.map");
            
            if (lines.Count == HEIGHTLENGTH)
            {
                for (int y = 0; y < HEIGHTLENGTH; y++)
                {
                    for (int x = 0; x < WIDTHLENGTH; x++)
                    {
                        // to load each tile.
                        char tileType = lines[y][x];

                        int posX = DesignOptions.Bounds.MinX + (x * FLOORWIDTH);
                        int posY = DesignOptions.Bounds.MaxY + ((y * FLOORHEIGHT) - (HEIGHTLENGTH * FLOORHEIGHT));
                        if (x == 0 && y == 0) boundsFloor = posY;
                        this.Add(new Floor(BasicTextures.GetTexture("F" + tileType), 
                                new Vector2(posX, posY)));
                    }
                }
            }
        }
    }
}
