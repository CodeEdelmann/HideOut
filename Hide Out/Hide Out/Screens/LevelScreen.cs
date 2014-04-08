﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using HideOut.Controllers;
using HideOut.Entities;

namespace HideOut.Screens
{
    class LevelScreen : Screen
    {
        DisplayController displayController;
        SpriteBatch spriteBatch;
        PlayerController playerController;
        ItemController itemController;
        NPCController npcController;
        ObstacleController obstacleController;
        EntityGenerationController entityGenerationController;
        TileController tileController;
        CollisionController collisionController;
        LevelController levelController;
        XMLController xmlController;
        BasicEffect basicEffect;

        public static readonly bool CHEAT_MODE = false;

        public static readonly int MAX_GAME_WIDTH = 5000;
        public static readonly int MAX_GAME_HEIGHT = 5000;
        public static int GAME_WIDTH = 1000;
        public static int GAME_HEIGHT = 1000;

        public static bool isPaused = false;
        public static KeyboardState pauseState;
        bool mobile = true;

        public override void Initialize()
        {
            Type = "LevelScreen";
            this.InitializeControllers();

            xmlController.write_fname = "Content/Levels/save.xml";
            xmlController.read_fname = "Content/Levels/1.xml";
            levelController.InitializeLevel(1);

            // var fontFilePath = Path.Combine(Content.RootDirectory, "CourierNew32.fnt");
            // var fontFile = FontLoader.Load(fontFilePath);
            // var fontTexture = Content.Load<Texture2D>("CourierNew32_0.png");
        }
        public override void LoadContent(GraphicsDevice gd, ContentManager cm)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(gd);

            //testing for text display




            // TODO: use this.Content to load your game content here
            displayController.LoadContent(cm);
            npcController.LoadContent(cm);
            obstacleController.LoadContent(cm);
            playerController.LoadContent(cm);
            itemController.LoadContent(cm);

            //Initialization for basicEffect
            basicEffect = new BasicEffect(gd);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;


            //displayController.addDisplay(10, 100, "Hello world 1");


            //_fontRenderer = new FontRenderer(fontFile, <Texture2D>("player.png"));
        }
        public override void Update(GameTime gameTime)
        {
            pauseState = Keyboard.GetState();
            if (isPaused)
            {
                if (pauseState.IsKeyDown(Keys.Enter))
                {
                    isPaused = false;
                    if (displayController.hasWon == true || displayController.hasLost == true)
                    {
                        // TODO: quit game
                    }
                }
                else
                    return;
            }
            //Console.WriteLine("Time: " + gameTime.ElapsedGameTime.TotalMilliseconds);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // TODO: quit game
            }

            // TODO: Add your update logic here
            if (npcController.Update(playerController.thePlayer, obstacleController.obstacles, gameTime) && !CHEAT_MODE)
            {
                isPaused = true;
                //TODO: show losing screen
                Console.WriteLine("You lose!  Good day!");
                displayController.hasLost = true;//Exit();
            }

            switch (levelController.Update())
            {
                case 0:
                    if (pauseState.IsKeyDown(Keys.Enter))
                        isPaused = false;
                    break;
                case 1:
                    isPaused = true;
                    break;
                case 2:
                    isPaused = true;
                    //TODO: show winning screen
                    Console.WriteLine("You win!  Good day!");
                    displayController.hasWon = true;// GetHashCode//Exit();
                    break;
            }

            if (playerController.Update(gameTime, mobile))
            {
                isPaused = true;
                Console.WriteLine("You lose!  Good day!");
                displayController.hasLost = true;
            }

            itemController.Update(gameTime);
            obstacleController.Update(gameTime);
            entityGenerationController.Update(gameTime);
            displayController.Update(gameTime);
            xmlController.Update();
        }
        public override void Draw(GraphicsDevice gd)
        {
            gd.Clear(Color.CornflowerBlue);

            //This block of code is a necessary ritual for the FOVs.  Just leave it be.
            gd.BlendState = BlendState.Opaque;
            gd.DepthStencilState = DepthStencilState.Default;
            gd.SamplerStates[0] = SamplerState.LinearWrap;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, gd.Viewport.Width, gd.Viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            basicEffect.World = Matrix.Identity;
            basicEffect.View = Matrix.Identity;
            basicEffect.Projection = halfPixelOffset * projection;
            basicEffect.VertexColorEnabled = true;
            gd.RasterizerState = RasterizerState.CullNone;

            npcController.DrawFOVs(gd, basicEffect);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            obstacleController.Draw(spriteBatch);
            itemController.Draw(spriteBatch);
            npcController.Draw(spriteBatch);
            playerController.Draw(spriteBatch);

            displayController.Draw(spriteBatch);
            displayController.drawStats(playerController.thePlayer.currentHunger, playerController.thePlayer.currentThirst, spriteBatch);


            //_fontRenderer.DrawText(spriteBatch, 50, 50, "Hello World!");



            spriteBatch.End();
        }

        private void InitializeControllers()
        {
            displayController = new DisplayController();
            npcController = new NPCController();
            playerController = new PlayerController();
            itemController = new ItemController();
            obstacleController = new ObstacleController();
            levelController = new LevelController();
            entityGenerationController = new EntityGenerationController();
            xmlController = new XMLController();
            tileController = new TileController();
            collisionController = new CollisionController();

            //set collision controller
            collisionController.tileController = tileController;

            //set entity generation controller
            entityGenerationController.itemController = itemController;
            entityGenerationController.npcController = npcController;
            entityGenerationController.obstacleController = obstacleController;

            //set xmlController
            xmlController.playerController = playerController;
            xmlController.npcController = npcController;
            xmlController.obstacleController = obstacleController;
            xmlController.itemController = itemController;

            //set tile controller
            tileController.itemController = itemController;
            tileController.npcController = npcController;
            tileController.obstacleController = obstacleController;
            tileController.max_game_height = MAX_GAME_HEIGHT;
            tileController.max_game_width = MAX_GAME_WIDTH;
            tileController.tileSize = PlayerController.SPRITE_SIZE;
            npcController.tileController = tileController;
            itemController.tileController = tileController;
            obstacleController.tileController = tileController;
            tileController.InitializeTiles();

            //set player controller
            playerController.itemController = itemController;
            playerController.collisionController = collisionController;

            //set level controller
            levelController.itemController = itemController;
            levelController.obstacleController = obstacleController;
            levelController.playerController = playerController;
            levelController.npcController = npcController;
            levelController.tileController = tileController;
            levelController.xmlController = xmlController;

            //set npc controller
            npcController.collisionController = collisionController;
        }
    }
}