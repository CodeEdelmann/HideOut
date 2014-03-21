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

        public PlayerController()
        {

        }

        public void CreatePlayer(Vector2 newPosition)
        {
            //set thePlayer to a newly created player object
            thePlayer = new Player();
            thePlayer.position = newPosition;
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
            sb.Draw(thePlayer.sprite, thePlayer.rectangle, Color.White);
        }

        public void LoadPlayerContent(ContentManager cm)
        {
            //Start by loading all textures
            //policeTexture = cm.Load<Texture2D>("");  //When sprite is added, uncomment this line and put name of file in between quotes

            //Then assign textures to NPCs depending on their tag

            thePlayer.sprite = playerTexture; 
            
        }


    }
}
