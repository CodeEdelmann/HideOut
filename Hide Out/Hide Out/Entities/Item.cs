using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hide_Out.Entities
{
    enum ItemType { WaterBottle, CandyBar }; 
    class Item : Entity
    {
        public ItemType tag { get; set; }
        public bool canPickUp { get; set; }
        public int expirationTime { get; set; }
        public int time { get; set; }
        public bool isVisible { get; set; }

        public Item(ItemType type, Vector2 pos, Texture2D spr, int lifeTime, bool visible, int currentTime)
        {
            position = pos;
            sprite = spr;
            tag = type;
            time = currentTime;
            expirationTime = time + lifeTime;
            isVisible = visible;
            canPickUp = true;
            sprite = spr;
        }

        public void pickUp()
        {
            canPickUp = false;
            isVisible = false;
        }

        public void updateTime(int currentTime)
        {
            time = time + (currentTime - time);
        }

    }
}
