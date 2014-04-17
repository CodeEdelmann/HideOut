using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;
using HideOut.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using HideOut.Entities;
using Microsoft.Xna.Framework.Audio;

namespace HideOut.Controllers
{
    class PlayerController
    {

        SoundEffect munch;
        SoundEffect slurp;
        SoundEffect clink;

        public Player thePlayer;
        private Texture2D playerTexture;
        public static readonly int SPRITE_SIZE = 50;
        private int fountainSpeed = 500;
        public CollisionController collisionController { set; get; }
        public ObstacleController obstacleController { set; get; }
        public ItemController itemController { get; set; }
        public static readonly int COLLISION_STEP_AMOUNT = 4;
        public static readonly int COLLISION_DELTA = 3;

        public static Dictionary<Entities.Direction, List<Texture2D>> textures;

        public PlayerController()
        {
            textures = new Dictionary<Entities.Direction, List<Texture2D>>();
            textures[Entities.Direction.Down] = new List<Texture2D>();
            textures[Entities.Direction.Up] = new List<Texture2D>();
            textures[Entities.Direction.Right] = new List<Texture2D>();
            textures[Entities.Direction.Left] = new List<Texture2D>();
        }

        public void CreatePlayer(Vector2 position)
        {
            //set thePlayer to a newly created player object
            thePlayer = new Player();
            thePlayer.currentSpeed = 5;
            thePlayer.baseSpeed = 5;
            thePlayer.currentThirst = 10;
            thePlayer.maxThirst = 15;
            thePlayer.currentHunger = 10;
            thePlayer.maxHunger = 15;
            thePlayer.position = position;
            thePlayer.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
            thePlayer.sprite = playerTexture;
            thePlayer.direction = Entities.Direction.Down;
        }

        public bool FindLegalPositionHorizontally()
        {
            thePlayer.position += new Vector2(COLLISION_DELTA, 0);
            if(!collisionController.IllegalMove(this.thePlayer))
                return true;
            thePlayer.position += new Vector2(COLLISION_DELTA * -2, 0);
            if (!collisionController.IllegalMove(this.thePlayer))
                return true;
            thePlayer.position += new Vector2(COLLISION_DELTA, 0);
            return false;
        }

        public bool FindLegalPositionVertically()
        {
            thePlayer.position += new Vector2(0, COLLISION_DELTA);
            if (!collisionController.IllegalMove(this.thePlayer))
                return true;
            thePlayer.position += new Vector2(0, COLLISION_DELTA * -2);
            if (!collisionController.IllegalMove(this.thePlayer))
                return true;
            thePlayer.position += new Vector2(0, COLLISION_DELTA);
            return false;
        }

        public bool Update(GameTime gameTime, bool mobile)
        {
            bool retVal = thePlayer.UpdateState(gameTime);

            KeyboardState keyboard = Keyboard.GetState();

            if (mobile)
            {
                
                if (keyboard.IsKeyDown(Keys.Left))
                {
                    Vector2 previousPosition = thePlayer.position;
                    thePlayer.MoveLeft(gameTime, 1); 
                    if (!HideOutGame.LEVEL_DESIGN_MODE && collisionController.IllegalMove(this.thePlayer))
                    {
                        if (!FindLegalPositionVertically())
                        {
                            for (int i = 1; i <= COLLISION_STEP_AMOUNT; i++)
                            {
                                if (collisionController.IllegalMove(this.thePlayer))
                                    thePlayer.MoveRight(gameTime, 1 / Math.Pow(2, i), false);
                                else
                                    thePlayer.MoveLeft(gameTime, 1 / Math.Pow(2, i), false);
                            }
                        }
                    }
                    if (!HideOutGame.LEVEL_DESIGN_MODE && collisionController.IllegalMove(this.thePlayer))
                    {
                        this.thePlayer.position = previousPosition;
                    }
                }
                if (keyboard.IsKeyDown(Keys.Right))
                {
                    Vector2 previousPosition = thePlayer.position;
                    thePlayer.MoveRight(gameTime, 1);
                    if (!HideOutGame.LEVEL_DESIGN_MODE && collisionController.IllegalMove(this.thePlayer))
                    {
                        if (!FindLegalPositionVertically())
                        {
                            for (int i = 1; i <= COLLISION_STEP_AMOUNT; i++)
                            {
                                if (collisionController.IllegalMove(this.thePlayer))
                                    thePlayer.MoveLeft(gameTime, 1 / Math.Pow(2, i), false);
                                else
                                    thePlayer.MoveRight(gameTime, 1 / Math.Pow(2, i), false);
                            }
                        }
                    }
                    if (!HideOutGame.LEVEL_DESIGN_MODE && collisionController.IllegalMove(this.thePlayer))
                    {
                        this.thePlayer.position = previousPosition;
                    }
                }
                if (keyboard.IsKeyDown(Keys.Up))
                {
                    Vector2 previousPosition = thePlayer.position;
                    thePlayer.MoveUp(gameTime, 1);
                    if (!HideOutGame.LEVEL_DESIGN_MODE && collisionController.IllegalMove(this.thePlayer))
                    {
                        if (!FindLegalPositionHorizontally())
                        {
                            for (int i = 1; i <= COLLISION_STEP_AMOUNT; i++)
                            {
                                if (collisionController.IllegalMove(this.thePlayer))
                                    thePlayer.MoveDown(gameTime, 1 / Math.Pow(2, i), false);
                                else
                                    thePlayer.MoveUp(gameTime, 1 / Math.Pow(2, i), false);
                            }
                        }
                    }
                    if (!HideOutGame.LEVEL_DESIGN_MODE && collisionController.IllegalMove(this.thePlayer))
                    {
                        this.thePlayer.position = previousPosition;
                    }
                }
                if (keyboard.IsKeyDown(Keys.Down))
                {
                    Vector2 previousPosition = thePlayer.position;
                    thePlayer.MoveDown(gameTime, 1);
                    if (!HideOutGame.LEVEL_DESIGN_MODE && collisionController.IllegalMove(this.thePlayer))
                    {
                        if (!FindLegalPositionHorizontally())
                        {
                            for (int i = 1; i <= COLLISION_STEP_AMOUNT; i++)
                            {
                                if (collisionController.IllegalMove(this.thePlayer))
                                    thePlayer.MoveUp(gameTime, 1 / Math.Pow(2, i), false);
                                else
                                    thePlayer.MoveDown(gameTime, 1 / Math.Pow(2, i), false);
                            }
                        }
                    }
                    if (!HideOutGame.LEVEL_DESIGN_MODE && collisionController.IllegalMove(this.thePlayer))
                    {
                        this.thePlayer.position = previousPosition;
                    }
                }

                List<Item> items = collisionController.GetCollidingItems(thePlayer);
                foreach (Item i in items)
                {
                    if(!HideOutGame.LEVEL_DESIGN_MODE)
                        this.PickupItem(i);
                }
                if (HideOutGame.LEVEL_DESIGN_MODE)
                    thePlayer.isVisible = false;

                if (collisionController.IsHidden(thePlayer))
                {
                    thePlayer.isVisible = false;
                }
                else
                {
                    thePlayer.isVisible = true;
                }

                Obstacle nearFountain = collisionController.NearFountain(thePlayer);
                if (nearFountain != null && thePlayer.currentThirst < thePlayer.maxThirst)
                {
                    fountainSpeed -= gameTime.ElapsedGameTime.Milliseconds;
                    if (fountainSpeed <= 0)
                    {
                        thePlayer.currentThirst++;
                        fountainSpeed = 1000;
                        slurp.Play();
                    }
                    obstacleController.TurnFountainOn(nearFountain);
                    
                }
                else
                {
                    obstacleController.TurnFountainsOff();
                }
            }

            this.UpdateScreenOffsets();
            return retVal;
        }

