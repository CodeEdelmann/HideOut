using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hide_Out.Entities;
using Hide_Out.Primatives;
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

        public void createNPC(NPCType tag, Vector2 position)
        {
            NPC npc = new NPC();
            npc.tag = tag;
            npc.position = position;
            switch (tag)
            {
                case NPCType.Police:
                    npc.sprite = policeTexture;
                    npc.rectangleBounds = new Point(100, 100);
                    npc.vision = new Vision(npc.drawRectangle, 500.0f, .25, new Vector2(0, 0), Color.Red);
                    npc.speed = 10;
                    break;
            }
            this.addNPC(npc);
        }

        public void removeNPC(NPC npc)
        {
            npcs.Remove(npc);
        }

        public bool UpdateNPCs(Rectangle playerRectangle)
        {
            foreach (NPC npc in this.npcs)
            {
                if(UpdateNPC(npc, playerRectangle)) return true;
            }
            return false;
        }

        public bool UpdateNPC(NPC npc, Rectangle playerRectangle)
        {
            switch (npc.tag)
            {
                case NPCType.Police:
                    //npc.Move(npc.vision.viewDirection);
                    if (npc.CanSee(playerRectangle)) return true;
                    npc.Rotate(.01);
                    break;
            }
            return false;
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
                sb.Draw(npc.sprite, npc.drawRectangle, Color.White);
            }
        }

        public void LoadNPCContent(ContentManager cm)
        {
            //Start by loading all textures
            policeTexture = cm.Load<Texture2D>("police"); 
            
            //Then assign textures to NPCs depending on their tag
            foreach (NPC npc in this.npcs)
            {
                switch (npc.tag)
                {
                    case NPCType.Police: 
                        npc.sprite = policeTexture; 
                        break;
                }
            }
        }
    }
}
