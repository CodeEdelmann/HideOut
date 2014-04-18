using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using HideOut.BmFont;
using HideOut.Entities;
using HideOut.Controllers;

namespace HideOut.Screens
{
    public class TitleScreen : Screen
    {
        //int musicSelection = 0;
        BasicEffect basicEffect;
        SpriteBatch spriteBatch;
        KeyboardState oldState;

        Player player;
        NPC police1;
        NPC police2;

        Texture2D playerTexture;
        Texture2D npcTexture;

        Texture2D logo;
        Texture2D bgTexture;

        int index;
        readonly int MENU_LEN = 3;

        FontFile fontFile;
        Texture2D fontTexture;
        FontRenderer fontRenderer;

        public override void Initialize()
        {
            Type = "TitleScreen";
            oldState = Keyboard.GetState();
            player = new Player();
            police1 = new NPC();
            police2 = new NPC();

            player.position = new Vector2(200, HideOutGame.SCREEN_HEIGHT - 75);
            police1.position = new Vector2(50, HideOutGame.SCREEN_HEIGHT - 75);
            police2.position = new Vector2(100, HideOutGame.SCREEN_HEIGHT - 75);

            police1.rectangleBounds = new Point(NPCController.SPRITE_SIZE, NPCController.SPRITE_SIZE);
            police2.rectangleBounds = new Point(NPCController.SPRITE_SIZE, NPCController.SPRITE_SIZE);

            player.rectangleBounds = new Point(PlayerController.SPRITE_SIZE, PlayerController.SPRITE_SIZE);

        }

        public int musicType()
        {
            return musicSelection;
        }


        public override void LoadContent(GraphicsDevice gd, ContentManager cm)
        {
            //Initialization for basicEffect
            basicEffect = new BasicEffect(gd);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            spriteBatch = new SpriteBatch(gd);
            string fontFilePath = Path.Combine(cm.RootDirectory, "Fonts/font.fnt");
            fontFile = FontLoader.Load(fontFilePath);
            fontTexture = cm.Load<Texture2D>("Fonts/font_0.png");
            fontRenderer = new FontRenderer(fontFile, fontTexture);
            playerTexture = cm.Load<Texture2D>("player.png");
            npcTexture = cm.Load<Texture2D>("police.png");
            logo = cm.Load<Texture2D>("LOGO.png");
            bgTexture = cm.Load<Texture2D>("bg.png");
            index = 0;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
            player.position += new Vector2(3, 0);
            police1.position += new Vector2(3, 0);
            police2.position += new Vector2(3, 0);
            player.UpdateSpriteData(gameTime, Direction.Right);
            police1.UpdateSpriteData(gameTime, Direction.Right);
            police2.UpdateSpriteData(gameTime, Direction.Right);

            if (player.position.X > HideOutGame.SCREEN_WIDTH)
                player.position = new Vector2(0, player.position.Y);
            if (police1.position.X > HideOutGame.SCREEN_WIDTH)
                police1.position = new Vector2(0, police1.position.Y);
            if (police2.position.X > HideOutGame.SCREEN_WIDTH)
                police2.position = new Vector2(0, police2.position.Y);

            if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
            {
                index = (index - 1 + MENU_LEN) % MENU_LEN;
            }
            if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
            {
                index = (index + 1) % MENU_LEN;
            }
            if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
            {
                switch (index)
                {
                    case 1:
                        Type = "LevelScreen";
                        System.IO.File.WriteAllText("Content\\Levels\\savestate.txt", "1");
                        index = 0;
                        musicSelection = 0;
                        break;
                    case 0:
                        Type = "LevelScreen";
                        musicSelection = 0;
                        break;
                    case 2:
                        Type = "Exit";
                        break;
                }
            }
            oldState = newState;
        }
        public override void Draw(GraphicsDevice gd)
        {
            gd.Clear(Color.White);

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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, HideOutGame.SCREEN_WIDTH, HideOutGame.SCREEN_HEIGHT), new Rectangle(0, 0, HideOutGame.SCREEN_WIDTH, HideOutGame.SCREEN_WIDTH), Color.Green);

            bool draw_ng = true, draw_lg = true, draw_ex = true;

            //spriteBatch.Draw(exposition, new Rectangle(20, 20, 199, 172), Color.White);

            spriteBatch.Draw(logo, new Rectangle(240, 30, (int)(643 * .8), (int)(414 * .8)), Color.White);

            //draw player and police
            int len = NPCController.textures[Direction.Right].Count;
            spriteBatch.Draw(NPCController.textures[Direction.Right][police1.directionIndex % len], police1.worldRectangle, Color.White);
            spriteBatch.Draw(NPCController.textures[Direction.Right][police2.directionIndex % len], police2.worldRectangle, Color.White);

            int numTextures = PlayerController.textures[player.direction].Count;
            Texture2D spriteToDraw = PlayerController.textures[player.direction][player.directionIndex % numTextures];

            spriteBatch.Draw(spriteToDraw, player.worldRectangle, Color.White);


            switch (index)
            {
                case 0:
                    fontRenderer.DrawText(spriteBatch, HideOutGame.SCREEN_WIDTH / 2 - 70, HideOutGame.SCREEN_HEIGHT / 2 + 80, "> Continue");
                    draw_ng = false;
                    break;
                case 1:
                    fontRenderer.DrawText(spriteBatch, HideOutGame.SCREEN_WIDTH / 2 - 70, HideOutGame.SCREEN_HEIGHT / 2 + 130, "> New Game");
                    draw_lg = false;
                    break;
                case 2:
                    fontRenderer.DrawText(spriteBatch, HideOutGame.SCREEN_WIDTH / 2 - 70, HideOutGame.SCREEN_HEIGHT / 2 + 180, "> Exit");
                    draw_ex = false;
                    break;
            }

            if (draw_ng)
            {
                fontRenderer.DrawText(spriteBatch, HideOutGame.SCREEN_WIDTH / 2 - 50, HideOutGame.SCREEN_HEIGHT / 2 + 80, "Continue");
            }
            if (draw_lg)
            {
                fontRenderer.DrawText(spriteBatch, HideOutGame.SCREEN_WIDTH / 2 - 50, HideOutGame.SCREEN_HEIGHT / 2 + 130, "New Game");
            }
            if (draw_ex)
            {
                fontRenderer.DrawText(spriteBatch, HideOutGame.SCREEN_WIDTH / 2 - 50, HideOutGame.SCREEN_HEIGHT / 2 + 180, "Exit");
            }

            spriteBatch.End();
        }

        public bool SaveExists()
        {
            return File.Exists("Content\\Levels\\savestate.txt");
        }
    }
}