﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework;
using NamoCode.Game.Class.Design;
using NamoCode.Game.Class.Media;

namespace DeadLineGames.MIWIGD.Objects.SeventhScreen
{
    class Blocks : AObjects
    {
        public delegate void HandleCoinUp(object sender, EventArgs e);
        public event HandleCoinUp OnCoinUp;

        public Blocks()
            : base()
        {                             
            float posX = (DesignOptions.Bounds.MaxX - DesignOptions.Bounds.MinX) / 9;
            float posY = 243;
            for(int i = 0; i < 3; i++)
            {
                //this.Add(new Coin("Coin_Block_" + i, new Vector2((posX * ((i * 2) + 3)), posY))); 

                var coin = new Coin("Coin_Block_" + i, new Vector2((posX * ((i * 2) + 3)), posY));
                coin.OnCoinUp += new Coin.HandleCoinUp(coin_OnCoinUp);

                this.Add(coin);

                this.Add(new Block("Block_" + i, new Vector2((posX * ((i * 2) + 3)), posY)));
            }

        }

        private void coin_OnCoinUp(object sender, EventArgs e)
        {
            if (OnCoinUp != null)
            {
                OnCoinUp(sender, new EventArgs());
            }
        }

        

        public void updateBlocks(TimeSpan elapsed, Mario mario)
        {
            checkBlockCollisions(mario);
            base.Update(elapsed);
        }

        private void checkBlockCollisions(Mario mario)
        {
            foreach (AObject b in this)
            {
                if (b.GetType().Name == "Block")
                {
                    Block bloque = (Block)b;
                    if (mario.HaveColision(b))
                    {
                        if (mario.Posicion.Y >= (b.Posicion.Y + b.Height - 5)
                            || mario.Posicion.Y >= (b.Posicion.Y + b.Height - 15))
                        {
                            if (!mario.lockDir[Direction.Left]
                                && !mario.lockDir[Direction.Right])
                            {
                                if (mario.Posicion.Y >= (b.Posicion.Y + b.Height - 15))
                                    mario.SetPosicion(mario.Posicion.X, b.Posicion.Y + b.Height);
                                mario.upper = true;
                                if (bloque.actualFrame == 0)
                                {
                                    bloque.activated = true;
                                    foreach (AObject c in this)
                                    {
                                        if (c.Name == "Coin_" + bloque.Name)
                                        {
                                            Coin moneda = (Coin)c;
                                            moneda.activated = true;
                                            break;
                                        }
                                    }
                                    Player.Instance.Play("Coin");
                                }
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
                        else if (mario.Posicion.X >= (b.Posicion.X + b.Width - 5)
                            || mario.Posicion.X >= (b.Posicion.X + b.Width - 15))
                        {
                            mario.lockDir[Direction.Left] = true;
                            mario.lockDir[Direction.Right] = false;
                            mario.lockDir[Direction.Down] = false;
                            if(mario.Posicion.X >= (b.Posicion.X + b.Width - 15))
                                mario.SetPosicion(b.Posicion.X + b.Width, mario.Posicion.Y);
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
}
