using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hide_Out.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Hide_Out.Controllers
{
    class ObstacleController
    {
        public List<Obstacle> obstacles { get; set; }
        private Texture2D bushTexture;
        private Texture2D treeTexture;
        private Texture2D fountainTexture;
        private Texture2D pondTexture;

        public ObstacleController()
        {
            obstacles = new List<Obstacle>();
        }

        public void createObstacle(ObstacleType type, Vector2 pos)
        {
            Obstacle obstacle = new Obstacle();
            obstacle.position = pos;
            obstacle.tag = type;
            obstacle.rectangleBounds = new Point(100, 100);
            switch (type)
            {
                case ObstacleType.Bush:
                    obstacle.sprite = bushTexture;
                    obstacle.CanOverlapWith = true;
                    obstacle.CanSeeThrough = false;
                    break;
                case ObstacleType.Tree:
                    obstacle.sprite = treeTexture;
                    obstacle.CanOverlapWith = false;
                    obstacle.CanSeeThrough = false;
                    break;
                case ObstacleType.Fountain:
                    obstacle.sprite = fountainTexture;
                    obstacle.CanOverlapWith = false;
                    obstacle.CanSeeThrough = true;
                    break;
                case ObstacleType.Pond:
                    obstacle.sprite = pondTexture;
                    obstacle.CanOverlapWith = false;
                    obstacle.CanSeeThrough = true;
                    break;
            }
            this.addObstacle(obstacle);
        }

        public void addObstacle(Obstacle obstacle)
        {
            obstacles.Add(obstacle);
        }

        public void removeObstacle(Obstacle obstacle)
        {
            obstacles.Remove(obstacle);
        }

        public void UpdateObstacles()
        {
        }

        public void drawObstacles(SpriteBatch sb)
        {
            foreach (Obstacle obstacle in this.obstacles)
            {
                sb.Draw(obstacle.sprite, obstacle.drawRectangle, Color.White);
            }
        }

        public void LoadObstacleContent(ContentManager cm)
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
