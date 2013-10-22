using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework;

namespace DeadLineGames.MIWIGD.Objects.FifthScreen
{
    class QTEPusher : AnimatedElement
    {

        #region Singleton

        public QTEPusher()
            : base(
                BasicTextures.CargarTextura("FifthScreen/QTEButtons", "QTEButtons"),
                "QTEButtons",
                new Vector2(),
                new FrameRateInfo(4, 0.10f, 4, false)
            )
        {
            this.Pause();
            this.Visible = false;
        }

        private static volatile QTEPusher m_instance = null;
        private static readonly object padlock = new object();

        public static QTEPusher Instance
        {
            get
            {
                lock (padlock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new QTEPusher();
                    }

                    return QTEPusher.m_instance;
                }
            }
        }

        #endregion


        public override void Update(TimeSpan elapsedtime)
        {

            base.Update(elapsedtime);
        }

    }
}
