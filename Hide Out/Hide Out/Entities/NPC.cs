using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HideOut.Entities
{
    public enum NPCType { Police, Bird, Squirrel, Child };
    class NPC : Entity
    {
        public Vision vision { get; set; }
        public int speed { get; set; }
        public NPCType tag { get; set; }

        public NPC() : base()
        {

        }

        public NPC(NPCType type, Vector2 pos, Vector2 facing, Texture2D spr)
        {
            position = pos;
            sprite = spr;
            tag = type;
            switch (type) {
                case NPCType.Police:
                    vision = new Vision(this.screenRectangle, 50.0f, 1, facing, Color.Red);
                    speed = 1;
                    break;
            }
        }

        public void Move(Vector2 direction)
        {
            Vector2 normVec = Normalize(direction);
            position += normVec*speed;
            Rectangle tempRec = this.screenRectangle;
            tempRec.X = (int) position.X;
            tempRec.Y = (int) position.Y;
            Vision tempVis = vision;
            tempVis.parentLocation = this.screenRectangle;
        }

        public void Rotate(double angle) //positive to turn left, negative to turn right
        {
            vision.Rotate(angle);
        }

        public bool CanSeePlayer(Rectangle rect, List<Obstacle> visibleObstacles)
        {
            return vision.CanSeePlayer(rect, visibleObstacles);
        }

        public bool CanSee(Rectangle rect)
        {
            return vision.CanSee(rect);
        }

        public VertexPositionColor[] GetFieldOfViewTriangle()
        {
            return vision.GetFieldOfViewTriangle();
        }

        private Vector2 Normalize(Vector2 v)
        {
            float distance = Distance(v, new Vector2(0, 0));
            return new Vector2(v.X / distance, v.Y / distance);
        }

        private float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public override string ToString()
        {
            string retVal = base.ToString() +
                "Type: " + this.tag + "\n" +
                "Vision: " + this.vision + "\n" +
                "Speed: " + this.speed + "\n";
            return retVal;
        }
    }
}
