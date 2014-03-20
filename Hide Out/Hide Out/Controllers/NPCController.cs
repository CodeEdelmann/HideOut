using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hide_Out.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Hide_Out.Controllers
{
    class NPCController
    {
        public List<NPC> npcs { get; set; }
        private Texture2D policeTexture;

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

        public void DrawFOVs(GraphicsDevice gd, BasicEffect bs)
        {
            foreach (NPC npc in this.npcs)
            {
                foreach(EffectPass pass in bs.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, npc.vision.getFieldOfViewTriangle(), 0, 1);
                }
            }
        }

        public void DrawNPCs(SpriteBatch sb)
        {
            foreach (NPC npc in this.npcs)
            {
                sb.Draw(npc.sprite, npc.rectangle, Color.White);
            }
        }

        public void LoadNPCContent(ContentManager cm)
        {
            //Start by loading all textures
            //policeTexture = cm.Load<Texture2D>("");  //When sprite is added, uncomment this line and put name of file in between quotes
            
            //Then assign textures to NPCs depending on their tag
            foreach (NPC npc in this.npcs)
            {
                switch (npc.tag)
                {
                    case NPCType.Police: npc.sprite = policeTexture; break;
                }
            }
        }
    }
}
