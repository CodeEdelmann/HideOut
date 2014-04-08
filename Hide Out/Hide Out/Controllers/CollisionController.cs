using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;
using HideOut.Primitives;
using HideOut.Screens;
using Microsoft.Xna.Framework;

namespace HideOut.Controllers
{
    class CollisionController
    {
        public TileController tileController { get; set; }
        public CollisionController()
        {

        }

        public bool IllegalMove(Entity e)
        {
            return this.GetCollidingObstacles(e).Count > 0 ||
                e.position.X < 0 ||
                e.position.X + e.rectangleBounds.X > LevelScreen.GAME_WIDTH ||
                e.position.Y < 0 ||
                e.position.Y + e.rectangleBounds.Y > LevelScreen.GAME_HEIGHT;
        }

        public List<Item> GetCollidingItems(Entity e)
        {
            List<Item> retVal = new List<Item>();
            List<Item> nearbyItems = tileController.GetNearbyItems(e.collisionRectangle);
            foreach(Item i in nearbyItems)
            {
                if (e.collisionRectangle.Intersects(i.worldRectangle))
                {
                    retVal.Add(i);
                }
            }
            return retVal;
        }

        public List<Obstacle> GetCollidingObstacles(Entity e)
        {
            List<Obstacle> retVal = new List<Obstacle>();
            List<Obstacle> nearbyObstacles = tileController.GetNearbyObstacles(e.collisionRectangle);
            foreach (Obstacle i in nearbyObstacles)
            {
                if (e is Player)
                {
                    if (!i.canOverlapWith && e.collisionRectangle.Intersects(i.worldRectangle))
                    {
                        retVal.Add(i);
                    }
                }
                else if (e is NPC) 
                {
                    if(e.collisionRectangle.Intersects(i.worldRectangle))
                    {
                        retVal.Add(i);
                    }
                }
            }
            return retVal;
        }

        public bool IsHidden(Player p)
        {
            List<Obstacle> nearbyObstacles = tileController.GetNearbyObstacles(p.worldRectangle);
            foreach(Obstacle i in nearbyObstacles)
            {
                if(i.canOverlapWith && i.worldRectangle.Contains(p.worldRectangle))
                {
                    return true;
                }
            }
            return false;
        }

        public bool NearFountain(Player p)
        {
            List<Obstacle> nearbyObstacles = tileController.GetNearbyObstacles(p.worldRectangle);
            foreach (Obstacle i in nearbyObstacles)
            {
                if(i.tag == ObstacleType.Fountain && i.worldRectangle.Intersects(p.worldRectangle))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
