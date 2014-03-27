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
        public int currentSpeed { get; set; }
        public int baseSpeed { get; set; }
        public int currentThirst { get; set; }
        public int maxThirst { get; set; }
        public int currentHunger { get; set; }
        public int maxHunger { get; set; }
        public List<Item> items { get; set; }

        public Player() : base()
        {

        }

        public void MoveRight()
        {
            this.position += new Vector2(this.currentSpeed, 0);
        }

        public void MoveLeft()
        {
            this.position += new Vector2(-1 * this.currentSpeed, 0);
        }

        public void MoveUp()
        {
            this.position += new Vector2(0, -1 * this.currentSpeed);
        }

        public void MoveDown()
        {
            this.position += new Vector2(0, this.currentSpeed);
        }

        public override string ToString()
        {
            string retVal = base.ToString() +
                "Speed: " + this.currentSpeed + " / " + this.baseSpeed + "\n" +
                "Thirst: " + this.currentThirst + " / " + this.maxThirst + "\n" +
                "Hunger: " + this.currentHunger + " / " + this.maxHunger + "\n";
            foreach (Item item in this.items)
            {
                retVal += "Item: " + item + "\n";
            }
            return retVal;
        }
    }
}
