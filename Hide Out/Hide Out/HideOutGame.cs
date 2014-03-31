#region Using Statements
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

        public static readonly int MAX_GAME_WIDTH = 5000;
        public static readonly int MAX_GAME_HEIGHT = 5000;
        public static int GAME_WIDTH = 1000;
        public static int GAME_HEIGHT = 1000;
        public static readonly int SCREEN_WIDTH = 800;
        public static readonly int SCREEN_HEIGHT = 500;
        public static int SCREEN_OFFSET_X = 0;
        public static int SCREEN_OFFSET_Y = 0;

        public HideOutGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
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
            this.InitializeControllers();

            xmlController = new XMLController("Content/Levels/world.xml", "Content/Levels/save.xml", playerController, obstacleController, itemController, npcController);
            xmlController.read();

           // var fontFilePath = Path.Combine(Content.RootDirectory, "CourierNew32.fnt");
           // var fontFile = FontLoader.Load(fontFilePath);
           // var fontTexture = Content.Load<Texture2D>("CourierNew32_0.png");

            base.Initialize();
        }

        private void InitializeControllers()
        {
            npcController = new NPCController();
            playerController = new PlayerController();
            itemController = new ItemController();
            obstacleController = new ObstacleController();
            entityGenerationController = new EntityGenerationController(itemController, npcController, obstacleController);
            tileController = new TileController(itemController, npcController, obstacleController, MAX_GAME_HEIGHT, MAX_GAME_WIDTH, PlayerController.SPRITE_SIZE);
            collisionController = new CollisionController(tileController);

            npcController.tileController = tileController;
            itemController.tileController = tileController;
            obstacleController.tileController = tileController;
            playerController.itemController = itemController;
            playerController.collisionController = collisionController;
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


           //_fontRenderer = new FontRenderer(fontFile, <Texture2D>("player.png"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /*
        public static FontFile Load(Stream stream)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
            FontFile file = (FontFile)deserializer.Deserialize(stream);
            return file;
        }
        */

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            //Console.WriteLine("Time: " + gameTime.ElapsedGameTime.TotalMilliseconds);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (npcController.Update(playerController.thePlayer.collisionRectangle, obstacleController.obstacles))
            {
                Console.WriteLine("You lose!  Good day!");
                Exit();
            }
            playerController.Update(gameTime);
            itemController.Update(gameTime);
            obstacleController.Update(gameTime);
            entityGenerationController.Update(gameTime);
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



            //_fontRenderer.DrawText(spriteBatch, 50, 50, "Hello World!");



            spriteBatch.End();


            


            base.Draw(gameTime);
        }
    }
}
