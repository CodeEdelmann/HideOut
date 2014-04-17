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
    public class EndScreen : Screen
    {
        BasicEffect basicEffect;
        SpriteBatch spriteBatch;
        public static KeyboardState oldState {get; set;}

        Texture2D logo;
        Texture2D bgTexture;

        FontFile fontFile;
        Texture2D fontTexture;
        FontRenderer fontRenderer;

        public override void Initialize()
        {
            Type = "EndScreen";
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
            logo = cm.Load<Texture2D>("LOGO.png");
            bgTexture = cm.Load<Texture2D>("bg.png");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
            {
                Type = "TitleScreen";
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

            fontRenderer.DrawText(spriteBatch, HideOutGame.SCREEN_WIDTH / 2 - 50, 25, "Credits");

            fontRenderer.DrawText(spriteBatch, 100, 100, "Art Assets:");
            fontRenderer.DrawText(spriteBatch, 100, 150, "Faye Huynh");

            fontRenderer.DrawText(spriteBatch, 100, 250, "Sound Effects:");
            fontRenderer.DrawText(spriteBatch, 100, 300, "http://www.newgrounds.com/audio/listen/568885,");
            fontRenderer.DrawText(spriteBatch, 100, 350, "http://www.newgrounds.com/audio/listen/135985");

            spriteBatch.End();
        }
    }
}
