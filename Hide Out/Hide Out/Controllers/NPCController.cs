using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hide_Out.Entities;

namespace Hide_Out.Controllers
{
    class NPCController
    {
        public List<NPC> npcs { get; set; }

        public NPCController()
        {
            npcs = new List<NPC>();
        }

        public void addNPC(NPC npc)
        {
            npcs.Add(npc);
        }

        public void removeNPC(NPC npc)
        {
            npcs.Remove(npc);
        }

        public void UpdateNPCs()
        {
            foreach (NPC npc in this.npcs)
            {
                UpdateNPC(npc);
            }
        }

        public void UpdateNPC(NPC npc)
        {
            switch (npc.tag)
            {
                case NPCType.Police:
                    npc.Move(npc.vision.viewDirection);
                    npc.Rotate(.05);
                    break;
            }
        }
    }
}
