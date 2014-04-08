using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HideOut.Controllers
{
    class LevelController
    {
        public static readonly string PATH = "Content/Levels/";
        public XMLController xmlController { get; set; }
        public PlayerController playerController { get; set; }
        public NPCController npcController { get; set; }
        public ItemController itemController { get; set; }
        public ObstacleController obstacleController { get; set; }
        public TileController tileController { get; set; }

        public int currentLevel { get; set; }

        public LevelController()
        {
            currentLevel = 1;
        }

        public int Update()
        {
            if (itemController.numCoins <= 0)
            {
                currentLevel++;
                return InitializeLevel(currentLevel);
            }
            return 0; // continue level
        }

        public void ClearLevel()
        {
            npcController.npcs.Clear();
            itemController.ClearItems();
            obstacleController.obstacles.Clear();
            tileController.ClearTiles();
        }

        public int InitializeLevel(int level)
        {
            ClearLevel();
            currentLevel = level;
            RecordLevel(level);
            string newFileName = PATH + level.ToString() + ".xml";
            if (File.Exists(newFileName))
            {
                xmlController.read_fname = newFileName;
                xmlController.read();
                return 1; // load next level
            }
            else
            {
                RecordLevel(1);
                return 2; // last level, you win
            }
        }

        public void RecordLevel(int level)
        {
            System.IO.File.WriteAllText("Content\\Levels\\savestate.txt", level.ToString());
        }
    }
}
