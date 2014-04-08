using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HideOut.Screens
{
    abstract public class Screen
    {
        public string Type;
        public virtual void Initialize() {}
        public virtual void LoadContent() {}
        public virtual void LoadContent(GraphicsDevice gd, ContentManager cm) {}
        public virtual void Update(GameTime gameTime) {}
        public virtual void Draw(SpriteBatch spriteBatch) {}
        public virtual void Draw(GraphicsDevice gd) {}
    }
}
