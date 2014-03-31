using System;
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
        public int max_game_height { get; set; }
        public int max_game_width { get; set; }
        public int tileSize { get; set; }
        public ItemController itemController { get; set; }
        public ObstacleController obstacleController { get; set; }
        public NPCController npcController { get; set; }

        List<List<Tile>> tiles;

        public TileController()
        {

        }
        public TileController(ItemController ic, NPCController nc, ObstacleController oc, int gameHeight, int gameWidth, int playerSize) : this()
        {
            itemController = ic;
            obstacleController = oc;
            npcController = nc;
            this.max_game_height = gameHeight;
            this.max_game_width = gameWidth;
            this.tileSize = playerSize;
        }

        public void InitializeTiles()
        {
            tiles = new List<List<Tile>>();
            for (int x = 0; x < max_game_width; x += tileSize)
            {
                List<Tile> row = new List<Tile>();
                for (int y = 0; y < max_game_height; y += tileSize)
                {
                    row.Add(new Tile(x, y, tileSize, tileSize));
                }
                tiles.Add(row);
            }
        }

        //Adds all of the obstacles from the controllers into their respective tiles
        public void InitializeEntities()
        {
            foreach (Item item in itemController.activeItems)
            {
                List<Tile> tiles = this.GetNearbyTiles(item.screenRectangle);
                foreach (Tile tile in tiles)
                {
                    tile.items.Add(item);
                    item.tiles.Add(tile);
                }
            }

            foreach (Obstacle obstacle in obstacleController.obstacles)
            {
                List<Tile> tiles = this.GetNearbyTiles(obstacle.screenRectangle);
                foreach (Tile tile in tiles)
                {
                    tile.obstacles.Add(obstacle);
                    obstacle.tiles.Add(tile);
                }
            }

            foreach (NPC npc in npcController.npcs)
            {
                List<Tile> tiles = this.GetNearbyTiles(npc.screenRectangle);
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
            int row = Math.Max(rectangle.X / tileSize, 0);
            int col = Math.Max(rectangle.Y / tileSize, 0);

            int row2 = Math.Min((rectangle.X + rectangle.Width) / tileSize, this.tiles.Count - 1);
            int col2 = Math.Min((rectangle.Y + rectangle.Height) / tileSize, this.tiles.Count - 1);

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

        public void Remove(Entity e)
        {
            if (e is Item)
            {
                foreach (Tile tile in e.tiles)
                {
                    tile.items.Remove((Item)e);
                }
                e.tiles.Clear();
            }
            else if (e is Obstacle)
            {
                foreach (Tile tile in e.tiles)
                {
                    tile.obstacles.Remove((Obstacle)e);
                }
                e.tiles.Clear();
            }
            else if (e is NPC)
            {
                foreach (Tile tile in e.tiles)
                {
                    tile.npcs.Remove((NPC)e);
                }
                e.tiles.Clear();
            }
        }

        public void Add(Entity e)
        {
            List<Tile> tiles = this.GetNearbyTiles(e.worldRectangle);
            foreach(Tile tile in tiles)
            {
                if (e is Item)
                {
                    tile.items.Add((Item)e);
                    e.tiles.Add(tile);
                }
                else if (e is Obstacle)
                {
                    tile.obstacles.Add((Obstacle)e);
                    e.tiles.Add(tile);
                }
                else if (e is NPC)
                {
                    tile.npcs.Add((NPC)e);
                    e.tiles.Add(tile);
                }
            }
        }

        public void Update(Entity e)
        {
            this.Remove(e);
            this.Add(e);
        }

        public void ClearTiles()
        {
            foreach (List<Tile> l in tiles)
            {
                foreach (Tile t in l)
                {
                    t.items.Clear();
                    t.npcs.Clear();
                    t.obstacles.Clear();
                }
            }
        }

        
    }
}
