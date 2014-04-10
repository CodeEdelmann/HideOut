using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using HideOut.Entities;


namespace HideOut.Controllers
{
    class EntityGenerationController
    {
        Entity dragAndDropEntity;
        KeyboardState oldState;
        bool isListening;
        public ItemController itemController { get; set; }
        public NPCController npcController { get; set; }
        public ObstacleController obstacleController { get; set; }
        MouseState oldMouseState;

        public EntityGenerationController()
        {
            oldState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();
            isListening = false;
        }

        public EntityGenerationController(ItemController ic, NPCController nc, ObstacleController oc) : this()
        {
            itemController = ic;
            npcController = nc;
            obstacleController = oc;
        }

        public void UpdateDragAndDrop()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed)
            {
                Point p = new Point(mouse.X, mouse.Y);
                Entity e = GetEntity(p);
                if (e != null)
                {
                    dragAndDropEntity = e;
                }
            }
            else if (mouse.LeftButton != ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                dragAndDropEntity = null;
            }
            else if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                if (dragAndDropEntity != null)
                {
                    int dx = mouse.X - oldMouseState.X;
                    int dy = mouse.Y - oldMouseState.Y;
                    dragAndDropEntity.position = dragAndDropEntity.position + new Vector2(dx, dy);
                }
            }
            oldMouseState = mouse;
        }

        public Entity GetEntity(Point p)
        {
            foreach(Item i in itemController.activeItems)
            {
                if (i.screenRectangle.Contains(p))
                    return i;
            }
            foreach (Obstacle i in obstacleController.obstacles)
            {
                if (i.screenRectangle.Contains(p))
                    return i;
            }
            foreach (NPC i in npcController.npcs)
            {
                if (i.screenRectangle.Contains(p))
                    return i;
            }
            return null;
        }

        public void DeleteEntity(MouseState mouseState)
        {
            int x = mouseState.X;
            int y = mouseState.Y;
            Point p = new Point(x, y);
            List<Entity> toRemove = new List<Entity>();
            foreach(Item i in itemController.activeItems)
            {
                if(i.screenRectangle.Contains(p))
                {
                    toRemove.Add(i);
                }
            }
            foreach (Entity i in toRemove)
            {
                itemController.RemoveItem((Item)i);
                return;
            }

            foreach (Obstacle i in obstacleController.obstacles)
            {
                if (i.screenRectangle.Contains(p))
                {
                    toRemove.Add(i);
                }
            }

            foreach (Entity i in toRemove)
            {
                obstacleController.RemoveObstacle((Obstacle)i);
                return;
            }

            foreach (NPC i in npcController.npcs)
            {
                if (i.screenRectangle.Contains(p))
                {
                    toRemove.Add(i);
                }
            }

            foreach (Entity i in toRemove)
            {
                npcController.RemoveNPC((NPC)i);
                return;
            }

        }

        public void Update(GameTime gameTime)
        {
            UpdateDragAndDrop();
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.D) && !oldState.IsKeyDown(Keys.D))
            {
                DeleteEntity(Mouse.GetState());
            }

            if (newState.IsKeyDown(Keys.M) && !oldState.IsKeyDown(Keys.M))
            {
                isListening = !isListening;
            }
            if (isListening)
            {
                MouseState mouseState = Mouse.GetState();

                if (newState.IsKeyDown(Keys.P) && !oldState.IsKeyDown(Keys.P))
                {
                    npcController.CreateNPC(Entities.NPCType.PoliceB, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
                else if (newState.IsKeyDown(Keys.A) && !oldState.IsKeyDown(Keys.A))
                {
                    itemController.CreateItem(Entities.ItemType.Apple, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
                else if (newState.IsKeyDown(Keys.C) && !oldState.IsKeyDown(Keys.C))
                {
                    itemController.CreateItem(Entities.ItemType.CandyBar, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
                else if (newState.IsKeyDown(Keys.O) && !oldState.IsKeyDown(Keys.O))
                {
                    itemController.CreateItem(Entities.ItemType.Coin, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
                else if (newState.IsKeyDown(Keys.W) && !oldState.IsKeyDown(Keys.W))
                {
                    itemController.CreateItem(Entities.ItemType.WaterBottle, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
                else if (newState.IsKeyDown(Keys.B) && !oldState.IsKeyDown(Keys.B))
                {
                    obstacleController.CreateObstacle(Entities.ObstacleType.Bush, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
                else if (newState.IsKeyDown(Keys.T) && !oldState.IsKeyDown(Keys.T))
                {
                    obstacleController.CreateObstacle(Entities.ObstacleType.Tree, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
                else if (newState.IsKeyDown(Keys.F) && !oldState.IsKeyDown(Keys.F))
                {
                    obstacleController.CreateObstacle(Entities.ObstacleType.Fountain, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
                else if (newState.IsKeyDown(Keys.N) && !oldState.IsKeyDown(Keys.N))
                {
                    obstacleController.CreateObstacle(Entities.ObstacleType.Pond, new Vector2(mouseState.X + HideOutGame.SCREEN_OFFSET_X, mouseState.Y + HideOutGame.SCREEN_OFFSET_Y));
                    isListening = false;
                }
            }

            oldState = newState;
        }
    }
}
