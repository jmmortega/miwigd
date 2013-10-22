using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeadLineGames.MIWIGD.Objects.FifthScreen
{

    public enum ActiveButton { A, B, X, Y };

    class QTEPusher : AnimatedElement
    {

        public Buttons AButton;

        #region Singleton

        public QTEPusher()
            : base(
                BasicTextures.CargarTextura("FifthScreen/QTEButtons", "QTEButtons"),
                "QTEButtons",
                new Vector2(),
                new FrameRateInfo(4, 0.15f, 4, false)
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

        public void setPosition(Vector2 position)
        {
            this.Posicion = position;
        }

        public void playButton(TimeSpan elapsed)
        {
            if (!this.Visible) 
                this.Visible = true;
            if (this.Frames.Pause)
                this.Play();

            this.Update(elapsed);

        }

        public void changeButton()
        {
            switch (getNumber(4.0))
            {
                case 1:
                    AButton = Buttons.A;
                    break;
                case 2:
                    AButton = Buttons.B;
                    break;
                case 3:
                    AButton = Buttons.X;
                    break;
                case 4:
                    AButton = Buttons.Y;
                    break;
            }
            this.Restart();
        }

        private int getNumber(double divisor)
        {
            double probabilidad = 1 / divisor;
            double porcentaje = Azar.Instance.GetPorcentual();
            for (int i = 1; i <= divisor; i++)
            {
                if (porcentaje <= probabilidad * i)
                    return i;
            }
            return -1;
        }

        private void Update(TimeSpan elapsedtime)
        {
            switch (AButton)
            { 
                case Buttons.A:
                    this.ChangeRow(0);
                    break;
                case Buttons.B:
                    this.ChangeRow(1);
                    break;
                case Buttons.X:
                    this.ChangeRow(2);
                    break;
                case Buttons.Y:
                    this.ChangeRow(3);
                    break;
            }

            base.Update(elapsedtime);
        }

    }
}
