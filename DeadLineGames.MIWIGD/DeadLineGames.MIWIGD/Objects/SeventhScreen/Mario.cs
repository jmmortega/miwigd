using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Class.Input;
using Microsoft.Xna.Framework.Input;
using NamoCode.Game.Class.Media;

namespace DeadLineGames.MIWIGD.Objects.SeventhScreen
{
    public class Mario : AObject
    {
        
        private const int TOTALJUMPTIME = 30;

        private float posYInit = 0;
        private int jumpTime = 0;
        private int jump = 0;
        private int oldJump = 0;
        private bool isOnScreen = false;
        public bool upper = false;
        public Dictionary<Direction, bool> lockDir;

        public delegate void HandleMarioIsOut(object sender, EventArgs e);
        public event HandleMarioIsOut OnMarioIsOut;

        public Mario(Vector2 posicion)
            : base(BasicTextures.GetTexture("Mario"), 
            "Mario",
            new FrameRateInfo(3, 0.13f, 4, false),
            posicion)
        {
            this.Frames.ChangeRow(2);
            this.posYInit = posicion.Y;

            lockDir = new Dictionary<Direction, bool>();
            lockDir.Add(Direction.Left, false);
            lockDir.Add(Direction.Right, false);
            lockDir.Add(Direction.Down, false);
        }

        public override void Update(TimeSpan elapsed)
        {
            if (!isOnScreen)
            {
                this.SetPosicion(this.Posicion.X + 0.50f, this.Posicion.Y);
                if (this.Posicion.X > DesignOptions.Bounds.MinX + 35)
                    isOnScreen = true;
            }
            else
            {
                Mover();
            }
            base.Update(elapsed);
        }

        public void Mover()
        {
            //int speed = 0;
            float posX = this.Posicion.X;
            float posY = this.Posicion.Y;
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickRight) ||
                InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Right))
            {
                if(jumpTime == 0)
                    this.Frames.ChangeRow(2);
                if(!this.lockDir[Direction.Right])
                    posX += 2;
            }
            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.LeftThumbstickLeft) ||
                InputState.GetInputState().KeyboardState.IsKeyDown(Keys.Left))
            {
                if(jumpTime == 0)
                    this.Frames.ChangeRow(3);
                if (!this.lockDir[Direction.Left])
                    posX -= 2;
            }

            if (InputState.GetInputState().GamepadOne.IsButtonDown(Buttons.A) ||
                InputState.GetInputState().KeyboardState.IsKeyDown(Keys.A))
            {
                if (!upper && jump == 0)
                    Player.Instance.Play("Jump");

                if (jumpTime <= TOTALJUMPTIME)
                    jumpTime += 5;
                switch (this.Frames.ActualFrameRow)
                {
                    case 2:
                        this.Frames.ChangeRow(0);
                        break;
                    case 3:
                        this.Frames.ChangeRow(1);
                        break;
                }
                this.Frames.Pause = true;
                this.Frames.ChangeFrame(1);
            }

            if (!upper)
            {
                if (jumpTime == 0 && oldJump != 0)
                {
                    this.Frames.Pause = true;
                    upper = true;
                }
                else
                    posY = Jumping();
            }
            else
            {
                if (!this.lockDir[Direction.Down])
                {
                    if (jump == 0 && oldJump != 0)
                    {
                        jump = oldJump;
                    }
                    posY = Falling();
                }
                else
                {
                    upper = false;
                    if(oldJump == 0)
                        oldJump = jump;
                    jump = 0;
                    jumpTime = 0;
                }
            }

            if (posX == this.Posicion.X && jumpTime == 0)
            {
                switch (this.Frames.ActualFrameRow)
                {
                    case 2:
                        this.Frames.ChangeRow(0);
                        break;
                    case 3:
                        this.Frames.ChangeRow(1);
                        break;
                }
                this.Frames.Pause = true;
                this.Frames.ChangeFrame(0);
            }
            else
            {
                if (jumpTime == 0 && jump == 0)
                    this.Frames.Pause = false;
                else
                    this.Frames.Pause = true;
            }

            if (base.Posicion.X > DesignOptions.Bounds.MaxX)
            {
                if (OnMarioIsOut != null)
                {
                    OnMarioIsOut(this, new EventArgs());
                }
            }

            this.SetPosicion(posX, posY);
        }
        
        private float Jumping()
        {
            float posY = this.Posicion.Y;
            if (jumpTime > 0)
            {
                if (jump++ <= jumpTime)
                {
                    posY = this.Posicion.Y - (5 - ((jump * 10) / 100));
                }
                else
                {
                    upper = true;
                }
            }

            return posY;
        }


        private float Falling()
        {
            float posY = this.Posicion.Y;
            if (posY <= posYInit)
            {
                posY = this.Posicion.Y + (5 - ((--jump * 20) / 100));
            }
            else
            {
                posY = posYInit;
                jump = 0;
                oldJump = 0;
                jumpTime = 0;
                upper = false;
            }
            return posY;
        }

    }
}
