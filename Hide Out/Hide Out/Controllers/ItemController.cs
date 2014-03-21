using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Hide_Out.Entities;

namespace Hide_Out.Controllers
{
    class ItemController
    {

        public List<Item> activeItems { get; set; }
        private Texture2D waterBottleTexture;
        private Texture2D appleTexture;
        private Texture2D candyBarTexture;
        private static readonly int SPRITE_SIZE = 25;
        private static readonly int DRAWS_PER_MINUTE = 3600;

        public ItemController()
        {
            activeItems = new List<Item>();
        }

        public void addItem(Item item)
        {
            activeItems.Add(item);
        }

        public void createItem(ItemType type, Vector2 pos)
        {
            Item item = new Item();
            item.position = pos;
            item.rectangle = new Rectangle((int)pos.X, (int)pos.Y, SPRITE_SIZE, SPRITE_SIZE);
            item.isVisible = true;
            item.canPickUp = true;
            item.expirationTime = DRAWS_PER_MINUTE;
            switch (type)
            {
                case ItemType.WaterBottle:
                    item.sprite = waterBottleTexture;
                    break;
                case ItemType.CandyBar:
                    item.sprite = candyBarTexture;
                    break;
                case ItemType.Apple:
                    item.sprite = appleTexture;
                    break;
            }

            activeItems.Add(item);
        }

        public void removeItem(Item item)
        {
            activeItems.Remove(item);
        }

        public void clearItems()
        {
            activeItems.Clear();
        }

        public void updateItems()
        {
            foreach (Item item in this.activeItems){
                item.updateTime();
                if (item.expirationTime <= 0)
                {
                    activeItems.Remove(item);
                }
            }
        }

        public void pickUp(Item item)
        {
            activeItems.Remove(item);
        }

        public void drawItems(SpriteBatch sb)
        {
            foreach (Item item in this.activeItems)
            {
                sb.Draw(item.sprite, item.rectangle, Color.White);
            }
        }

        public void loadItemContent(ContentManager cm)
        {
            waterBottleTexture = cm.Load<Texture2D>("waterBottle.png");
            candyBarTexture = cm.Load<Texture2D>("candybar.png");  
            appleTexture = cm.Load<Texture2D>("apple.png");

            foreach (Item item in this.activeItems)
            {
                switch (item.tag)
                {
                    case ItemType.WaterBottle: 
                        item.sprite = waterBottleTexture;
                        break;

                    case ItemType.Apple:
                        item.sprite = appleTexture;
                        break;
                    
                    case ItemType.CandyBar:
                        item.sprite = candyBarTexture;
                        break;
                }

            }
        }
    }
}
