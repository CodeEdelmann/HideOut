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
using HideOut.Screens;
using Microsoft.Xna.Framework.Audio;
#endregion



namespace HideOut
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class HideOutGame : Game
    {
        GraphicsDeviceManager graphics;
        public static readonly int SCREEN_WIDTH = 1000;
        public static readonly int SCREEN_HEIGHT = 600;
        public static int SCREEN_OFFSET_X = 0;
        public static int SCREEN_OFFSET_Y = 0;

        public Screen currentScreen { get; set; }
        TitleScreen titleScreen;
        LevelScreen levelScreen;
        EndScreen endScreen;
        public static bool LEVEL_INITIALIZED = false;
        public static readonly bool LEVEL_DESIGN_MODE = false;
        public static readonly string LEVEL_TO_EDIT = ""; //set this to the file name in Content/Levels (i.e. "3.xml") 
                                                            //Saving will write to that file in the debug folder (you still have to copy it over)
                                                            //Leaving this blank will open a blank screen based no the next variable
        public static readonly int LEVEL_DESIGN_SIZE = 1; //1 is 1000x1000, 2 is 1500x1500, 3 is 2000x2000

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

            titleScreen = new TitleScreen();
            levelScreen = new LevelScreen();
            endScreen = new EndScreen();
            if (LEVEL_DESIGN_MODE)
                currentScreen = levelScreen;
            else
                currentScreen = titleScreen; // Choose starting screen;

            titleScreen.Initialize();
            levelScreen.Initialize();
            endScreen.Initialize();

            base.Initialize();



        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            titleScreen.LoadContent(GraphicsDevice, Content);
            levelScreen.LoadContent(GraphicsDevice, Content);
            endScreen.LoadContent(GraphicsDevice, Content);

            //  http://www.newgrounds.com/audio/listen/564520
            //backgroundMusic = Content.Load<SoundEffect>("ambience.wav");
            //backgroundMusic.Play();
            base.LoadContent();
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
            switch (currentScreen.Type)
            {
                case "TitleScreen":
                    currentScreen = titleScreen;
                    currentScreen.Type = "TitleScreen";
                    break;
                case "LevelScreen":
                    if (!LEVEL_INITIALIZED)
                    {
                        levelScreen.InitializeLevel();
                        LEVEL_INITIALIZED = true;
                    }
                    currentScreen = levelScreen;
                    currentScreen.Type = "LevelScreen";
                    break;
                case "EndScreen":
                    currentScreen = endScreen;
                    currentScreen.Type = "EndScreen";
                    break;
                case "Exit":
                    Exit();
                    break;
            }

            currentScreen.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            currentScreen.Draw(GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}
