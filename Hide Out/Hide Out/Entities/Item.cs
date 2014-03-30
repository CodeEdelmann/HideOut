using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HideOut.Entities
{
    enum ItemType { WaterBottle, Apple, CandyBar, Coin }; 
    class Item : Entity
    {
        public ItemType tag { get; set; }
        public bool canPickUp { get; set; }
        public bool isVisible { get; set; }
        public int waterValue { get; set; }
        public int foodValue { get; set; }
        public int speedValue { get; set; }

        public Item() : base()
        {
        }

        public void PickUp()
        {
            canPickUp = false;
            isVisible = false;
        }

        public void UpdateTime()
        {

        }

        public override string ToString()
        {
            return base.ToString() +
                "Type: " + tag + "\n" +
                "Position: X - " + position.X + " Y - " + position.Y + "\n" + 
                "Visibility: " + isVisible + "\n" +
                "PickUp: " + canPickUp + "\n";
        }

    }
}
