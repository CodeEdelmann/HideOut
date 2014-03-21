﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Hide_Out.Entities
{
    abstract class Entity
    {
        public Vector2 position { get; set; }
        public Rectangle rectangle { get; set; } //Needed for intersection/collision checks
        public Texture2D sprite { get; set; }
        public Entity()
        {
        }

        public override string ToString()
        {
            return base.ToString() +
                "Position: X - " + position.X + " Y - " + position.Y + "\n" +
                "Width: " + rectangle.Width + "\n" +
                "Height: " + rectangle.Height + "\n";
        }
    }
}
