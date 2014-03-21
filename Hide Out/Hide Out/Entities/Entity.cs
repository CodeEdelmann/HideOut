using System;
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

        public Point rectangleBounds { get; set; }
        public Rectangle drawRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, rectangleBounds.X, rectangleBounds.Y);
            }
        }
        public Texture2D sprite { get; set; }
        public Entity()
        {
        }
    }
}
