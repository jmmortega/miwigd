using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using NamoCode.Game.Class.Design;

namespace DeadLineGames.MIWIGD.Objects.SecondScreen
{
    public class Link : AObject
    {

        private const float MOVIMIENTO = 2.5f;

        public Direction dir;
        public List<Direction> blockedDir;

        public Link(Vector2 posicion)
            : base(BasicTextures.CargarTextura("SecondScreen/Link", "Link"),
            "Link",
            new FrameRateInfo(2, 0.20f, 4, false),
            posicion)
        {
            dir = Direction.Up;
            blockedDir = new List<Direction>();
        }

        public override void Update(TimeSpan elapsed)
        {
            mover();
            orientar();
            base.Update(elapsed);
        }

        private void mover()
        {

            float X = this.Posicion.X, Y = this.Posicion.Y;
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickUp))
            {
                dir = Direction.Up;
                bool block = false;
                foreach (Direction d in blockedDir)
                    if (dir == d)
                        block = true;
                if(!block)
                    Y -= MOVIMIENTO;
            }
            else if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickDown))
            {
                dir = Direction.Down;
                bool block = false;
                foreach (Direction d in blockedDir)
                    if (dir == d)
                        block = true;
                if (!block)
                    Y += MOVIMIENTO;
            }
            else if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                dir = Direction.Left;
                bool block = false;
                foreach (Direction d in blockedDir)
                    if (dir == d)
                        block = true;
                if (!block)
                    X -= MOVIMIENTO;
            }
            else if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                dir = Direction.Right;
                bool block = false;
                foreach (Direction d in blockedDir)
                    if (dir == d)
                        block = true;
                if (!block)
                    X += MOVIMIENTO;
            }
            if (X == this.Posicion.X && Y == this.Posicion.Y)
            {
                this.Frames.Pause = true;
                this.Frames.ActualFrame = 0;
            }
            else
                this.Frames.Pause = false;

            if (X > DesignOptions.Bounds.MinX && X < (DesignOptions.Bounds.MaxX - this.Width))
                this.Posicion = new Vector2(X, this.Posicion.Y);
            if (Y > DesignOptions.Bounds.MinY && Y < (DesignOptions.Bounds.MaxY - this.Height))
                this.Posicion = new Vector2(this.Posicion.X, Y);
            
        }

        private void orientar()
        {
            switch (dir)
            {
                case Direction.Up:
                    this.Frames.ChangeRow(0);
                    break;
                case Direction.Down:
                    this.Frames.ChangeRow(1);
                    break;
                case Direction.Left:
                    this.Frames.ChangeRow(2);
                    break;
                case Direction.Right:
                    this.Frames.ChangeRow(3);
                    break;
            }
        }

    }
}
