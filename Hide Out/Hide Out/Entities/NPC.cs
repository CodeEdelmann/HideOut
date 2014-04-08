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
        public List<Vision> visions { get; set; }
        public float moveSpeed { get; set; }
        public double rotateSpeed { get; set; }
        public NPCType tag { get; set; }

        public NPC() : base()
        {
            visions = new List<Vision>();
        }

        public void MoveForward(GameTime gameTime)
        {
            Vector2 normVec = Normalize(vision.viewDirection);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds;

            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;

            for (int i = 0; i < this.visions.Count; i++)
            {
                visions[i].parentLocation = this.worldRectangle;
            }
        }

        public void MoveBackward(GameTime gameTime)
        {
            Vector2 normVec = Normalize(vision.viewDirection);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += -1 * normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            
            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;

            for (int i = 0; i < this.visions.Count; i++)
            {
                visions[i].parentLocation = this.worldRectangle;
            }
        }

        public void RotateLeft(GameTime gameTime) //positive to turn left, negative to turn right
        {
            vision.Rotate(rotateSpeed * gameTime.ElapsedGameTime.Milliseconds);
            foreach (Vision v in this.visions)
            {
                v.Rotate(rotateSpeed * gameTime.ElapsedGameTime.Milliseconds);
            }
        }

        public void RotateRight(GameTime gameTime) //positive to turn left, negative to turn right
        {
            vision.Rotate(-1 * rotateSpeed * gameTime.ElapsedGameTime.Milliseconds);
            foreach (Vision v in this.visions)
            {
                v.Rotate(-1 * rotateSpeed * gameTime.ElapsedGameTime.Milliseconds);
            }
        }

        public bool CanSeePlayer(Player player, List<Obstacle> visibleObstacles)
        {
            return vision.CanSeePlayer(player, visibleObstacles);
        }

        public bool CanSee(Rectangle rect)
        {
            return vision.CanSee(rect);
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
                "Speed: " + this.moveSpeed + "\n";
            return retVal;
        }
    }
}
