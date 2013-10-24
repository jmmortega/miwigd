using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Class.Media;

namespace DeadLineGames.MIWIGD.Objects.EighthScreen
{

    public enum states { Start, Idle, GTalking, PTalking, GAttack, PAttack, GWin, PWin };

    class Characters : AnimatedElement
    {

        private const int TALKINGTIME = 200;
        private int talks = 0;

        public int contGuybrushStakes = 0;
        public int contPirateStakes = 0;

        public bool hasStarted = false;
        public bool gIsTalking = false;
        public bool pIsTalking = false;
        public bool gEndTalking = false;
        public bool pEndTalking = false;
        public bool isFighting = false;
        public bool isLoosing = false;
        public bool isLoosed = false;

        private int frames = 0;
        private int contFrames = 0;
        private int actFrame = 0;

        private DialogGenerator dg;

        public Characters(Vector2 position)
            : base(BasicTextures.GetTexture("start"),
            "Characters",
            position,
            new FrameRateInfo(6, 0.15f, 1, false))
        {
            dg = new DialogGenerator(new Vector2());
        }

        public override void Update(TimeSpan elapsedtime)
        {
            if (!hasStarted)
            {
                if (this.ActualFrame == this.Frames.FrameCount - 1)
                {
                    hasStarted = true;
                }
            }
            else
            {
                if (pIsTalking)
                {
                    if (talks < TALKINGTIME)
                        talks++;
                    else
                    {
                        talks = 0;
                        pEndTalking = true;
                        pIsTalking = false;
                    }
                }
                if (gIsTalking)
                {
                    if (talks < TALKINGTIME)
                        talks++;
                    else
                    {
                        talks = 0;
                        gEndTalking = true;
                        gIsTalking = false;
                    }
                }
                if (isFighting || isLoosing)
                {
                    if (this.Frames.TotalElapsed < this.Frames.SecondPerFrame)
                    {
                        if (contFrames < frames - 1)
                        {
                            if (actFrame != this.ActualFrame)
                            {
                                if (isLoosing && contPirateStakes == 4)
                                {
                                    if (this.Frames.ActualFrameRow == 1
                                        && this.ActualFrame == 4)
                                    {
                                        dg.firstTalking = false;
                                        dg.lastTalking = true;
                                        this.gIsTalking = false;
                                        this.Posicion = Screens.EighthScreen.position;
                                        dg.talkGuybrush(this, Talk.None);
                                    }
                                }
                                if (isLoosing && contGuybrushStakes == 4)
                                {
                                    if (this.ActualFrame == 5)
                                    {
                                        dg.firstTalking = false;
                                        dg.lastTalking = true;
                                        this.pIsTalking = false;
                                        this.Posicion = Screens.EighthScreen.position;
                                        dg.talkPirate(this, Talk.None);
                                    }
                                }
                                actFrame = this.ActualFrame;
                                contFrames++;
                            }
                        }
                        else
                        {
                            if (isLoosing)
                            {
                                isLoosed = true;
                                this.Pause();
                            }
                            else
                                isFighting = false;
                            contFrames = 0;
                            actFrame = 0;
                        }
                    }
                }
            }
            base.Update(elapsedtime);
        }

        public void changeSprites(states state)
        {
            switch (state)
            {
                case states.Idle:
                    this.Textura = BasicTextures.GetTexture("idle");
                    this.Frames = new FrameRateInfo(1, 0, 1, false);
                    break;
                case states.GTalking:
                    this.Textura = BasicTextures.GetTexture("guybrush-speak");
                    this.Frames = new FrameRateInfo(5, 0.15f, 1, false);
                    gIsTalking = true;
                    break;
                case states.PTalking:
                    this.Textura = BasicTextures.GetTexture("chema-speak");
                    this.Frames = new FrameRateInfo(6, 0.15f, 1, false);
                    pIsTalking = true;
                    break;
                case states.GAttack:
                    this.Textura = BasicTextures.GetTexture("guybrush-attack");
                    this.Frames = new FrameRateInfo(8, 0.20f, 2, true);
                    frames = 10;
                    isFighting = true;
                    contGuybrushStakes++;
                    Player.Instance.Play("Swords");
                    break;
                case states.PAttack:
                    this.Textura = BasicTextures.GetTexture("chema-attack");
                    this.Frames = new FrameRateInfo(8, 0.20f, 2, true);
                    frames = 9;
                    isFighting = true;
                    contPirateStakes++;
                    Player.Instance.Play("Swords");
                    break;
                case states.GWin:
                    this.Textura = BasicTextures.GetTexture("guybrush-win");
                    this.Frames = new FrameRateInfo(8, 0.20f, 2, true);
                    frames = 12;
                    isLoosing = true;
                    contGuybrushStakes++;
                    Player.Instance.Play("Swords");
                    break;
                case states.PWin:
                    this.Textura = BasicTextures.GetTexture("chema-win");
                    this.Frames = new FrameRateInfo(8, 0.20f, 3, true);
                    frames = 18;
                    isLoosing = true;
                    contPirateStakes++;
                    Player.Instance.Play("Swords");
                    break;
            }
            this.Restart();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            dg.Draw(sprite);
            base.Draw(sprite);
        }

    }
}
