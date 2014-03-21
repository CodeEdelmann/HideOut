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
        private static readonly int DRAWS_PER_MINUTE = 3600;
        public ItemType tag { get; set; }
        public bool canPickUp { get; set; }
        public int expirationTime { get; set; }
        public bool isVisible { get; set; }

        public Item(ItemType type, Vector2 pos, int currentTime, bool visible, int lifeTime)
        {
            position = pos;
            rectangle = new Rectangle((int)pos.X, (int)pos.Y, 25, 25);
            tag = type;
            expirationTime = currentTime + lifeTime;
            isVisible = visible;
            canPickUp = true;
        }

        public Item(ItemType type, Vector2 pos, int currentTime)
        {
            position = pos;
            rectangle = new Rectangle((int)pos.X, (int)pos.Y, 25, 25);
            tag = type;
            expirationTime = currentTime + DRAWS_PER_MINUTE;
            isVisible = true;
            canPickUp = true;
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
            return "" + tag + position + expirationTime + isVisible + canPickUp;
        }

    }
}
