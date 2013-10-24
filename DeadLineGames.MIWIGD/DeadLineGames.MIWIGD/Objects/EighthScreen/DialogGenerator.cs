using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace DeadLineGames.MIWIGD.Objects.EighthScreen
{

    public enum Talk { Insult, Reply, None };

    class DialogGenerator: Elements
    {

        public bool lastTalking = false;

        public bool firstTalking = true;
        public bool loosedStake = false;
        public bool opsLoaded = false;

        public int insult { get; set; }
        public int reply { get; set; }
    
        public DialogGenerator(Vector2 incrementoPosicion)
            : base(incrementoPosicion)
        { }

        public void updateSentences(Characters c, Talk talking)
        {
            if (talking != Talk.None)
            {
                float posY = this.IncrementoPosicion.Y;
                for (int i = 0; i < 4; i++)
                {
                    this.Add(new AnimatedElement(BasicTextures.GetTexture("buttons"),
                        "B" + i,
                        new Vector2(this.IncrementoPosicion.X,
                            posY),
                        new FrameRateInfo(1, 0, 4, false)));
                    ((AnimatedElement)this.GetElement("B" + i)).ChangeRow(i);
                    if (talking == Talk.Insult)
                    {
                        this.Add(new ElementString(BasicTextures.CargarFuente("EighthScreen/MonkeyFont"),
                            "T" + i,
                            Insults.Insult[i],
                            new Vector2(this.IncrementoPosicion.X + 24,
                                posY + 6)));
                    }
                    else if (talking == Talk.Reply)
                    {
                        string text;
                        if (firstTalking)
                        {
                            text = Insults.FirstReply;
                        }
                        else
                        {
                            text = Insults.Reply[i];
                        }

                        this.Add(new ElementString(BasicTextures.CargarFuente("EighthScreen/MonkeyFont"),
                            "T" + i,
                            text,
                            new Vector2(this.IncrementoPosicion.X + 24,
                                posY + 6)));
                    }
                    posY += this.GetElement("T" + i).Height + 8;
                    ((ElementString)this.GetElement("T" + i)).Color = Color.DarkGreen;
                    if (firstTalking) break;
                }
                opsLoaded = true;
                c.changeSprites(states.Idle);
            }
        }

        public void talkPirate(Characters p, Talk talking)
        {
            if (!p.pIsTalking)
            {
                string text = string.Empty;
                if (firstTalking)
                {
                    insult = 0;
                    text = Insults.FirstSentence;
                }
                else if (lastTalking)
                {
                    text = Insults.PirateLastSentence;
                }
                else
                {
                    if (talking == Talk.Insult)
                    {
                        reply = randomSentence();
                        text = Insults.Reply[reply];
                    }
                    if (talking == Talk.Reply)
                    {
                        insult = randomSentence();
                        text = Insults.Insult[insult];
                    }
                }
                this.Add(new ElementString(BasicTextures.CargarFuente("EighthScreen/MonkeyFont"),
                    "Pirate",
                    text,
                    new Vector2(p.Posicion.X + 20,
                        p.Posicion.Y - 20)));
                ((ElementString)this.GetElement("Pirate")).Color = Color.Fuchsia;

                if (!p.pEndTalking)
                {
                    p.changeSprites(states.PTalking);
                }
            }
        }

        private int randomSentence()
        {
            double probabilidad = 1 / 4.0;
            double porcentaje = Azar.Instance.GetPorcentual();
            for (int i = 1; i <= 4; i++)
            {
                if (porcentaje <= probabilidad * i)
                {
                    return i - 1;
                }
            }
            return -1;
        }

        public void talkGuybrush(Characters gb, Talk talking)
        {
            if (!gb.gIsTalking)
            {
                string text = string.Empty;
                if (firstTalking)
                {
                    insult = 0;
                    text = Insults.FirstReply;
                }
                else if (lastTalking)
                {
                    text = Insults.GuybrushLastSentence;
                }
                else
                {
                    if (talking == Talk.Insult)
                    {
                        text = Insults.Insult[insult];
                    }
                    if (talking == Talk.Reply)
                    {
                        text = Insults.Reply[reply];
                    }
                }
                this.Add(new ElementString(BasicTextures.CargarFuente("EighthScreen/MonkeyFont"),
                    "Guybrush",
                    text,
                    new Vector2(gb.Posicion.X - 100,
                        gb.Posicion.Y - 30)));
                if (!gb.gEndTalking)
                {
                    gb.changeSprites(states.GTalking);
                }
            }
        }

        public void checkStake(Characters c, Talk talking)
        {
            if (insult == reply)
            {
                if (talking == Talk.Insult)
                    loosedStake = true;
                else
                    loosedStake = false;
            }
            else
            {
                if (talking == Talk.Insult)
                    loosedStake = false;
                else
                    loosedStake = true;
            }
            if (firstTalking)
                firstTalking = false;
            else
            {
                this.Clear();
                if (loosedStake)
                    if (c.contPirateStakes == 3)
                        c.changeSprites(states.PWin);
                    else
                        c.changeSprites(states.PAttack);
                else
                    if (c.contGuybrushStakes == 3)
                        c.changeSprites(states.GWin);
                    else
                        c.changeSprites(states.GAttack);
            }
        }

    }
}
