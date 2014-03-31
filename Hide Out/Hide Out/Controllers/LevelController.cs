using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

        int currentLevel;

        public LevelController()
        {
            currentLevel = 1;
        }

        public bool Update()
        {
            if (itemController.numCoins <= 0)
            {
                currentLevel++;
                ClearLevel();
                return InitializeLevel(currentLevel);
            }
            return true;
        }

        public void ClearLevel()
        {
            npcController.npcs.Clear();
            itemController.activeItems.Clear();
            obstacleController.obstacles.Clear();
            tileController.ClearTiles();
        }

        public bool InitializeLevel(int level)
        {
            currentLevel = level;
            string newFileName = PATH + level.ToString() + ".xml";
            if(File.Exists(newFileName))
            {
                xmlController.read_fname = newFileName;
                xmlController.read();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
