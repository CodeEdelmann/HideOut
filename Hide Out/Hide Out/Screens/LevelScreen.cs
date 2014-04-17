using System;
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
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace HideOut.Screens
{
    class LevelScreen : Screen
    {
        enum soundMode { game, victory, failure };
        bool gameSoundOn = true;
        int musicLoopCounter = 0;
        SoundEffect victorySound;
        SoundEffect failureSound;
        SoundEffect gameSound;
        DisplayController displayController;
        SpriteBatch spriteBatch;
        PlayerController playerController;
        ItemController itemController;
        NPCController npcController;
        ObstacleController obstacleController;
        EntityGenerationController entityGenerationController;
        TileController tileController;
        CollisionController collisionController;
        public LevelController levelController { get; set; }
        XMLController xmlController;
        BasicEffect basicEffect;

        Texture2D bgTexture;

        public static readonly bool CHEAT_MODE = false;

        public static readonly int MAX_GAME_WIDTH = 5000;
        public static readonly int MAX_GAME_HEIGHT = 5000;
        public static int GAME_WIDTH = 1000;
        public static int GAME_HEIGHT = 1000;

        public bool isPaused { get; set; }
        public static KeyboardState pauseState;
        public KeyboardState oldState = Keyboard.GetState();
        bool mobile = true;

        public override void Initialize()
        {
            isPaused = false;
            this.InitializeControllers();

            xmlController.write_fname = "Content/Levels/save.xml";
            InitializeLevel();
            // var fontFilePath = Path.Combine(Content.RootDirectory, "CourierNew32.fnt");
            // var fontFile = FontLoader.Load(fontFilePath);
            // var fontTexture = Content.Load<Texture2D>("CourierNew32_0.png");
        }

        public void InitializeLevel()
        {
            if (HideOutGame.LEVEL_DESIGN_MODE)
            {
                levelController.InitializeBlankLevel();
            }
            else
            {
                int current_level = ReadLevel();
                levelController.InitializeLevel(current_level);
            }
        }

        public int ReadLevel()
        {
            if (!File.Exists("Content\\Levels\\savestate.txt"))
            {
                return 1;
            }

            string text = System.IO.File.ReadAllText("Content\\Levels\\savestate.txt");

            int level;
            try
            {
                level = Int32.Parse(text);
            }
            catch
            {
                level = 1;
            }

            return level;
        }

        public override void LoadContent(GraphicsDevice gd, ContentManager cm)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(gd);

            //testing for text display

            // http://www.newgrounds.com/audio/listen/568885
            victorySound = cm.Load<SoundEffect>("victory.wav");

            // http://www.newgrounds.com/audio/listen/135985
            failureSound = cm.Load<SoundEffect>("police.wav");//failure.wav");

            gameSound = cm.Load<SoundEffect>("ambience.wav");

            // TODO: use this.Content to load your game content here
            //.LoadContent(GraphicsDevice, Content);
            displayController.LoadContent(cm);
            npcController.LoadContent(cm);
            obstacleController.LoadContent(cm);
            playerController.LoadContent(cm);
            itemController.LoadContent(cm);

            bgTexture = cm.Load<Texture2D>("bg.png");

            //Initialization for basicEffect
            basicEffect = new BasicEffect(gd);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            //displayController.addDisplay(10, 100, "Hello world 1");


            //_fontRenderer = new FontRenderer(fontFile, <Texture2D>("player.png"));
        }
        public override void Update(GameTime gameTime)
        {
            

            if (HideOutGame.LEVEL_DESIGN_MODE)
            {
                playerController.Update(gameTime, mobile);
                entityGenerationController.Update(gameTime);
                xmlController.Update();
                return;
            }
            pauseState = Keyboard.GetState();
            if (isPaused)
            {
                if (((pauseState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))|| 
                    (pauseState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))|| 
                    (pauseState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))|| 
                    (pauseState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))) && !displayController.hasLost && !displayController.hasWon)
                {
                    isPaused = false;
                    displayController.displayLevel = false;           
                }
                else if (pauseState.IsKeyDown(Keys.Enter) && (displayController.hasWon == true || displayController.hasLost == true))
                {
                    isPaused = false;
                    displayController.displayLevel = false;
                    displayController.hasLost = false;
                    displayController.hasWon = false;
                    HideOutGame.LEVEL_INITIALIZED = false;
                    Type = "TitleScreen";
                    //gameSoundOn = true;
                    //gameSound.Dispose();
                    //gameSound.Play();
                 
                    return;
                }
                else
                {
                    oldState = pauseState;
                    return;
                }
            }
            oldState = pauseState;


            //musicLoopCounter -= gameTime.ElapsedGameTime.Milliseconds;
            //musicLoopCounter = musicLoopCounter - gameTime.ElapsedGameTime.Milliseconds;
            if (gameSoundOn && ((musicLoopCounter == 0) || (gameSound.IsDisposed))) {

                
                    gameSound.Dispose();
                    gameSound.Play();
                    musicLoopCounter = 2000;// +gameSound.Duration.Milliseconds;
                
                
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
               // gameSound.Dispose
               //failureSound.Player();
                gameSoundOn = false;
                gameSound.Dispose();
                failureSound.Play();
                musicLoopCounter = failureSound.Duration.Milliseconds;
                //Console.WriteLine("Duration: " + failureSound.Duration.Milliseconds

                //failureSound.Duration
                displayController.hasLost = true;//Exit();
            }

            displayController.level = levelController.currentLevel;
            switch (levelController.Update())
            {
                case 0:
                    //isPaused = true;
                    displayController.displayLevel = true;
                    /*
                    if (pauseState.IsKeyDown(Keys.Enter))
                    {
                        isPaused = false;
                    }
                    else
                    {
                        isPaused = true;
                    } */
                    break;
                case 1:
                    isPaused = true;
                    displayController.displayLevel = true;
                    displayController.level = levelController.currentLevel;
                    break;
                case 2:
                    isPaused = true;
                    //TODO: show winning screen
                    gameSoundOn = false;
                    Console.WriteLine("You win!  Good day!");
                                    gameSound.Dispose();
                victorySound.Play();
                musicLoopCounter = victorySound.Duration.Milliseconds;

                   
                    try
                    {
                        File.Delete("Content\\Levels\\savestate.txt");
                    }
                    catch { }
                    displayController.hasWon = true;// GetHashCode//Exit();
                    break;
            }

            if (playerController.Update(gameTime, mobile))
            {
                isPaused = true;
                gameSoundOn = false;
                Console.WriteLine("You lose!  Good day!");
                gameSound.Dispose();
                failureSound.Play();
                musicLoopCounter = failureSound.Duration.Milliseconds;

                displayController.hasLost = true;
            }

            itemController.Update(gameTime);
            obstacleController.Update(gameTime);
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

            if(!HideOutGame.LEVEL_DESIGN_MODE)
                npcController.DrawFOVs(gd, basicEffect);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw(bgTexture, new Rectangle(0 - HideOutGame.SCREEN_OFFSET_X, 0 - HideOutGame.SCREEN_OFFSET_Y, GAME_WIDTH, GAME_HEIGHT), new Rectangle(0, 0, GAME_WIDTH, GAME_HEIGHT), Color.Green);
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
