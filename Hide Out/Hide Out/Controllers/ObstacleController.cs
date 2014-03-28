using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HideOut.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace HideOut.Controllers
{
    class ObstacleController
    {
        public List<Obstacle> obstacles { get; set; }
        public TileController tileController { get; set; }

        private Texture2D bushTexture;
        private Texture2D treeTexture;
        private Texture2D fountainTexture;
        private Texture2D pondTexture;
        private static readonly int SPRITE_SIZE = 75;

        public ObstacleController()
        {
            obstacles = new List<Obstacle>();
        }

        public void CreateObstacle(ObstacleType type, Vector2 pos)
        {
            Obstacle obstacle = new Obstacle();
            obstacle.position = pos;
            obstacle.tag = type;
            obstacle.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
            switch (type)
            {
                case ObstacleType.Bush:
                    obstacle.sprite = bushTexture;
                    obstacle.canOverlapWith = true;
                    obstacle.canSeeThrough = false;
                    break;
                case ObstacleType.Tree:
                    obstacle.sprite = treeTexture;
                    obstacle.canOverlapWith = false;
                    obstacle.canSeeThrough = false;
                    break;
                case ObstacleType.Fountain:
                    obstacle.sprite = fountainTexture;
                    obstacle.canOverlapWith = false;
                    obstacle.canSeeThrough = true;
                    break;
                case ObstacleType.Pond:
                    obstacle.sprite = pondTexture;
                    obstacle.canOverlapWith = false;
                    obstacle.canSeeThrough = true;
                    break;
            }
            this.AddObstacle(obstacle);
        }

        public void AddObstacle(Obstacle obstacle)
        {
            obstacles.Add(obstacle);
            tileController.Add(obstacle);
        }

        public void RemoveObstacle(Obstacle obstacle)
        {
            obstacles.Remove(obstacle);
            tileController.Remove(obstacle);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Obstacle obstacle in this.obstacles)
            {
                sb.Draw(obstacle.sprite, obstacle.screenRectangle, Color.White);
            }
        }

        public void LoadContent(ContentManager cm)
        {
            bushTexture = cm.Load<Texture2D>("bush.png");
            treeTexture = cm.Load<Texture2D>("trees.png");
            fountainTexture = cm.Load<Texture2D>("waterFountain.png");
            pondTexture = cm.Load<Texture2D>("pond.png");
            
            foreach (Obstacle obstacle in this.obstacles)
            {
                switch (obstacle.tag)
                {
                    case ObstacleType.Bush:
                        obstacle.sprite = bushTexture;
                        break;
                    case ObstacleType.Tree:
                        obstacle.sprite = treeTexture;
                        break;
                    case ObstacleType.Fountain:
                        obstacle.sprite = fountainTexture;
                        break;
                    case ObstacleType.Pond:
                        obstacle.sprite = pondTexture;
                        break;
                }
            }
        }
    }
}
