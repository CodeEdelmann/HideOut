using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HideOut.Entities
{
    public enum NPCType { PoliceA, PoliceB, Bird, Squirrel, Child };
    public enum NPCState { PatrolLeft, PatrolRight, PatrolLeftBackwards, PatrolRightBackwards, Chase, Investigate };

    class NPC : Entity
    {
        public Vision vision { get; set; }
        public List<Vision> visions { get; set; }
        public float moveSpeed { get; set; }
        public double rotateSpeed { get; set; }
        public NPCType tag { get; set; }
        public NPCState state { get; set; }
        public int stateTime;


        public NPC() : base()
        {
            visions = new List<Vision>();
        }

        public void MoveForward(GameTime gameTime, float speedMod)
        {
            Vector2 normVec = Normalize(vision.viewDirection);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod;

            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;

            for (int i = 0; i < this.visions.Count; i++)
            {
                visions[i].parentLocation = this.worldRectangle;
            }
        }

        public void MoveBackward(GameTime gameTime, float speedMod)
        {
            Vector2 normVec = Normalize(vision.viewDirection);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += -1 * normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod;
            
            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;

            for (int i = 0; i < this.visions.Count; i++)
            {
                visions[i].parentLocation = this.worldRectangle;
            }
        }

        public void MoveUp(GameTime gameTime, float speedMod)
        {
            Vector2 normVec = new Vector2(0,-1);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod;

            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;

            for (int i = 0; i < this.visions.Count; i++)
            {
                visions[i].parentLocation = this.worldRectangle;
            }
        }

        public void MoveDown(GameTime gameTime, float speedMod)
        {
            Vector2 normVec = new Vector2(0, 1);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod;

            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;

            for (int i = 0; i < this.visions.Count; i++)
            {
                visions[i].parentLocation = this.worldRectangle;
            }
        }

        public void MoveLeft(GameTime gameTime, float speedMod)
        {
            Vector2 normVec = new Vector2(-1, 0);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod;

            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;

            for (int i = 0; i < this.visions.Count; i++)
            {
                visions[i].parentLocation = this.worldRectangle;
            }
        }

        public void MoveRight(GameTime gameTime, float speedMod)
        {
            Vector2 normVec = new Vector2(1, 0);
            if (!Single.IsNaN(normVec.X) && !Single.IsNaN(normVec.Y)) position += normVec * moveSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod;

            Vision tempVis = vision;
            tempVis.parentLocation = this.worldRectangle;
            vision = tempVis;

            for (int i = 0; i < this.visions.Count; i++)
            {
                visions[i].parentLocation = this.worldRectangle;
            }
        }

        public void RotateLeft(GameTime gameTime, float speedMod) //positive to turn left, negative to turn right
        {
            vision.Rotate(rotateSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod);
            foreach (Vision v in this.visions)
            {
                v.Rotate(rotateSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod);
            }
        }

        public void RotateRight(GameTime gameTime, float speedMod) //positive to turn left, negative to turn right
        {
            vision.Rotate(-1 * rotateSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod);
            foreach (Vision v in this.visions)
            {
                v.Rotate(-1 * rotateSpeed * gameTime.ElapsedGameTime.Milliseconds * speedMod);
            }
        }

        public bool CanSeePlayer(Player player, List<Obstacle> visibleObstacles)
        {
            if (!player.isVisible)
                return false;
            foreach(Vision v in this.visions)
            {
                if (v.IntersectsWithPlayer(player, visibleObstacles))
                    return true;
            }
            return false;
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
