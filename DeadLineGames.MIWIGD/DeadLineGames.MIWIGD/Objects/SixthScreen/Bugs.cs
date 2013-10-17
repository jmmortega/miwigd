using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;

namespace DeadLineGames.MIWIGD.Objects.SixthScreen
{
    class Bugs: AObjects
    {

        #region Singleton

        public Bugs()
            : base()
        { }

        private static volatile Bugs m_instance = null;
        private static readonly object padlock = new object();

        public static Bugs Instance
        {
            get
            {
                lock (padlock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new Bugs();
                    }

                    return Bugs.m_instance;
                }
            }

        }

        #endregion

        public void updateBugs(TimeSpan elapsed)
        {
            foreach (Bug b in m_instance)
            {
                if (!b.isEating && !b.isOutOfBounds)
                    b.Update(elapsed);
                else
                {
                    m_instance.Remove(b);
                    break;
                }
            }
        }

    }
}
