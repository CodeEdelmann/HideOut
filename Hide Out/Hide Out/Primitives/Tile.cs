using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;

namespace HideOut.Primitives
{
    class Tile
    {
        public int height { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public List<Obstacle> obstacles { get; set; }
        public List<NPC> npcs { get; set; }
        public List<Item> items { get; set; }

        public Tile(int x, int y, int height, int width)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            obstacles = new List<Obstacle>();
            npcs = new List<NPC>();
            items = new List<Item>();
        }
    }
}
