using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hide_Out.Entities
{
    enum ItemType { WaterBottle, Apple, CandyBar }; 
    class Item : Entity
    {
        public ItemType tag { get; set; }
        public bool canPickUp { get; set; }
        public int expirationTime { get; set; }
        public bool isVisible { get; set; }

        public Item()
        {
        }

        public void pickUp()
        {
            canPickUp = false;
            isVisible = false;
        }

        public void updateTime()
        {
            expirationTime--;
        }

        public override string ToString()
        {
            return base.ToString() +
                "Type: " + tag + "\n" +
                "Position: X - " + position.X + " Y - " + position.Y + "\n" + 
                "Expiration: " + expirationTime + "\n" + 
                "Visibility: " + isVisible + "\n" +
                "PickUp: " + canPickUp + "\n";
        }

    }
}
