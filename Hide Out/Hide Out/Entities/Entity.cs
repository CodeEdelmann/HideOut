using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HideOut.Primitives;


namespace HideOut.Entities
{
    abstract class Entity
    {
        public Vector2 position { get; set; }
        public Point rectangleBounds { get; set; }
        public List<Tile> tiles;
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
            tiles = new List<Tile>();
        }

        public override string ToString()
        {
            return base.ToString() +
                "Position: X - " + position.X + " Y - " + position.Y + "\n" +
                "Width: " + this.rectangleBounds.X + "\n" +
                "Height: " + this.rectangleBounds.Y + "\n";
        }
    }
}
