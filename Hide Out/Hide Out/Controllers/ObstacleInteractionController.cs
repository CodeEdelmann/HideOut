using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HideOut.Entities;
using HideOut.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using HideOut.Entities;

namespace HideOut.Controllers
{
    class ObstacleInteractionController
    {
        ObstacleController obstacleController;
        PlayerController playerController;
        SoundEffect slurp;
        public ObstacleInteractionController(ObstacleController oc, PlayerController pc)
        {
            this.obstacleController = oc;
            this.playerController = pc;

        }
             public void LoadContent(ContentManager cm)
        {
            slurp = cm.Load<SoundEffect>("thirst.wav");
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
                       // slurp.play();
                    
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
