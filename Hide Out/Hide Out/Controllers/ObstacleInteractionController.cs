using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;

namespace HideOut.Controllers
{
    class ObstacleInteractionController
    {
        ObstacleController obstacleController;
        PlayerController playerController;

        public ObstacleInteractionController(ObstacleController oc, PlayerController pc)
        {
            this.obstacleController = oc;
            this.playerController = pc;
        }

        public void interacts(Player p, Obstacle o)
        {
            switch(o.tag)
            {
                case ObstacleType.Bush:
                    // set player invisible
                    p.isVisible = false;
                    break;
                case ObstacleType.Fountain:
                    p.currentThirst = p.maxThirst;
                    break;
                case ObstacleType.Pond:
                    // do nothing
                    break;
                case ObstacleType.Tree:
                    // do nothing
                    break;
            }
        }
    }
}
