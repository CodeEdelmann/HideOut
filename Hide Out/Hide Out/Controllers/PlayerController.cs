using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hide_Out.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Hide_Out.Controllers
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
            thePlayer.CurrentSpeed = 10;
            thePlayer.MaxSpeed = 10;
            thePlayer.CurrentThirst = 10;
            thePlayer.MaxThirst = 10;
            thePlayer.CurrentHunger = 10;
            thePlayer.MaxHunger = 10;
            thePlayer.Items = new List<Item>();
            thePlayer.position = position;
            thePlayer.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
            thePlayer.sprite = playerTexture;
        }

        public void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left))
            {
                thePlayer.moveLeft();
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                thePlayer.moveRight();

            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                thePlayer.moveUp();

            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                thePlayer.moveDown();
            }

            Console.WriteLine(thePlayer);
        }

        public void pickupItem(Item item)
        {
            if (thePlayer.Items.Count < 3)
            {
                thePlayer.Items.Add(item);
                item.position = new Vector2(-1, -1);
                //todo: set item to not be drawn
            }
        }

        public void dropItem(Item item)
        {
            thePlayer.Items.Remove(item);
        }

        public void useItem(int index)
        {
            if (index >= 0 && index < thePlayer.Items.Count)
            {
                //todo: call itemController's add item
            }
        }

        public void drawPlayer(SpriteBatch sb)
        {
            sb.Draw(thePlayer.sprite, thePlayer.drawRectangle, Color.White);
        }

        public void LoadPlayerContent(ContentManager cm)
        {
            //Start by loading all textures
            playerTexture = cm.Load<Texture2D>("player.png");

            //Then assign textures to NPCs depending on their tag
            thePlayer.sprite = playerTexture; 
            
        }


    }
}
