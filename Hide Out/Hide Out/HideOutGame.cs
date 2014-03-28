﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using HideOut.Entities;
using HideOut.Controllers;
#endregion

namespace HideOut
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class HideOutGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerController playerController;
        ItemController itemController;
        NPCController npcController;
        ObstacleController obstacleController;
        EntityGenerationController entityGenerationController;
        TileController tileController;
        CollisionController collisionController;
        XMLController xmlController;
        BasicEffect basicEffect;
        private static readonly int GAME_WIDTH = 1280;
        private static readonly int GAME_HEIGHT = 720;


        public HideOutGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GAME_HEIGHT;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.ApplyChanges();
            
            npcController = new NPCController();
            playerController = new PlayerController();
            itemController = new ItemController();
            obstacleController = new ObstacleController();
            entityGenerationController= new EntityGenerationController(itemController, npcController, obstacleController);
            tileController = new TileController(itemController, npcController, obstacleController, GAME_HEIGHT, GAME_WIDTH, PlayerController.SPRITE_SIZE);
            collisionController = new CollisionController(tileController);

            xmlController = new XMLController("world.xml", "save.xml", playerController, obstacleController, itemController, npcController);
            xmlController.read();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            npcController.LoadContent(Content);
            obstacleController.LoadContent(Content);
            playerController.LoadContent(Content);
            itemController.LoadContent(Content);

            //Initialization for basicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (npcController.Update(playerController.thePlayer.drawRectangle))
            {
                Console.WriteLine("You lose!  Good day!");
                Exit();
            }
            playerController.Update();
            itemController.Update();
            obstacleController.Update();
            entityGenerationController.Update();
            xmlController.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //This block of code is a necessary ritual for the FOVs.  Just leave it be.
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            basicEffect.World = Matrix.Identity;
            basicEffect.View = Matrix.Identity;
            basicEffect.Projection = halfPixelOffset * projection;
            basicEffect.VertexColorEnabled = true;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            npcController.DrawFOVs(GraphicsDevice, basicEffect);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            obstacleController.Draw(spriteBatch);
            itemController.Draw(spriteBatch);
            npcController.Draw(spriteBatch);
            playerController.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
