using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;
using Microsoft.Xna.Framework.Graphics;

namespace DeadLineGames.MIWIGD.Objects.SixthScreen
{
    class Timon : AObject
    {

        private int TIMETOTHROWBUG = 50;
        private int TIMETOGETCRAZY = Azar.Instance.GetNumber(50, 100);

        private Dictionary<CharsState, FrameRateInfo> frames;
        private CharsState estado = CharsState.Idle;
        private Direction dir = Direction.Right;
        private int timeToThrow = 1;
        private int contTurn = 0;
        private int actualCont = 0;

        private int timeToCrazy = 0;

        public Timon(Vector2 posicion)
            : base(BasicTextures.GetTexture("Tidle"),
            "Timon",
            new FrameRateInfo(1, 0, 1, false),
            posicion)
        {
            frames = new Dictionary<CharsState, FrameRateInfo>();
            frames.Add(CharsState.Idle, this.Frames);
            frames.Add(CharsState.Run, new FrameRateInfo(8, 0.12f, 1, false));
            frames.Add(CharsState.Turn, new FrameRateInfo(6, 0.05f, 1, false));

            cambiarFrame();
        }

        public override void Update(TimeSpan elapsed)
        {
            crazyDir();
            cambiarEstado();
            cambiarFrame();
            throwBug();
   
            base.Update(elapsed);
        }

        private void crazyDir()
        {
            timeToCrazy++;
            if (timeToCrazy % TIMETOGETCRAZY == 0)
            {
                estado = CharsState.Turn;
                if (dir == Direction.Left) dir = Direction.Right;
                else if (dir == Direction.Right) dir = Direction.Left;
                TIMETOGETCRAZY = Azar.Instance.GetNumber(80, 200);
                timeToCrazy = 0;
            }
        }

        private void cambiarEstado()
        {
            float X = this.Posicion.X;
            switch (estado)
            {
                case CharsState.Idle:
                    if (timeToThrow % 10 == 0)
                    {
                        estado = CharsState.Run;
                        TIMETOTHROWBUG = Azar.Instance.GetNumber(50, 200);
                    }
                    break;
                case CharsState.Run:
                    if (dir == Direction.Right)
                    {
                        X += 7; 
                    }
                    if (dir == Direction.Left)
                    {
                        X -= 7;
                    }
                    break;
                case CharsState.Turn:
                    if (this.ActualFrame == this.Frames.FrameCount - 1)
                    {
                        if (contTurn == 2)
                        {
                            contTurn = 0;
                            estado = CharsState.Run;
                            if (dir == Direction.Right)
                                this.SpriteEffect = SpriteEffects.None;
                            else if (dir == Direction.Left)
                                this.SpriteEffect = SpriteEffects.FlipHorizontally;
                        }
                        actualCont = contTurn;
                    }
                    else if (this.ActualFrame == 0 && actualCont == contTurn)
                        contTurn++;
                    break;
            }

            if (X >= DesignOptions.Bounds.MinX && X <= DesignOptions.Bounds.MaxX - this.Width)
                this.SetPosicion(X, this.Posicion.Y);
            else if (X < DesignOptions.Bounds.MinX)
            {
                estado = CharsState.Turn;
                dir = Direction.Right;
            }
            else if (X > DesignOptions.Bounds.MaxX - this.Width)
            {
                estado = CharsState.Turn;
                dir = Direction.Left;
            }
        }

        private void cambiarFrame()
        {
            switch (estado)
            {
                case CharsState.Idle:
                    if (this.Texture != BasicTextures.GetTexture("Tidle"))
                    {
                        this.Texture = BasicTextures.GetTexture("Tidle");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case CharsState.Run:
                    if (this.Texture != BasicTextures.GetTexture("Trun"))
                    {
                        this.Texture = BasicTextures.GetTexture("Trun");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case CharsState.Turn:
                    if (this.Texture != BasicTextures.GetTexture("Tturn"))
                    {
                        this.Texture = BasicTextures.GetTexture("Tturn");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                        this.Frames.TypePlaying = EnumPlaying.OnePlay;
                    }
                    break;
            }
        }

        private void throwBug()
        {
            if (timeToThrow % TIMETOTHROWBUG == 0)
            {
                Bugs.Instance.Add(new Bug("Bug_" + randomBug(),
                    new Vector2(this.Center.X - 15, this.Posicion.Y + this.Height)));
                TIMETOTHROWBUG = Azar.Instance.GetNumber(50, 200);
                timeToThrow = 0;
            }
            timeToThrow++;
        }

        private int randomBug() 
        {
            double probabilidad = 1 / 7.0;
            double porcentaje = Azar.Instance.GetPorcentual();
            for (int i = 1; i <= 7; i++)
            {
                if (porcentaje <= probabilidad * i)
                    return i - 1;
            }
            return -1;
        }

    }
}
