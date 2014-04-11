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
        public static readonly float HITBOX_SCALE = 0.9F;
        public Vector2 position { get; set; }
        public Point rectangleBounds { get; set; }
        public List<Tile> tiles;
        public Rectangle screenRectangle
        {
            get
            {
                return new Rectangle((int)position.X - HideOutGame.SCREEN_OFFSET_X, (int)position.Y - HideOutGame.SCREEN_OFFSET_Y, rectangleBounds.X, rectangleBounds.Y);
            }
        }

        public Rectangle worldRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, rectangleBounds.X, rectangleBounds.Y);
            }
        }

        public Rectangle collisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X + rectangleBounds.X * (1 - HITBOX_SCALE) / 2),
                                     (int)(position.Y + rectangleBounds.Y * (1 - HITBOX_SCALE) / 2),
                                     (int)(rectangleBounds.X * HITBOX_SCALE),
                                     (int)(rectangleBounds.Y * HITBOX_SCALE));
            }
        }

        public Texture2D sprite { get; set; }
        public Entity()
        {
            tiles = new List<Tile>();
        }

        public bool OnScreen()
        {
            return this.screenRectangle.X + this.screenRectangle.Width > 0 &&
                this.screenRectangle.Y + this.screenRectangle.Height > 0 &&
                this.screenRectangle.X <= HideOutGame.SCREEN_WIDTH &&
                this.screenRectangle.Y <= HideOutGame.SCREEN_HEIGHT;
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
