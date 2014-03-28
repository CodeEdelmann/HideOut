﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HideOut.Entities;
using HideOut.Primitives;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace HideOut.Controllers
{
    class NPCController
    {
        public List<NPC> npcs { get; set; }
        private Texture2D policeTexture;
        private static readonly int SPRITE_SIZE = 50;

        public NPCController()
        {
            npcs = new List<NPC>();
        }

        public void AddNPC(NPC npc)
        {
            npcs.Add(npc);
        }

        public void CreateNPC(NPCType tag, Vector2 position)
        {
            NPC npc = new NPC();
            npc.tag = tag;
            npc.position = position;
            switch (tag)
            {
                case NPCType.Police:
                    npc.sprite = policeTexture;
                    npc.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
                    npc.vision = new Vision(npc.worldRectangle, 500.0f, .25, new Vector2(0, 0), Color.Red);
                    npc.speed = 10;
                    break;
            }
            this.AddNPC(npc);
        }

        public void RemoveNPC(NPC npc)
        {
            npcs.Remove(npc);
        }

        public bool Update(Rectangle playerRectangle)
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
                    gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, npc.vision.GetFieldOfViewTriangle(), 0, 1);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (NPC npc in this.npcs)
            {
                sb.Draw(npc.sprite, npc.screenRectangle, Color.White);
            }
        }

        public void LoadContent(ContentManager cm)
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
