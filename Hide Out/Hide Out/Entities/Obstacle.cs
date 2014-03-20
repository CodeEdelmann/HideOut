using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hide_Out.Entities
{
    enum ObstacleType { Bush, Tree, Fountain, Pond };
    class Obstacle : Entity
    {
        Boolean CanOverlapWith { get; set; }
        Boolean CanSeeThrough { get; set; }
        ObstacleType tag { get; set; }

        public Obstacle(ObstacleType type, Vector2 pos, Texture2D spr)
        {
            position = pos;
            sprite = spr;
            tag = type;
            switch (type)
            {
                case ObstacleType.Bush:
                    CanOverlapWith = true;
                    CanSeeThrough = false;
                    break;
                case ObstacleType.Tree:
                    CanOverlapWith = false;
                    CanSeeThrough = false;
                    break;
                case ObstacleType.Fountain:
                    CanOverlapWith = false;
                    CanSeeThrough = true;
                    break;
                case ObstacleType.Pond:
                    CanOverlapWith = false;
                    CanSeeThrough = true;
                    break;
            }
        }

        public Obstacle(Texture2D spr)
            : this(ObstacleType.Bush, new Vector2(0, 0), spr)
        {
        }
    }
}
