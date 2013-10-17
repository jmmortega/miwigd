using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using NamoCode.Game.Utils;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using NamoCode.Game.Class.Design;
using DeadLineGames.MIWIGD.Screens;

namespace DeadLineGames.MIWIGD.Objects.SixthScreen
{

    class Pumba : AObject
    {

        private const int MOVIMIENTO = 4;

        private Dictionary<CharsState, FrameRateInfo> frames;
        private int actuaFrame = 0;

        public CharsState estado = CharsState.Idle;
        public bool isEating = false;
        public bool isBurping = false;

        public Pumba(Vector2 posicion)
            : base(BasicTextures.GetTexture("idle"),
            "Pumba",
            new FrameRateInfo(4, 0.15f, 1, false),
            posicion)
        {
            frames = new Dictionary<CharsState, FrameRateInfo>();
            frames.Add(CharsState.Idle, this.Frames);
            frames.Add(CharsState.Eating, new FrameRateInfo(4, 0.15f, 1, false));
            frames.Add(CharsState.StartRun, new FrameRateInfo(3, 0.15f, 1, false));
            frames.Add(CharsState.Run, new FrameRateInfo(10, 0.15f, 1, false));
            frames.Add(CharsState.Turn, new FrameRateInfo(3, 0.25f, 1, false));
            frames.Add(CharsState.Burp, new FrameRateInfo(11, 0.15f, 1, false));

            cambiarFrame();

        }

        public override void Update(TimeSpan elapsed)
        {
            if (estado != CharsState.StartRun && estado != CharsState.Turn) actuaFrame = this.ActualFrame;
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickLeft)
                || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Left))
            {
                this.SpriteEffect = SpriteEffects.FlipHorizontally;
                cambiarEstado(-MOVIMIENTO);
            }
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickRight)
                || InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Right))
            {
                this.SpriteEffect = SpriteEffects.None;
                cambiarEstado(MOVIMIENTO);
            }

            if ((InputState.GetInputState().GamepadOne.IsButtonUp(Buttons.LeftThumbstickLeft)
                && InputState.GetInputState().KeyboardState.IsKeyUp(Keys.Left))
                && (InputState.GetInputState().GamepadOne.IsButtonUp(Buttons.LeftThumbstickRight)
                && InputState.GetInputState().KeyboardState.IsKeyUp(Keys.Right)))
            {
                if (estado != CharsState.Idle 
                    && estado != CharsState.Eating)
                    estado = CharsState.Turn;
                
                cambiarEstado(0);
            }

            cambiarFrame();

            base.Update(elapsed);
        }

        private void cambiarEstado(int Movimiento)
        {

            float X = this.Posicion.X;

            switch (estado)
            {
                case CharsState.Idle:
                    if (Movimiento != 0 && !isEating && !isBurping)
                        estado = CharsState.StartRun;
                    else if(isEating)
                        estado = CharsState.Eating;
                    else if (isBurping)
                        estado = CharsState.Burp;
                    break;

                case CharsState.Eating:
                case CharsState.Burp:
                    if (this.ActualFrame == this.Frames.FrameCount - 1)
                    {
                        estado = CharsState.Idle;
                        isEating = false;
                        isBurping = false;
                    }
                    break;

                case CharsState.StartRun:
                    if (this.ActualFrame == this.Frames.FrameCount - 1)
                        estado = CharsState.Run;
                    else
                    {
                        if (actuaFrame != this.ActualFrame)
                        {
                            actuaFrame = this.ActualFrame;
                            if (this.SpriteEffect == SpriteEffects.FlipHorizontally)
                            {
                                X -= (actuaFrame + 1);
                            }
                            else if (this.SpriteEffect == SpriteEffects.None)
                            {
                                X += (actuaFrame + 1);
                            }
                        }
                    }
                    break;

                case CharsState.Run:
                    X = this.Posicion.X + Movimiento;
                    break;

                case CharsState.Turn:
                    if (this.ActualFrame == this.Frames.FrameCount - 1)
                    {
                        estado = CharsState.Idle;
                    }
                    else
                    {
                        if (this.ActualFrame == 0 || actuaFrame != this.ActualFrame)
                        {
                            actuaFrame = this.ActualFrame;
                            if (this.SpriteEffect == SpriteEffects.FlipHorizontally)
                            {
                                X -= (MOVIMIENTO - actuaFrame);
                            }
                            else if (this.SpriteEffect == SpriteEffects.None)
                            {
                                X += (MOVIMIENTO - actuaFrame);
                            }
                        }
                    }
                    break;

            }

            if (X >= DesignOptions.Bounds.MinX && X <= DesignOptions.Bounds.MaxX - this.Width)
                this.SetPosicion(X, this.Posicion.Y);

        }

        private void cambiarFrame()
        {
            switch (estado)
            {
                case CharsState.Idle:
                    if (this.Texture != BasicTextures.GetTexture("idle"))
                    {
                        this.Texture = BasicTextures.GetTexture("idle");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case CharsState.Eating:
                    if (this.Texture != BasicTextures.GetTexture("eating"))
                    {
                        this.Texture = BasicTextures.GetTexture("eating");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case CharsState.StartRun:
                    if (this.Texture != BasicTextures.GetTexture("startrun"))
                    {
                        this.Texture = BasicTextures.GetTexture("startrun");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case CharsState.Run:
                    if (this.Texture != BasicTextures.GetTexture("run"))
                    {
                        this.Texture = BasicTextures.GetTexture("run");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case CharsState.Turn:
                    if (this.Texture != BasicTextures.GetTexture("turn"))
                    {
                        this.Texture = BasicTextures.GetTexture("turn");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case CharsState.Burp:
                    if (this.Texture != BasicTextures.GetTexture("burp"))
                    {
                    this.Texture = BasicTextures.GetTexture("burp");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
            }
        }

    }
}