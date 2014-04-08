using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace HideOut.Screens
{
    public class TitleScreen : Screen
    {
        BasicEffect basicEffect;
        SpriteBatch spriteBatch;
        KeyboardState oldState;

        Texture2D newGame;
        Texture2D loadGame;
        Texture2D exit;
        Texture2D newGameSelected;
        Texture2D loadGameSelected;
        Texture2D exitSelected;
        int index;
        readonly int MENU_LEN = 3;

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
            newGame = cm.Load<Texture2D>("black.png");
            loadGame = cm.Load<Texture2D>("black.png");
            exit = cm.Load<Texture2D>("black.png");
            newGameSelected = cm.Load<Texture2D>("green.png");
            loadGameSelected = cm.Load<Texture2D>("green.png");
            exitSelected = cm.Load<Texture2D>("green.png");
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
                    case 0:
                        Type = "LevelScreen";
                        LevelScreen.CURRENT_LEVEL = 1;
                        break;
                    case 1:
                        Type = "LevelScreen";
                        break;
                    case 2:
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
            switch (index)
            {
                case 0:
                    spriteBatch.Draw(newGameSelected, new Rectangle(350, 100, 100, 50), Color.White);
                    draw_ng = false;
                    break;
                case 1:
                    spriteBatch.Draw(loadGameSelected, new Rectangle(350, 200, 100, 50), Color.White);
                    draw_lg = false;
                    break;
                case 2:
                    spriteBatch.Draw(exitSelected, new Rectangle(350, 300, 100, 50), Color.White);
                    draw_ex = false;
                    break;
            }
            if (draw_ng)
                spriteBatch.Draw(newGame, new Rectangle(350, 100, 100, 50), Color.White);
            if (draw_lg)
                spriteBatch.Draw(loadGame, new Rectangle(350, 200, 100, 50), Color.White);
            if (draw_ex)
                spriteBatch.Draw(exit, new Rectangle(350, 300, 100, 50), Color.White);

            spriteBatch.End();
        }

        public bool SaveExists()
        {
            return File.Exists("Content\\Levels\\savestate.txt");
        }
    }
}
