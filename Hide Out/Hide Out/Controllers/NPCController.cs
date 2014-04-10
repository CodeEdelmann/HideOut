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
                case NPCType.PoliceA:
                    //lack of break here is intentional!  We want both police managed the same way here
                case NPCType.PoliceB:
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
                    npc.rotateSpeed = .001;
                    npc.moveSpeed = 0.1f;
                    npc.state = NPCState.PatrolLeft;
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
                case NPCType.PoliceA:
                    {
                        npc.MoveForward(gameTime, 1.0f);
                        if(collisionController.IllegalMove(npc)) npc.MoveBackward(gameTime, 1.0f);
                        npc.RotateRight(gameTime, 1.0f);
                    }
                    break;
                case NPCType.PoliceB:
                    npc.stateTime += gameTime.ElapsedGameTime.Milliseconds;

                    double angleToPlayer = Math.Atan2(-1*((double)(player.worldRectangle.Y + (player.worldRectangle.Height / 2)) - (npc.worldRectangle.Y + (npc.worldRectangle.Height / 2))), 
                                ((double)(player.worldRectangle.X + (player.worldRectangle.Width / 2)) - (npc.worldRectangle.X + (npc.worldRectangle.Width / 2))));

                    double facingAngle = Math.Atan2(-1*npc.vision.viewDirection.Y, npc.vision.viewDirection.X);

                    double turnToAngle = facingAngle - angleToPlayer;

                    while (turnToAngle < -1 * MathHelper.Pi) turnToAngle += MathHelper.TwoPi;
                    while (turnToAngle > MathHelper.Pi) turnToAngle -= MathHelper.TwoPi;

                    switch (npc.state)
                    {
                        case NPCState.PatrolRight:
                            npc.MoveForward(gameTime, 1.0f);
                            if (collisionController.IllegalMove(npc))
                            {
                                npc.MoveBackward(gameTime, 1.0f);
                                npc.state = NPCState.PatrolRightBackwards;
                                npc.stateTime = 0;
                            }
                            else if (npc.stateTime > 3000)
                            {
                                npc.state = NPCState.PatrolLeft;
                                npc.stateTime = 0;
                            }
                            else npc.RotateRight(gameTime, 1.0f);

                            break;

                        case NPCState.PatrolLeft:
                            npc.MoveForward(gameTime, 1.0f);
                            if (collisionController.IllegalMove(npc))
                            {
                                npc.MoveBackward(gameTime, 1.0f);
                                npc.state = NPCState.PatrolLeftBackwards;
                                npc.stateTime = 0;
                            }
                            else if (npc.stateTime > 3000)
                            {
                                npc.state = NPCState.PatrolRight;
                                npc.stateTime = 0;
                            }
                            else npc.RotateLeft(gameTime, 1.0f);

                            break;

                        case NPCState.PatrolLeftBackwards:
                            npc.MoveBackward(gameTime, 1.0f);
                            if (collisionController.IllegalMove(npc))
                            {
                                npc.MoveForward(gameTime, 1.0f);
                                npc.state = NPCState.PatrolLeft;
                                npc.stateTime = 0;
                            }
                            else if (npc.stateTime > 250)
                            {
                                npc.state = NPCState.PatrolLeft;
                                npc.stateTime = 0;
                            }
                            else npc.RotateLeft(gameTime, 2.5f);

                            break;

                        case NPCState.PatrolRightBackwards:
                            npc.MoveBackward(gameTime, 1.0f);
                            if (collisionController.IllegalMove(npc))
                            {
                                npc.MoveForward(gameTime, 1.0f);
                                npc.state = NPCState.PatrolRight;
                                npc.stateTime = 0;
                            }
                            else if (npc.stateTime > 250)
                            {
                                npc.state = NPCState.PatrolRight;
                                npc.stateTime = 0;
                            }
                            else npc.RotateRight(gameTime, 2.5f);

                            break;

                        case NPCState.Chase:
                            if (!player.isVisible)
                            {
                                npc.state = NPCState.PatrolLeft; //Randomize
                                npc.stateTime = 0;
                                break;
                            }
                            if (turnToAngle > 0) npc.RotateLeft(gameTime, 2.0f);
                            else if (turnToAngle < 0) npc.RotateRight(gameTime, 2.0f);

                            npc.MoveForward(gameTime, 2.0f);
                            if (collisionController.IllegalMove(npc))
                            {
                                npc.MoveBackward(gameTime, 2.0f);
                                if (turnToAngle > 0)
                                {
                                    npc.MoveUp(gameTime, 2.0f);
                                    if (collisionController.IllegalMove(npc))
                                    {
                                        npc.MoveDown(gameTime, 2.0f);
                                        if (Math.Abs(turnToAngle) < MathHelper.PiOver2)
                                        {
                                            npc.MoveRight(gameTime, 2.0f);
                                            if (collisionController.IllegalMove(npc))
                                            {
                                                npc.MoveLeft(gameTime, 2.0f);
                                                npc.state = NPCState.PatrolLeft; //Randomize
                                                npc.stateTime = 0;
                                            }
                                        }
                                    }
                                }
                                else if (turnToAngle < 0)
                                {
                                    npc.MoveDown(gameTime, 2.0f);
                                    if (collisionController.IllegalMove(npc))
                                    {
                                        npc.MoveUp(gameTime, 2.0f);
                                        if (Math.Abs(turnToAngle) < MathHelper.PiOver2)
                                        {
                                            npc.MoveLeft(gameTime, 2.0f);
                                            if (collisionController.IllegalMove(npc))
                                            {
                                                npc.MoveRight(gameTime, 2.0f);
                                                npc.state = NPCState.PatrolLeft; //Randomize
                                                npc.stateTime = 0;
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        case NPCState.Investigate:
                            if (!player.isVisible || npc.vision.maxViewDistance / 10 < ((player.worldRectangle.Y + (player.worldRectangle.Height / 2)) - (npc.worldRectangle.Y + (npc.worldRectangle.Height / 2)) +
                                (player.worldRectangle.X + (player.worldRectangle.Width / 2)) - (npc.worldRectangle.X + (npc.worldRectangle.Width / 2))))
                            {
                                npc.state = NPCState.PatrolLeft; //Randomize
                                npc.stateTime = 0;
                                break;
                            }
                            if (Math.Abs(turnToAngle) < .001)
                            {
                                npc.MoveForward(gameTime, 0.5f);
                            }
                            else if (turnToAngle > 0) npc.RotateLeft(gameTime, 3.0f);
                            else if (turnToAngle < 0) npc.RotateRight(gameTime, 3.0f);
                            break;
                    }

                    if (player.isVisible && npc.vision.maxViewDistance / 20 > ((player.worldRectangle.Y + (player.worldRectangle.Height / 2)) - (npc.worldRectangle.Y + (npc.worldRectangle.Height / 2)) +
                        (player.worldRectangle.X + (player.worldRectangle.Width / 2)) - (npc.worldRectangle.X + (npc.worldRectangle.Width / 2))))
                    {
                        npc.state = NPCState.Investigate;
                        npc.stateTime = 0;
                    }
                    else if (Math.Abs(turnToAngle) < npc.vision.viewAngle / 2 && visibleObstacles.Count == 0 && player.isVisible)
                    {
                        npc.state = NPCState.Chase;
                        npc.stateTime = 0;
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
                    case NPCType.PoliceA: 
                        npc.sprite = policeTexture; 
                        break;
                    case NPCType.PoliceB:
                        npc.sprite = policeTexture;
                        break;
                }
            }
        }
    }
}
