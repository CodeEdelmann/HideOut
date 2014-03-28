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

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left))
            {
                thePlayer.MoveLeft(gameTime);
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                thePlayer.MoveRight(gameTime);
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                thePlayer.MoveUp(gameTime);
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                thePlayer.MoveDown(gameTime);
            }
        }

        public void PickupItem(Item item)
        {
            if (thePlayer.items.Count < 3)
            {
                thePlayer.items.Add(item);
                item.position = new Vector2(-1, -1);
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
