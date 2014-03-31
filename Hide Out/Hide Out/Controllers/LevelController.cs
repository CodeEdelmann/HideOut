using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideOut.Controllers
{
    class LevelController
    {
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

        public void Update()
        {
            if (itemController.numCoins <= 0)
            {
                currentLevel++;
                ClearLevel();
                InitializeLevel(currentLevel);
            }
        }

        public void ClearLevel()
        {
            npcController.npcs.Clear();
            itemController.activeItems.Clear();
            obstacleController.obstacles.Clear();
            tileController.ClearTiles();
        }

        public void InitializeLevel(int level)
        {

        }
    }
}
