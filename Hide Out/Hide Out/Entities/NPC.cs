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
        public float moveSpeed { get; set; }
        public double rotateSpeed { get; set; }
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
                    rotateSpeed = .001;
                    moveSpeed = 5.0f;
                    break;
            }
        }

        public void MoveForward(GameTime gameTime)
        {
            Vector2 normVec = Normalize(vision.viewDirection);

            if(!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += normVec*moveSpeed*gameTime.ElapsedGameTime.Milliseconds;

            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;
        }

        public void MoveBackward(GameTime gameTime)
        {
            Vector2 normVec = Normalize(vision.viewDirection);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += -1*normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;
        }

        public void RotateLeft(GameTime gameTime) //positive to turn left, negative to turn right
        {
            vision.Rotate(rotateSpeed*gameTime.ElapsedGameTime.Milliseconds);
        }

        public void RotateRight(GameTime gameTime) //positive to turn left, negative to turn right
        {
            vision.Rotate(-1 * rotateSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }

        public bool CanSeePlayer(Player player, List<Obstacle> visibleObstacles)
        {
            return vision.CanSeePlayer(player, visibleObstacles);
        }

        public bool CanSee(Rectangle rect)
        {
            return vision.CanSee(rect);
        }

        public VertexPositionColor[] GetFieldOfViewTriangle()
        {
            return vision.GetFieldOfViewTriangle();
        }

        public VertexPositionColor[] GetFieldOfViewTriangleToDraw()
        {
            return vision.GetFieldOfViewTriangleToDraw();
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
                "Speed: " + this.moveSpeed + "\n";
            return retVal;
        }
    }
}