        public void UpdateScreenOffsets()
        {
            HideOutGame.SCREEN_OFFSET_X = (int)
                Math.Min(
                Math.Max(this.thePlayer.position.X - HideOutGame.SCREEN_WIDTH / 2, 0), 
                LevelScreen.GAME_WIDTH - HideOutGame.SCREEN_WIDTH);
            HideOutGame.SCREEN_OFFSET_Y = (int)
                Math.Min(
                Math.Max(this.thePlayer.position.Y - HideOutGame.SCREEN_HEIGHT / 2, 0),
                LevelScreen.GAME_HEIGHT - HideOutGame.SCREEN_HEIGHT);
        }

        public void PickupItem(Item item)
        {
            switch (item.tag)
            {
                case ItemType.WaterBottle:
                    thePlayer.currentThirst += item.waterValue;
                    if (thePlayer.currentThirst > thePlayer.maxThirst)
                    {
                        thePlayer.currentThirst = thePlayer.maxThirst;
                        slurp.Play();
                    }
                    break;
                case ItemType.CandyBar:
                    thePlayer.currentSpeed += item.speedValue;
                    munch.Play();
                    break;
                case ItemType.Apple:
                    thePlayer.currentHunger += item.foodValue;
                    if (thePlayer.currentHunger > thePlayer.maxHunger)
                    {
                        thePlayer.currentHunger = thePlayer.maxHunger;
                        munch.Play();
                    }
                    break;
                case ItemType.Coin:
                   clink.Play();
                    break;
            }
            itemController.RemoveItem(item);
        }

        public void Draw(SpriteBatch sb)
        {
            int numTextures = textures[thePlayer.direction].Count;
            Texture2D spriteToDraw = textures[thePlayer.direction][thePlayer.directionIndex % numTextures];
            sb.Draw(spriteToDraw, thePlayer.screenRectangle, Color.White);

            //Rectangle source = new Rectangle(0, 0, 50, 50);

            //sb.Draw(thePlayer.sprite, thePlayer.screenRectangle, source, Color.White);

            
        }

        public void LoadContent(ContentManager cm)
        {
            //Start by loading all textures
            playerTexture = cm.Load<Texture2D>("player.png");

            foreach(Direction d in textures.Keys)
            {
                for(int i = 1; i <=3; i++)
                {
                    string fname = "Movement/human " + d.ToString() + i.ToString() + ".png";
                    textures[d].Add(cm.Load<Texture2D>(fname.ToLower()));
                }
            }

            //Then assign textures to NPCs depending on their tag
            thePlayer.sprite = playerTexture;

            munch = cm.Load<SoundEffect>("hunger.wav");
            slurp = cm.Load<SoundEffect>("thirst.wav");
            clink = cm.Load<SoundEffect>("coin.wav");


            
        }


    }
}
