using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HideOut.Controllers
{
    class PlayerController
    {
        public Player thePlayer;
        private Texture2D playerTexture;
        private static readonly int SPRITE_SIZE = 50;

        public PlayerController()
        {

        }

        public void CreatePlayer(Vector2 position)
        {
            //set thePlayer to a newly created player object
            thePlayer = new Player();
            thePlayer.currentSpeed = 5;
            thePlayer.baseSpeed = 5;
            thePlayer.currentThirst = 10;
            thePlayer.maxThirst = 10;
            thePlayer.currentHunger = 10;
            thePlayer.maxHunger = 10;
            thePlayer.items = new List<Item>();
            thePlayer.position = position;
            thePlayer.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
            thePlayer.sprite = playerTexture;
        }

        public void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left))
            {
                thePlayer.MoveLeft();
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                thePlayer.MoveRight();
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                thePlayer.MoveUp();
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                thePlayer.MoveDown();
            }
        }

        public void PickupItem(Item item)
        {
            if (thePlayer.items.Count < 3)
            {
                thePlayer.items.Add(item);
                item.position = new Vector2(-1, -1);
                item.isVisible = false;
                item.canPickUp = false;
                //todo: set item to not be drawn
            }
        }

        public void DropItem(Item item)
        {
            thePlayer.items.Remove(item);
        }

        public void UseItem(int index)
        {
            if (index >= 0 && index < thePlayer.items.Count)
            {
                //todo: call itemController's add item
                Item item = thePlayer.items[index];
                switch (item.tag)
                {
                    case ItemType.WaterBottle:
                        thePlayer.currentThirst += item.waterValue;
                        if (thePlayer.currentThirst > thePlayer.maxThirst)
                            thePlayer.currentThirst = thePlayer.maxThirst;
                        break;
                    //implementing CandyBar - which affects speed - will be more difficult
                    case ItemType.Apple:
                        thePlayer.currentHunger += item.foodValue;
                        if (thePlayer.currentHunger > thePlayer.maxHunger)
                            thePlayer.currentHunger = thePlayer.maxHunger;
                        break; 

                }

                thePlayer.items.Remove(item);
                
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(thePlayer.sprite, thePlayer.drawRectangle, Color.White);
        }

        public void LoadContent(ContentManager cm)
        {
            //Start by loading all textures
            playerTexture = cm.Load<Texture2D>("player.png");

            //Then assign textures to NPCs depending on their tag
            thePlayer.sprite = playerTexture; 
            
        }


    }
}
