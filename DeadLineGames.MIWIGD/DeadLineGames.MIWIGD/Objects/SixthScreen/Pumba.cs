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

namespace DeadLineGames.MIWIGD.Objects.SixthScreen
{

    class Pumba : AObject
    {

        private const int MOVIMIENTO = 3;

        private Dictionary<PumbaState, FrameRateInfo> frames;
        private bool isEating = false;
        private bool isBurping = false;
        private int actuaFrame = 0;

        public PumbaState estado = PumbaState.Idle;

        public Pumba(Vector2 posicion)
            : base(BasicTextures.GetTexture("idle"),
            "Pumba",
            new FrameRateInfo(4, 0.15f, 1, false),
            posicion)
        {
            frames = new Dictionary<PumbaState, FrameRateInfo>();
            frames.Add(PumbaState.Idle, this.Frames);
            frames.Add(PumbaState.Eating, new FrameRateInfo(4, 0.15f, 1, false));
            frames.Add(PumbaState.StartRun, new FrameRateInfo(3, 0.15f, 1, false));
            frames.Add(PumbaState.Run, new FrameRateInfo(10, 0.15f, 1, false));
            frames.Add(PumbaState.Turn, new FrameRateInfo(3, 0.25f, 1, false));
            frames.Add(PumbaState.Burp, new FrameRateInfo(11, 0.15f, 1, false));

            cambiarFrame();

        }

        public override void Update(TimeSpan elapsed)
        {
            if (estado != PumbaState.StartRun && estado != PumbaState.Turn) actuaFrame = this.ActualFrame;
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
                if (estado != PumbaState.Idle)
                {
                    estado = PumbaState.Turn;
                    cambiarEstado(0);
                }
                if (isBurping)
                {
                    estado = PumbaState.Burp;
                    cambiarEstado(0);
                }
            }

            cambiarFrame();

            base.Update(elapsed);
        }

        private void cambiarEstado(int Movimiento)
        {

            float X = 0;

            switch (estado)
            {
                case PumbaState.Idle:
                    if (!isEating)
                        estado = PumbaState.StartRun;
                    else
                        estado = PumbaState.Eating;
                    break;

                case PumbaState.Eating:
                case PumbaState.Burp:
                    if (this.ActualFrame == this.Frames.FrameCount - 1)
                        estado = PumbaState.Idle;
                    break;

                case PumbaState.StartRun:
                    if (this.ActualFrame == this.Frames.FrameCount - 1)
                        estado = PumbaState.Run;
                    else
                    {
                        if (actuaFrame != this.ActualFrame)
                        {
                            actuaFrame = this.ActualFrame;
                            if (this.SpriteEffect == SpriteEffects.FlipHorizontally)
                            {
                                X = this.Posicion.X - (actuaFrame + 1);
                            }
                            else if (this.SpriteEffect == SpriteEffects.None)
                            {
                                X = this.Posicion.X + (actuaFrame + 1);
                            }
                        }
                    }
                    break;

                case PumbaState.Run:
                    X = this.Posicion.X + Movimiento;
                    break;

                case PumbaState.Turn:
                    if (this.ActualFrame == this.Frames.FrameCount - 1)
                    {
                        estado = PumbaState.Idle;
                    }
                    else
                    {
                        if (this.ActualFrame == 0 || actuaFrame != this.ActualFrame)
                        {
                            actuaFrame = this.ActualFrame;
                            if (this.SpriteEffect == SpriteEffects.FlipHorizontally)
                            {
                                X = this.Posicion.X - (MOVIMIENTO - actuaFrame);
                            }
                            else if (this.SpriteEffect == SpriteEffects.None)
                            {
                                X = this.Posicion.X + (MOVIMIENTO - actuaFrame);
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
                case PumbaState.Idle:
                    if (this.Texture != BasicTextures.GetTexture("idle"))
                    {
                        this.Texture = BasicTextures.GetTexture("idle");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case PumbaState.Eating:
                    if (this.Texture != BasicTextures.GetTexture("idle"))
                    {
                        this.Texture = BasicTextures.GetTexture("eating");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case PumbaState.StartRun:
                    if (this.Texture != BasicTextures.GetTexture("startrun"))
                    {
                        this.Texture = BasicTextures.GetTexture("startrun");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case PumbaState.Run:
                    if (this.Texture != BasicTextures.GetTexture("run"))
                    {
                        this.Texture = BasicTextures.GetTexture("run");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case PumbaState.Turn:
                    if (this.Texture != BasicTextures.GetTexture("turn"))
                    {
                        this.Texture = BasicTextures.GetTexture("turn");
                        this.Frames = frames[estado];
                        this.Frames.Restart();
                    }
                    break;
                case PumbaState.Burp:
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