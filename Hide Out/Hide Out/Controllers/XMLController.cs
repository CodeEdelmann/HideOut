using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using HideOut.Controllers;
using HideOut.Entities;
using HideOut.Screens;

namespace HideOut
{
    class XMLController
    {
        XmlTextReader reader;
        XmlWriter writer;
        public PlayerController playerController { get; set; }
        public ObstacleController obstacleController { get; set; }
        public ItemController itemController { get; set; }
        public NPCController npcController { get; set; }
        public string read_fname { get; set; }
        public string write_fname;
        KeyboardState oldState;
        bool isListening;
        public XMLController()
        {
            oldState = Keyboard.GetState();
            isListening = false;
        }
        public XMLController(string read_fname, string write_fname, PlayerController pc, ObstacleController oc, ItemController ic, NPCController nc) : this()
        {
            this.read_fname = read_fname;
            this.write_fname = write_fname;
            this.playerController = pc;
            this.obstacleController = oc;
            this.itemController = ic;
            this.npcController = nc;
        }
        public void read()
        {
            this.reader = new XmlTextReader(read_fname);
            string type = "";
            int xPos = -1;
            int yPos = -1;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        string name = reader.Name;
                        while (reader.MoveToNextAttribute())
                        {
                            switch (reader.Name)
                            {
                                case "X":
                                    Int32.TryParse(reader.Value, out xPos);
                                    break;
                                case "Y":
                                    Int32.TryParse(reader.Value, out yPos);
                                    break;
                            }
                        }
                        switch (name)
                        {
                            case "world":
                                LevelScreen.GAME_WIDTH = xPos;
                                LevelScreen.GAME_HEIGHT = yPos;
                                break;
                        }
                        break;
                    case XmlNodeType.Text:
                        type = reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        switch (reader.Name)
                        {
                            case "player":
                                this.playerController.CreatePlayer(new Vector2(xPos, yPos));
                                break;
                            case "obstacle":
                                switch (type)
                                {
                                    case "Bush":
                                        this.obstacleController.CreateObstacle(ObstacleType.Bush, new Vector2(xPos, yPos));
                                        break;
                                    case "Fountain":
                                        this.obstacleController.CreateObstacle(ObstacleType.Fountain, new Vector2(xPos, yPos));
                                        break;
                                    case "Pond":
                                        this.obstacleController.CreateObstacle(ObstacleType.Pond, new Vector2(xPos, yPos));
                                        break;
                                    case "Tree":
                                        this.obstacleController.CreateObstacle(ObstacleType.Tree, new Vector2(xPos, yPos));
                                        break;
                                    case "Border":
                                        this.obstacleController.CreateObstacle(ObstacleType.Border, new Vector2(xPos, yPos));
                                        break;
                                }
                                break;
                            case "item":
                                switch (type)
                                {
                                    case "Apple":
                                        this.itemController.CreateItem(ItemType.Apple, new Vector2(xPos, yPos));
                                        break;
                                    case "CandyBar":
                                        this.itemController.CreateItem(ItemType.CandyBar, new Vector2(xPos, yPos));
                                        break;
                                    case "WaterBottle":
                                        this.itemController.CreateItem(ItemType.WaterBottle, new Vector2(xPos, yPos));
                                        break;
                                    case "Coin":
                                        this.itemController.CreateItem(ItemType.Coin, new Vector2(xPos, yPos));
                                        break;
                                }
                                break;
                            case "npc":
                                switch (type)
                                {
                                    case "PoliceA":
                                        this.npcController.CreateNPC(NPCType.PoliceA, new Vector2(xPos, yPos));
                                        break;
                                    case "PoliceB":
                                        this.npcController.CreateNPC(NPCType.PoliceB, new Vector2(xPos, yPos));
                                        break;
                                    case "Bird":
                                        this.npcController.CreateNPC(NPCType.Bird, new Vector2(xPos, yPos));
                                        break;
                                    case "Squirrel":
                                        this.npcController.CreateNPC(NPCType.Squirrel, new Vector2(xPos, yPos));
                                        break;
                                    case "Child":
                                        this.npcController.CreateNPC(NPCType.Child, new Vector2(xPos, yPos));
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
            reader.Close();
        }
        public void write()
        {
            using (this.writer = XmlWriter.Create(write_fname))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("world");
                writer.WriteAttributeString("X", Convert.ToString(LevelScreen.GAME_WIDTH));
                writer.WriteAttributeString("Y", Convert.ToString(LevelScreen.GAME_HEIGHT));

                writer.WriteStartElement("player");
                writer.WriteAttributeString("X", Convert.ToString((int)this.playerController.thePlayer.position.X));
                writer.WriteAttributeString("Y", Convert.ToString((int)this.playerController.thePlayer.position.Y));
                writer.WriteFullEndElement();

                foreach (Obstacle o in this.obstacleController.obstacles)
                {
                    writer.WriteStartElement("obstacle");
                    writer.WriteAttributeString("X", Convert.ToString((int)o.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString((int)o.position.Y));
                    string type = "";
                    switch (o.tag)
                    {
                        case ObstacleType.Bush:
                            type = "Bush";
                            break;
                        case ObstacleType.Fountain:
                            type = "Fountain";
                            break;
                        case ObstacleType.Pond:
                            type = "Pond";
                            break;
                        case ObstacleType.Tree:
                            type = "Tree";
                            break;
                        case ObstacleType.Border:
                            type = "Border";
                            break;
                    }
                    writer.WriteName(type);
                    writer.WriteFullEndElement();
                }

                foreach (Item i in this.itemController.activeItems)
                {
                    writer.WriteStartElement("item");
                    writer.WriteAttributeString("X", Convert.ToString((int)i.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString((int)i.position.Y));
                    string type = "";
                    switch (i.tag)
                    {
                        case ItemType.Apple:
                            type = "Apple";
                            break;
                        case ItemType.CandyBar:
                            type = "CandyBar";
                            break;
                        case ItemType.WaterBottle:
                            type = "WaterBottle";
                            break;
                        case ItemType.Coin:
                            type = "Coin";
                            break;
                    }
                    writer.WriteName(type);
                    writer.WriteFullEndElement();
                }

                foreach (NPC n in this.npcController.npcs)
                {
                    writer.WriteStartElement("npc");
                    writer.WriteAttributeString("X", Convert.ToString((int)n.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString((int)n.position.Y));
                    string type = "";
                    switch (n.tag)
                    {
                        case NPCType.Bird:
                            type = "Bird";
                            break;
                        case NPCType.Child:
                            type = "Child";
                            break;
                        case NPCType.PoliceA:
                            type = "PoliceA";
                            break;
                        case NPCType.PoliceB:
                            type = "PoliceB";
                            break;                     
                        case NPCType.Squirrel:
                            type = "Squirrel";
                            break;
                    }
                    writer.WriteName(type);
                    writer.WriteFullEndElement();
                }

                writer.WriteFullEndElement();
            }
            writer.Close();
        }
        public void Update()
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.LeftControl) && !oldState.IsKeyDown(Keys.LeftControl))
            {
                isListening = !isListening;
            }
            if (isListening)
            {
                if (newState.IsKeyDown(Keys.S) && !oldState.IsKeyDown(Keys.S))
                {
                    this.write();
                    isListening = false;
                }
            }
        }
    }
}
