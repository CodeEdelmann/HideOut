using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HideOut.Entities
{
    enum ObstacleType { Bush, Tree, Fountain, Pond };
    class Obstacle : Entity
    {
        public Boolean canOverlapWith { get; set; }
        public Boolean canSeeThrough { get; set; }
        public ObstacleType tag { get; set; }

        public Obstacle()
        {
        }

        public override string ToString()
        {
            string retVal = base.ToString() +
                "Type: " + this.tag + "\n" +
                "Overlap: " + this.canOverlapWith + "\n" +
                "See: " + this.canSeeThrough + "\n";
            return retVal;
        }
    }
}
