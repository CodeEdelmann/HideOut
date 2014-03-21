using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hide_Out.Entities
{
    class Player : Entity
    {
        public int CurrentSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public int CurrentThirst { get; set; }
        public int MaxThirst { get; set; }
        public int CurrentHunger { get; set; }
        public int MaxHunger { get; set; }
        public List<Item> Items { get; set; }

        public Player() 
        {

        }

        public void moveRight()
        {
            this.position += new Vector2(this.CurrentSpeed, 0);
        }

        public void moveLeft()
        {
            this.position += new Vector2(-1 * this.CurrentSpeed, 0);
        }

        public void moveUp()
        {
            this.position += new Vector2(0, this.CurrentSpeed);
        }

        public void moveDown()
        {
            this.position += new Vector2(0, -1 * this.CurrentSpeed);
        }

        public override string ToString()
        {
            string retVal = base.ToString() +
                "Speed: " + this.CurrentSpeed + " / " + this.MaxSpeed + "\n" +
                "Thirst: " + this.CurrentThirst + " / " + this.MaxThirst + "\n" +
                "Hunger: " + this.CurrentHunger + " / " + this.MaxHunger + "\n";
            foreach (Item item in this.Items)
            {
                retVal += "Item: " + item + "\n";
            }
            return retVal;
        }
    }
}
