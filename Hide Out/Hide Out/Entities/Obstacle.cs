using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hide_Out.Entities
{
    enum ObstacleType { Bush, Tree, Fountain, Pond };
    class Obstacle : Entity
    {
        public Boolean CanOverlapWith { get; set; }
        public Boolean CanSeeThrough { get; set; }
        public ObstacleType tag { get; set; }

        public Obstacle()
        {
        }

        public override string ToString()
        {
            string retVal = base.ToString() +
                "Type: " + this.tag +
                "Overlap: " + this.CanOverlapWith +
                "See: " + this.CanSeeThrough;
            return retVal;
        }
    }
}
