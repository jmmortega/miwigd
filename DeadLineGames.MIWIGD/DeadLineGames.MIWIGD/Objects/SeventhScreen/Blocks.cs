using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Design;

namespace DeadLineGames.MIWIGD.Objects.SeventhScreen
{
    class Blocks : AObjects
    {

        public Blocks()
            : base()
        { 

            float posX = (DesignOptions.Bounds.MaxX - DesignOptions.Bounds.MinX) / 9;
            float posY = 243;
            for(int i = 0; i < 3; i++)
            {
                this.Add(new Block(new Vector2((posX * ((i * 2) + 3)), posY)));
            }

        }

        public void updateBlocks(TimeSpan elapsed, Mario mario)
        {
            checkBlockCollisions(mario);
            base.Update(elapsed);
        }

        private void checkBlockCollisions(Mario mario)
        {
            foreach (Block b in this)
            {
                if (mario.HaveColision(b))
                {
                    if (mario.Posicion.Y >= (b.Posicion.Y + b.Height - 5))
                    {
                        if (!mario.lockDir[Direction.Left]
                            && !mario.lockDir[Direction.Right])
                        {
                            mario.upper = true;
                            if (b.actualFrame == 0)
                                b.activated = true;
                        }
                    }
                    else if (mario.Posicion.Y + mario.Height <= b.Posicion.Y + 5)
                    {
                        mario.lockDir[Direction.Left] = false;
                        mario.lockDir[Direction.Right] = false;
                        mario.lockDir[Direction.Down] = true;
                        mario.SetPosicion(mario.Posicion.X, b.Posicion.Y - mario.Height + 1);
                    }
                    else if (mario.Posicion.X + mario.Width <= b.Posicion.X + 5)
                    {
                        mario.lockDir[Direction.Left] = false;
                        mario.lockDir[Direction.Right] = true;
                        mario.lockDir[Direction.Down] = false;
                    }
                    else if (mario.Posicion.X >= (b.Posicion.X + b.Width - 5))
                    {
                        mario.lockDir[Direction.Left] = true;
                        mario.lockDir[Direction.Right] = false;
                        mario.lockDir[Direction.Down] = false;
                    }
                    break;
                }
                else 
                {
                    mario.lockDir[Direction.Left] = false;
                    mario.lockDir[Direction.Right] = false;
                    mario.lockDir[Direction.Down] = false;
                }
            }
        }


    }
}
