﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Primitives;
using HideOut.Entities;
using Microsoft.Xna.Framework;


namespace HideOut.Controllers
{
    class TileController
    {
        public int height { get; set; }
        public int width { get; set; }
        public int tileSize { get; set; }
        ItemController itemController;
        ObstacleController obstacleController;
        NPCController npcController;

        List<List<Tile>> tiles;
        public TileController(ItemController ic, NPCController nc, ObstacleController oc, int gameHeight, int gameWidth, int playerSize)
        {
            itemController = ic;
            obstacleController = oc;
            npcController = nc;
            this.height = gameHeight;
            this.width = gameWidth;
            this.tileSize = playerSize;
            tiles = new List<List<Tile>>();
            for (int x = 0; x < width; x += playerSize)
            {
                List<Tile> row = new List<Tile>();
                for (int y = 0; y < height; y+= playerSize)
                {
                    row.Add(new Tile(x, y, playerSize, playerSize));
                }
                tiles.Add(row);
            }
        }

        //Adds all of the obstacles from the controllers into their respective tiles
        public void InitializeEntities()
        {
            foreach (Item item in itemController.activeItems)
            {
                List<Tile> tiles = this.GetNearbyTiles(item.drawRectangle);
                foreach (Tile tile in tiles)
                {
                    tile.items.Add(item);
                    item.tiles.Add(tile);
                }
            }

            foreach (Obstacle obstacle in obstacleController.obstacles)
            {
                List<Tile> tiles = this.GetNearbyTiles(obstacle.drawRectangle);
                foreach (Tile tile in tiles)
                {
                    tile.obstacles.Add(obstacle);
                    obstacle.tiles.Add(tile);
                }
            }

            foreach (NPC npc in npcController.npcs)
            {
                List<Tile> tiles = this.GetNearbyTiles(npc.drawRectangle);
                foreach (Tile tile in tiles)
                {
                    tile.npcs.Add(npc);
                    npc.tiles.Add(tile);
                }
            }
        }

        private List<Tile> GetNearbyTiles(Rectangle rectangle)
        {
            List<Tile> retVal = new List<Tile>();
            int row = rectangle.X / tileSize;
            int col = rectangle.Y / tileSize;

            int row2 = (rectangle.X + rectangle.Width) / tileSize;
            int col2 = (rectangle.Y + rectangle.Height) / tileSize;

            for (int r = row; r <= row2; r++)
            {
                for (int c = col; c <= col2; c++)
                {
                    retVal.Add(tiles[r][c]);
                }
            }
            return retVal;
        }

        public List<Item> GetNearbyItems(Rectangle rectangle)
        {
            List<Item> retVal = new List<Item>();
            List<Tile> tiles = this.GetNearbyTiles(rectangle);

            foreach (Tile tile in tiles)
            {
                retVal.AddRange(tile.items);
            }
            return retVal;
        }

        public List<Obstacle> GetNearbyObstacles(Rectangle rectangle)
        {
            List<Obstacle> retVal = new List<Obstacle>();
            List<Tile> tiles = this.GetNearbyTiles(rectangle);

            foreach (Tile tile in tiles)
            {
                retVal.AddRange(tile.obstacles);
            }
            return retVal;
        }

        public List<NPC> GetNearbyNPCs(Rectangle rectangle)
        {
            List<NPC> retVal = new List<NPC>();
            List<Tile> tiles = this.GetNearbyTiles(rectangle);

            foreach (Tile tile in tiles)
            {
                retVal.AddRange(tile.npcs);
            }
            return retVal;
        }

    }
}
