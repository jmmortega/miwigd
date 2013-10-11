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

        private float minBoundY = 0;

        public Direction dir;
        public List<Direction> blockedDir;
        public bool HaveWeapon = false;

        public Link(Vector2 posicion, float minBoundY)
            : base(BasicTextures.CargarTextura("SecondScreen/Link", "Link"),
            "Link",
            new FrameRateInfo(2, 0.20f, 4, false),
            posicion)
        {
            dir = Direction.Up;
            blockedDir = new List<Direction>();
            this.minBoundY = minBoundY;
        }

        public override void Update(TimeSpan elapsed)
        {
            if (!HaveWeapon)
            {
                mover();
                orientar();
            }
            else
            {
                if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.A) ||
                    InputState.GetInputState().KeyboardState.IsKeyDown(Keys.A))
                    catchWeapon(0);
            }
            base.Update(elapsed);
        }

        private void mover()
        {

            float X = this.Posicion.X, Y = this.Posicion.Y;
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickUp) ||
                InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Up))
            {
                dir = Direction.Up;
                bool block = false;
                foreach (Direction d in blockedDir)
                    if (dir == d)
                        block = true;
                if(!block)
                    Y -= MOVIMIENTO;
            }
            else if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickDown) ||
                InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Down))
            {
                dir = Direction.Down;
                bool block = false;
                foreach (Direction d in blockedDir)
                    if (dir == d)
                        block = true;
                if (!block)
                    Y += MOVIMIENTO;
            }
            else if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickLeft) ||
                InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Left))
            {
                dir = Direction.Left;
                bool block = false;
                foreach (Direction d in blockedDir)
                    if (dir == d)
                        block = true;
                if (!block)
                    X -= MOVIMIENTO;
            }
            else if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickRight) ||
                InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Right))
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
            if (Y > minBoundY && Y < (DesignOptions.Bounds.MaxY - this.Height))
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

        public void catchWeapon(int typeCatch)
        {
            switch (typeCatch)
            {
                case 0:
                    this.Texture = BasicTextures.GetTexture("Link");
                    this.Frames = new FrameRateInfo(2, 0.20f, 4, false);
                    HaveWeapon = false;
                    break;
                case 1:
                    this.Texture = BasicTextures.CargarTextura("SecondScreen/LinkSword", "LinkSword");
                    this.Frames = new FrameRateInfo(1, 0, 1, false);
                    HaveWeapon = true;
                    break;
                case 2:
                    this.Texture = BasicTextures.CargarTextura("SecondScreen/LinkObject", "LinkObject");
                    this.Frames = new FrameRateInfo(1, 0, 1, false);
                    HaveWeapon = true;
                    break;
            }
        }

    }
}
