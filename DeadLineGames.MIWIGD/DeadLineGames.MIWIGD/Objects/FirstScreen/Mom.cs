using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NamoCode.Game.Utils;

namespace DeadLinesGames.MIWIGD.Objects.FirstScreen
{
    public class Mom : AObject
    {
        public Mom(Texture2D texture, string name, Vector2 posicion, FrameRateInfo frame, Bounds bounds)
            : base(texture, name, posicion,frame, bounds)
        {}
        
            
            public void TakeAnimation(EnumMovement Movement)
            {
                if(Movement == EnumMovement.Abajo)
                {
                    this.Frames.ChangeFrame(1);
                }
                else if(Movement == EnumMovement.Arriba)
                {
                    this.Frames.ChangeFrame(0);
                }
                else if(Movement == EnumMovement.Izquierda)
                {
                    this.SpriteEffect = SpriteEffects.FlipHorizontally;
                    this.Frames.ChangeFrame(2);
                }
                else if(Movement == EnumMovement.Derecha)                
                {
                    this.Frames.ChangeFrame(2);
                }            
            }        
    }         
}

 