﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamoCode.Game.Class.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DeadLineGames.MIWIGD.Objects.EigthScreen
{
    public class MailBox : AObject
    {
        public MailBox(Texture2D textura, String name, Vector2 posicion)
            : base(textura, name, posicion)
        { }
    }
}
