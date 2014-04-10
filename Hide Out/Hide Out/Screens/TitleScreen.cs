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
        Texture2D startSelected;
        Texture2D start;
        Texture2D quitSelected;
        Texture2D quit;
        Texture2D resumeSelected;
        Texture2D resume;


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


             resume = cm.Load<Texture2D>("resume.png");
            resumeSelected = cm.Load<Texture2D>("resume-selected.png");
            quit = cm.Load<Texture2D>("quit.png");
            quitSelected = cm.Load<Texture2D>("quit-selected.png");
            start = cm.Load<Texture2D>("start.png");
            startSelected = cm.Load<Texture2D>("start-selected.png");
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
                    case 0:
                        Type = "LevelScreen";
                        System.IO.File.WriteAllText("Content\\Levels\\savestate.txt", "1");
                        break;
                    case 1:
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



             spriteBatch.Draw(startSelected, new Rectangle(350, 100, 100, 50), Color.White);
            spriteBatch.Draw(resumeSelected, new Rectangle(350, 200, 100, 50), Color.White);
            spriteBatch.Draw(quitSelected, new Rectangle(350, 300, 100, 50), Color.White);

            spriteBatch.Draw(exposition, new Rectangle(20, 20, 199, 172), Color.White);


            switch (index)
            {
                case 0:
                    //fontRenderer.DrawText(spriteBatch, 330, 200, "> New Game");
                    spriteBatch.Draw(startSelected, new Rectangle(350, 100, 100, 50), Color.Green);
                    draw_ng = false;
                    break;
                case 1:
                    //fontRenderer.DrawText(spriteBatch, 330, 300, "> Continue");
                    spriteBatch.Draw(resumeSelected, new Rectangle(350, 200, 100, 50), Color.Green);
                    draw_lg = false;
                    break;
                case 2:
                    // fontRenderer.DrawText(spriteBatch, 330, 400, "> Exit");
                    spriteBatch.Draw(quitSelected, new Rectangle(350, 300, 100, 50), Color.Green);
                    draw_ex = false;
                    break;
            }


        

            
       
            /*
            if (draw_ng) {
                fontRenderer.DrawText(spriteBatch, 350, 200, "New Game");

                spriteBatch.Draw(start, new Rectangle(350, 100, 100, 50), Color.White);
            }
            if (draw_lg)
            {
                fontRenderer.DrawText(spriteBatch, 350, 300, "Continue");

                spriteBatch.Draw(resume, new Rectangle(350, 200, 100, 50), Color.White);
                fontRenderer.DrawText(spriteBatch, 350, 400, "Exit");
                if (draw_ex)
                {
                    spriteBatch.Draw(quit, new Rectangle(350, 300, 100, 50), Color.White);
                }
            }
            */
            spriteBatch.End();
        }

        public bool SaveExists()
        {
            return File.Exists("Content\\Levels\\savestate.txt");
        }
    }
}
