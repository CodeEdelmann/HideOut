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
        public static readonly int SPRITE_SIZE = 50;
        public CollisionController collisionController { set; get; }
        public ItemController itemController { get; set; }

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
            thePlayer.position = position;
            thePlayer.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
            thePlayer.sprite = playerTexture;
        }

        public void Update(GameTime gameTime, bool mobile)
        {
            thePlayer.UpdateState(gameTime);

            KeyboardState keyboard = Keyboard.GetState();

            if (mobile)
            {
                if (keyboard.IsKeyDown(Keys.Left))
                {
                    thePlayer.MoveLeft(gameTime);
                    if (collisionController.IllegalMove(this.thePlayer))
                    {
                        thePlayer.MoveRight(gameTime);
                    }
                }
                if (keyboard.IsKeyDown(Keys.Right))
                {
                    thePlayer.MoveRight(gameTime);
                    if (collisionController.IllegalMove(this.thePlayer))
                    {
                        thePlayer.MoveLeft(gameTime);
                    }
                }
                if (keyboard.IsKeyDown(Keys.Up))
                {
                    thePlayer.MoveUp(gameTime);
                    if (collisionController.IllegalMove(this.thePlayer))
                    {
                        thePlayer.MoveDown(gameTime);
                    }
                }
                if (keyboard.IsKeyDown(Keys.Down))
                {
                    thePlayer.MoveDown(gameTime);
                    if (collisionController.IllegalMove(this.thePlayer))
                    {
                        thePlayer.MoveUp(gameTime);
                    }
                }

                List<Item> items = collisionController.GetCollidingItems(thePlayer);
                foreach (Item i in items)
                {
                    this.PickupItem(i);
                }

                if (collisionController.IsHidden(thePlayer))
                {
                    thePlayer.isVisible = false;
                }
                else
                {
                    thePlayer.isVisible = true;
                }

                if (collisionController.NearFountain(thePlayer))
                {
                    //Todo (SYNDEY): Update Player's thirst
                }
            }

            this.UpdateScreenOffsets();
        }

        public void UpdateScreenOffsets()
        {
            HideOutGame.SCREEN_OFFSET_X = (int)
                Math.Min(
                Math.Max(this.thePlayer.position.X - HideOutGame.SCREEN_WIDTH / 2, 0), 
                HideOutGame.GAME_WIDTH - HideOutGame.SCREEN_WIDTH);
            HideOutGame.SCREEN_OFFSET_Y = (int)
                Math.Min(
                Math.Max(this.thePlayer.position.Y - HideOutGame.SCREEN_HEIGHT / 2, 0),
                HideOutGame.GAME_HEIGHT - HideOutGame.SCREEN_HEIGHT);
        }

        public void PickupItem(Item item)
        {
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
            itemController.RemoveItem(item);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(thePlayer.sprite, thePlayer.screenRectangle, Color.White);

            //Rectangle source = new Rectangle(0, 0, 50, 50);

            //sb.Draw(thePlayer.sprite, thePlayer.screenRectangle, source, Color.White);

            
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
