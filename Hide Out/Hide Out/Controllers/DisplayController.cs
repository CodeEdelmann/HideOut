using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;
using HideOut.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HideOut.Controllers
{
    class DisplayController
    {

        int xPosition = 0;
        int yPosition = 100;
        int xSize = 8;
        int ySize = 12;
        int yOffset = 2;
        int xOffset = 4;


       //  public static readonly int SPRITE_SIZE = 50;
        List<Display> displays;
            
        private Texture2D fontTexture;

        private Texture2D loseGameTexture;
        private Texture2D hungerTexture;
        private Texture2D thirstTexture;
        private Texture2D winGameTexture;
        Rectangle sizingRectangle;
        public bool hasLost = false;
        public bool hasWon = false;

        public void lose()
        {
            hasLost = true;
        }
        public void win()
        {
            hasWon = true;

        }
        public DisplayController()
        {

        }

       
            //thePlayer.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
           
      

        public void Update(GameTime gameTime)
        {
   
        }


        public void drawStats(int hunger, int thirst, SpriteBatch sb)
        {
            for (int i = 0; i < hunger; i++)
            {
                sb.Draw(hungerTexture, new Rectangle(20 * i, 0, 20, 20), Color.White);
            }

            for (int i = 0; i < thirst; i++)
            {
                sb.Draw(thirstTexture, new Rectangle(20 * i, 50, 20, 20), Color.White);
            }


        }

        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(thePlayer.sprite, thePlayer.screenRectangle, Color.White);

            Rectangle source = new Rectangle(100, 100, 211, 40);

            //sb.Draw(thePlayer.sprite, thePlayer.screenRectangle, source, Color.White);
           // sb.Draw(fontTexture, source, sizingRectangle, Color.White);
         
            //sb.Draw(fontTexture, new Rectangle(xPosition, yPosition, 211, 40), sizingRectangle, Color.White);

 



            xPosition = 0;
             yPosition = 100;
            xSize = 8;
             ySize = 12;
             yOffset = 2;
             xOffset = 4;


             if (hasLost)
             {
                 sb.Draw(loseGameTexture, new Rectangle(0, 0, 100, 100), Color.White);
                 displayString("Game Over", 0, 0, sb);
             }
             else
             {
                 if (hasWon)
                 {
                     sb.Draw(winGameTexture, new Rectangle(0, 0, 100, 100), Color.White);
                     displayString("Congratulations!", 0, 0, sb);
                 }
             }



            //good for testing
            //sb.Draw(fontTexture, new Rectangle(xPosition, yPosition, xSize, ySize), new Rectangle(2 + (xSize * xOffset), 1 + (ySize * yOffset), xSize, ySize), Color.White);

            for (int i = 0; i < displays.Count(); i++)
            {
                String currentString = displays[i].text;
                displayString(currentString, displays[i].x, displays[i].y, sb);
                
            }

        }
        public void displayString(String currentString, int xPos, int yPos, SpriteBatch sb)
        {

            for (int ii = 0; ii < currentString.Length; ii++)
            {
                String workingString = currentString.Substring(ii, 1);
                //workingString.First().
                char workingChar = currentString[ii];

                //double result = char.GetNumericValue(displays[i].text, ii);

                //Console.WriteLine("Result: " + result);
                if (workingChar == ' ')
                {
                    //do nothing, this is a gap


                }
                else
                {
                    if (Char.IsLetter(workingChar))
                    {
                        int result = char.ToUpper(workingChar) - 64;
                        xPosition = xPos + (xSize * ii);
                        yPosition = yPos;
                        yOffset = 1;//hard coded as caps right now
                        xOffset = result - 1;//for letters

                        sb.Draw(fontTexture, new Rectangle(xPosition, yPosition, xSize, ySize), new Rectangle(2 + (xSize * xOffset), 1 + (ySize * yOffset), xSize, ySize), Color.White);


                    }
                    else
                    {
                        if (Char.IsNumber(workingChar))
                        {
                            int result = Convert.ToInt32(workingString);
                            if (result > -1)//TODO: handle negatives?
                            {
                                xPosition = xPos + (xSize * ii);
                                yPosition = yPos;
                                yOffset = 2;//numbers
                                xOffset = result;//for numbers

                                sb.Draw(fontTexture, new Rectangle(xPosition, yPosition, xSize, ySize), new Rectangle(2 + (xSize * xOffset), 1 + (ySize * yOffset), xSize, ySize), Color.White);
                            }
                        }
                        else
                        {
                            //handle symbols, punctuations, other special cases here

                        }
                    }
                }

            }
        }
        public void addDisplay(int xx, int yy, String text)
        {
            displays.Add(new Display(xx, yy, text));
        }

        public void LoadContent(ContentManager cm)
        {
            //Start by loading all textures
            //playerTexture = cm.Load<Texture2D>("player.png");
            fontTexture = cm.Load<Texture2D>("basicFont.png");

            winGameTexture = cm.Load<Texture2D>("victory.png");

           loseGameTexture = cm.Load<Texture2D>("gameOver.png");


           thirstTexture = cm.Load<Texture2D>("waterBottle.png");

           hungerTexture = cm.Load<Texture2D>("apple.png");


            sizingRectangle = new Rectangle(0, 0, 211, 40);
            displays = new List<Display>();
            //addDisplay(0, 0,


            //testing
            //displays.Add(new Display(0, 0, "12S red"));

            //Then assign textures to NPCs depending on their tag
           // thePlayer.sprite = playerTexture; 
            
        }


    }
}
