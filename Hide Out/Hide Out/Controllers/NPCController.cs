using System;
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
        public TileController tileController { get; set; }

        private Texture2D policeTexture;
        private static readonly int SPRITE_SIZE = 50;

        public NPCController()
        {
            npcs = new List<NPC>();
        }

        public void AddNPC(NPC npc)
        {
            npcs.Add(npc);
            tileController.Add(npc);
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
            tileController.Remove(npc);
        }

        public bool Update(Rectangle playerRectangle, List<Obstacle> obstacles)
        {
            foreach (NPC npc in this.npcs)
            {
                List<Obstacle> visibleObstacles = new List<Obstacle>();
                foreach (Obstacle obs in obstacles)
                {
                    if(npc.CanSee(obs.collisionRectangle) && !obs.canSeeThrough) visibleObstacles.Add(obs);
                }
                if(UpdateNPC(npc, playerRectangle, visibleObstacles)) return true;
            }
            return false;
        }

        public bool UpdateNPC(NPC npc, Rectangle playerRectangle, List<Obstacle> visibleObstacles)
        {   
            if (npc.CanSeePlayer(playerRectangle, visibleObstacles)) return true;
            switch (npc.tag)
            {
                case NPCType.Police:
                    //npc.Move(npc.vision.viewDirection);
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
                    gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, npc.vision.GetFieldOfViewTriangleToDraw(), 0, 1);
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
