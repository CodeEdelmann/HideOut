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

namespace HideOut.Screens
{
    public class TitleScreen : Screen
    {
        BasicEffect basicEffect;
        SpriteBatch spriteBatch;
        KeyboardState oldState;

        Texture2D exposition;


        int index;
        readonly int MENU_LEN = 3;

        FontFile fontFile;
        Texture2D fontTexture;
        FontRenderer fontRenderer;

        public override void Initialize()
        {
            Type = "TitleScreen";
            oldState = Keyboard.GetState();
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
            exposition = cm.Load<Texture2D>("exposition1.png");

            index = 0;
        }
        
        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
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
                        break;
                    case 0:
                        Type = "LevelScreen";
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

            spriteBatch.Begin();

            bool draw_ng = true, draw_lg = true, draw_ex = true;

            spriteBatch.Draw(exposition, new Rectangle(20, 20, 199, 172), Color.White);


            switch (index)
            {
                case 0:
                    fontRenderer.DrawText(spriteBatch, 330, 200, "> Continue");
                    draw_ng = false;
                    break;
                case 1:
                    fontRenderer.DrawText(spriteBatch, 330, 275, "> New Game");
                    draw_lg = false;
                    break;
                case 2:
                    fontRenderer.DrawText(spriteBatch, 330, 350, "> Exit");
                    draw_ex = false;
                    break;
            }
            
            if (draw_ng)
            {
                fontRenderer.DrawText(spriteBatch, 350, 200, "Continue");
            }
            if (draw_lg)
            {
                fontRenderer.DrawText(spriteBatch, 350, 275, "New Game");
            }
            if (draw_ex)
            {
                fontRenderer.DrawText(spriteBatch, 350, 350, "Exit");
            }
            
            spriteBatch.End();
        }

        public bool SaveExists()
        {
            return File.Exists("Content\\Levels\\savestate.txt");
        }
    }
}
