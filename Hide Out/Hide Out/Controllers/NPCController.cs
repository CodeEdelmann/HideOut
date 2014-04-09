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
        public CollisionController collisionController { get; set; }

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
            npc.stateTime = 0;
            switch (tag)
            {
                case NPCType.Police:
                    npc.sprite = policeTexture;
                    npc.rectangleBounds = new Point(SPRITE_SIZE, SPRITE_SIZE);
                    int numVisions = 100;
                    float maxWidth = .5f;
                    float viewWidth = maxWidth / numVisions;
                    float viewDistance = 500.0f;
                    for (int i = 0; i < numVisions; i++)
                    {
                        float angle = (i - numVisions/2) * viewWidth;
                        npc.visions.Add(new Vision(npc.worldRectangle, viewDistance, viewWidth / 2, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), Color.Red));
                    }
                    npc.vision = new Vision(npc.worldRectangle, viewDistance, maxWidth / 2, new Vector2(1, 0), Color.Red);
                    npc.rotateSpeed = .0006;
                    npc.moveSpeed = 0.08f;
                    npc.state = NPCState.Patrol;
                    break;
            }
            this.AddNPC(npc);
        }

        public void RemoveNPC(NPC npc)
        {
            npcs.Remove(npc);
            tileController.Remove(npc);
        }

        public bool Update(Player player, List<Obstacle> obstacles, GameTime gameTime)
        {
            foreach (NPC npc in this.npcs)
            {
                List<Obstacle> visibleObstacles = new List<Obstacle>();
                foreach (Obstacle obs in obstacles)
                {
                    if(!obs.canSeeThrough) visibleObstacles.Add(obs);
                }
                foreach (Vision vision in npc.visions)
                {
                    vision.Update(visibleObstacles);
                }
                if(UpdateNPC(npc, player, visibleObstacles, gameTime)) return true;
            }
            return false;
        }

        public bool UpdateNPC(NPC npc, Player player, List<Obstacle> visibleObstacles, GameTime gameTime)
        {   
            if (npc.CanSeePlayer(player, visibleObstacles)) return true;
            switch (npc.tag)
            {
                case NPCType.Police:
                    npc.stateTime += gameTime.ElapsedGameTime.Milliseconds;
                    switch (npc.state)
                    {
                        case NPCState.Patrol: //Currently homes in on the player
                            double angleToPlayer = Math.Atan2(-1*((double)(player.worldRectangle.Y + (player.worldRectangle.Height / 2)) - (npc.worldRectangle.Y + (npc.worldRectangle.Height / 2))), 
                                ((double)(player.worldRectangle.X + (player.worldRectangle.Width / 2)) - (npc.worldRectangle.X + (npc.worldRectangle.Width / 2))));

                            double facingAngle = Math.Atan2(-1*npc.vision.viewDirection.Y, npc.vision.viewDirection.X);

                            double turnToAngle = facingAngle - angleToPlayer;

                            while (turnToAngle < -1 * MathHelper.Pi) turnToAngle += MathHelper.TwoPi;
                            while (turnToAngle > MathHelper.Pi) turnToAngle -= MathHelper.TwoPi;

                            Console.WriteLine(angleToPlayer + " " + facingAngle + " " + turnToAngle);

                            if (turnToAngle > 0) npc.RotateLeft(gameTime);
                            else if (turnToAngle < 0) npc.RotateRight(gameTime);

                            npc.MoveForward(gameTime);
                            if(collisionController.IllegalMove(npc)) npc.MoveBackward(gameTime);

                            break;
                        case NPCState.Chase:
                            break;
                        case NPCState.Investigate:
                            break;
                    }
                    break;
            }
            return false;
        }

        public void DrawFOVs(GraphicsDevice gd, BasicEffect bs)
        {
            foreach (NPC npc in this.npcs)
            {
                foreach (EffectPass pass in bs.CurrentTechnique.Passes)
                {
                    foreach(Vision v in npc.visions){
                        pass.Apply();
                        gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, v.GetFieldOfViewTriangleToDraw(), 0, 1);
                    }
                    //pass.Apply();
                    //gd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, npc.vision.GetFieldOfViewTriangleToDraw(), 0, 1);
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
