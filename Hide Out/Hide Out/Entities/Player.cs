using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HideOut.Entities
{
    class Player : Entity
    {
        public static readonly float PLAYER_HITBOX_SCALE = 0.6F;
        float modd = 16.6666F;
        private int foodDec = 0;
        private int waterDec = 0;
        private int speedDec = 0;
        public int currentSpeed { get; set; }
        public int baseSpeed { get; set; }
        public int currentThirst { get; set; }
        public int maxThirst { get; set; }
        public int currentHunger { get; set; }
        public int maxHunger { get; set; }
        public bool isVisible { get; set; }
        public Entities.Direction direction { get; set; }
        public int directionIndex { get; set; } //this determines which sprite to draw
        public int directionIndexDec { get; set; }
        public static readonly int maxIndexDec = 150;
        public Dictionary<Entities.Direction, int> directionIndices;


        public Rectangle collisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X + rectangleBounds.X * (1 - PLAYER_HITBOX_SCALE) / 2),
                                     (int)(position.Y + rectangleBounds.Y * (1 - PLAYER_HITBOX_SCALE) / 2),
                                     (int)(rectangleBounds.X * PLAYER_HITBOX_SCALE),
                                     (int)(rectangleBounds.Y * PLAYER_HITBOX_SCALE));
            }
        }

        public Player() : base()
        {
            directionIndices = new Dictionary<Direction, int>();
            directionIndices.Add(Direction.Down, 0);
            directionIndices.Add(Direction.Up, 0);
            directionIndices.Add(Direction.Left, 0);
            directionIndices.Add(Direction.Right, 0);
        }

        public void UpdateSpriteData(GameTime gameTime, Direction movementDirection)
        {
            directionIndexDec -= gameTime.ElapsedGameTime.Milliseconds;
            if (directionIndexDec <= 0)
            {
                directionIndexDec = maxIndexDec;
                directionIndices[movementDirection]++;
            }
            this.direction = movementDirection;
            directionIndex = 0;
            //directionIndexDec = maxIndexDec;
        }

        public void MoveRight(GameTime gameTime, double multiplier)
        {
            this.MoveRight(gameTime, multiplier, true);
        }

        public void MoveRight(GameTime gameTime, double multiplier, bool updateSpriteData)
        {
            if(updateSpriteData)
                UpdateSpriteData(gameTime, Direction.Right);
            this.position += new Vector2((float)multiplier * this.currentSpeed / modd * gameTime.ElapsedGameTime.Milliseconds, 0);
        }

        public void MoveLeft(GameTime gameTime, double multiplier)
        {
            this.MoveLeft(gameTime, multiplier, true);
        }

        public void MoveLeft(GameTime gameTime, double multiplier, bool updateSpriteData)
        {
            if(updateSpriteData)
                UpdateSpriteData(gameTime, Direction.Left);
            this.position += new Vector2(-1 * (float)multiplier * this.currentSpeed / modd * gameTime.ElapsedGameTime.Milliseconds, 0);
        }

        public void MoveUp(GameTime gameTime, double multiplier)
        {
            this.MoveUp(gameTime, multiplier, true);
        }

        public void MoveUp(GameTime gameTime, double multiplier, bool updateSpriteData)
        {
            if(updateSpriteData)
                UpdateSpriteData(gameTime, Direction.Up);
            this.position += new Vector2(0, -1 * (float)multiplier * this.currentSpeed / modd * gameTime.ElapsedGameTime.Milliseconds);
        }

        public void MoveDown(GameTime gameTime, double multiplier)
        {
            this.MoveDown(gameTime, multiplier, true);
        }

        public void MoveDown(GameTime gameTime, double multiplier, bool updateSpriteData)
        {
            if(updateSpriteData)
                UpdateSpriteData(gameTime, Direction.Down);
            this.position += new Vector2(0, (float)multiplier * this.currentSpeed / modd * gameTime.ElapsedGameTime.Milliseconds);
        }

        public bool UpdateState(GameTime gameTime)
        {
            //Current decrements hard coded in; can shift with testing and adapt for readonly constants
            this.foodDec += gameTime.ElapsedGameTime.Milliseconds;
            this.waterDec += gameTime.ElapsedGameTime.Milliseconds;

            if (HideOutGame.LEVEL_DESIGN_MODE)
                return true;
            if (this.currentSpeed > this.baseSpeed)
            {
                this.speedDec += gameTime.ElapsedGameTime.Milliseconds;
                if (speedDec >= 1000)
                {
                    currentSpeed--;
                    speedDec = 0;
                }
            }

            if (this.foodDec >= 5000)
            {
                this.currentHunger--;
                //Console.Write("Current GameTime: " + gameTime.ElapsedGameTime.Seconds + " Thirst: " + this.currentThirst + " Hunger: " + this.currentHunger);
                if (this.currentHunger == 0)
                {
                  //  Console.Write("out of thirst/stamina!");
                    return true;
                }
                this.foodDec = 0;
            }

            if (this.waterDec >= 4000)
            {
                this.currentThirst--;
                if (this.currentThirst == 0)
                {
                    return true;
                }
                this.waterDec = 0;
            }
            return false;

        }

        public override string ToString()
        {
            string retVal = base.ToString() +
                "Speed: " + this.currentSpeed + " / " + this.baseSpeed + "\n" +
                "Thirst: " + this.currentThirst + " / " + this.maxThirst + "\n" +
                "Hunger: " + this.currentHunger + " / " + this.maxHunger + "\n";

            return retVal;
        }
    }
}
