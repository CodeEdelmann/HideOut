using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;
using HideOut.Primitives;
using Microsoft.Xna.Framework;

namespace HideOut.Controllers
{
    class CollisionController
    {
        TileController tileController;
        public CollisionController(TileController tileController)
        {
            this.tileController = tileController;
        }

        public List<Item> GetCollidingItems(Rectangle rectangle)
        {
            List<Item> retVal = new List<Item>();
            List<Item> nearbyItems = tileController.GetNearbyItems(rectangle);
            foreach(Item i in nearbyItems)
            {
                if(rectangle.Intersects(i.drawRectangle))
                {
                    retVal.Add(i);
                }
            }
            return retVal;
        }

        public List<Obstacle> GetCollidingObstacles(Rectangle rectangle)
        {
            List<Obstacle> retVal = new List<Obstacle>();
            List<Obstacle> nearbyObstacles = tileController.GetNearbyObstacles(rectangle);
            foreach (Obstacle i in nearbyObstacles)
            {
                if (rectangle.Intersects(i.drawRectangle))
                {
                    retVal.Add(i);
                }
            }
            return retVal;
        }
    }
}
