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
       //  public static readonly int SPRITE_SIZE = 50;
        List<Display> displays;
            
        private Texture2D fontTexture;
        Rectangle sizingRectangle;
        public DisplayController()
        {

        }

       
            //thePlayer.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
           
      

        public void Update(GameTime gameTime)
        {
   
        }

  
        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(thePlayer.sprite, thePlayer.screenRectangle, Color.White);

            Rectangle source = new Rectangle(100, 100, 211, 40);

            //sb.Draw(thePlayer.sprite, thePlayer.screenRectangle, source, Color.White);
           // sb.Draw(fontTexture, source, sizingRectangle, Color.White);
            int xPosition = 0;
            int yPosition = 100;
            int xSize = 8;
            int ySize = 12;
            int yOffset = 2;
            int xOffset = 4;
            //sb.Draw(fontTexture, new Rectangle(xPosition, yPosition, 211, 40), sizingRectangle, Color.White);
            
            
            
            //good for testing
            //sb.Draw(fontTexture, new Rectangle(xPosition, yPosition, xSize, ySize), new Rectangle(2 + (xSize * xOffset), 1 + (ySize * yOffset), xSize, ySize), Color.White);

            for (int i = 0; i < displays.Count(); i++)
            {
                for (int ii = 0; ii < displays[i].text.Length; ii++)
                {
                    String workingString = displays[i].text.Substring(ii, 1);
                    //workingString.First().
                    char workingChar = displays[i].text[ii];

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
                            xPosition = displays[i].x + (xSize * ii);
                            yPosition = displays[i].y;
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
                                    xPosition = displays[i].x + (xSize * ii);
                                    yPosition = displays[i].y;
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
            sizingRectangle = new Rectangle(0, 0, 211, 40);
            displays = new List<Display>();



            //testing
            //displays.Add(new Display(0, 0, "12S red"));

            //Then assign textures to NPCs depending on their tag
           // thePlayer.sprite = playerTexture; 
            
        }


    }
}
