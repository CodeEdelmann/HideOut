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
        private int decrement = 0;
        private int speedDec = 0;
        public int currentSpeed { get; set; }
        public int baseSpeed { get; set; }
        public int currentThirst { get; set; }
        public int maxThirst { get; set; }
        public int currentHunger { get; set; }
        public int maxHunger { get; set; }
        public bool isVisible { get; set; }

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

        }

        public void MoveRight(GameTime gameTime)
        {
            
            this.position += new Vector2(this.currentSpeed / modd * gameTime.ElapsedGameTime.Milliseconds, 0);
        }

        public void MoveLeft(GameTime gameTime)
        {
            this.position += new Vector2(-1 * this.currentSpeed / modd * gameTime.ElapsedGameTime.Milliseconds, 0);
        }

        public void MoveUp(GameTime gameTime)
        {
            this.position += new Vector2(0, -1 * this.currentSpeed / modd * gameTime.ElapsedGameTime.Milliseconds);
        }

        public void MoveDown(GameTime gameTime)
        {
            this.position += new Vector2(0, this.currentSpeed / modd * gameTime.ElapsedGameTime.Milliseconds);
        }

        public bool UpdateState(GameTime gameTime)
        {
            //Current decrements hard coded in; can shift with testing and adapt for readonly constants
            this.decrement += gameTime.ElapsedGameTime.Milliseconds;
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

            if (this.decrement >= 5000)
            {
                this.currentThirst--;
                this.currentHunger--;
                //Console.Write("Current GameTime: " + gameTime.ElapsedGameTime.Seconds + " Thirst: " + this.currentThirst + " Hunger: " + this.currentHunger);
                if (this.currentHunger == 0 || this.currentThirst == 0)
                {
                  //  Console.Write("out of thirst/stamina!");
                    return true;
                }
                this.decrement = 0;
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
