using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using HideOut.Entities;

namespace HideOut.Controllers
{
    class ItemController
    {
        public List<Item> activeItems { get; set; }
        public TileController tileController { get; set; }
        private Texture2D waterBottleTexture;
        private Texture2D appleTexture;
        private Texture2D candyBarTexture;
        private Texture2D coinTexture;
        private static readonly int SPRITE_SIZE = 25;

        public ItemController()
        {
            activeItems = new List<Item>();
        }

        public void AddItem(Item item)
        {
            activeItems.Add(item);
            tileController.Add(item);
        }

        public void CreateItem(ItemType type, Vector2 pos)
        {
            Item item = new Item();
            item.tag = type;
            item.position = pos;
            item.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
            item.isVisible = true;
            item.canPickUp = true;
            switch (type)
            {
                case ItemType.WaterBottle:
                    item.sprite = waterBottleTexture;
                    item.waterValue = 5;
                    break;
                case ItemType.CandyBar:
                    item.sprite = candyBarTexture;
                    item.speedValue = 10;
                    break;
                case ItemType.Apple:
                    item.sprite = appleTexture;
                    item.foodValue = 5;
                    break;
                case ItemType.Coin:
                    item.sprite = coinTexture;
                    break;
            }

            this.AddItem(item);
        }

        public void RemoveItem(Item item)
        {
            activeItems.Remove(item);
            tileController.Remove(item);
        }

        public void ClearItems()
        {
            activeItems.Clear();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void PickUp(Item item)
        {
            activeItems.Remove(item);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Item item in this.activeItems)
            {
                sb.Draw(item.sprite, item.screenRectangle, Color.White);
            }
        }

        public void LoadContent(ContentManager cm)
        {
            waterBottleTexture = cm.Load<Texture2D>("waterBottle.png");
            candyBarTexture = cm.Load<Texture2D>("candybar.png");  
            appleTexture = cm.Load<Texture2D>("apple.png");
            coinTexture = cm.Load<Texture2D>("coin.png");

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

                    case ItemType.Coin:
                        item.sprite = coinTexture;
                        break;
                }

            }
        }
    }
}
