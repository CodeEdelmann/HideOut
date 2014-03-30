using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using HideOut.Controllers;
using HideOut.Entities;

namespace HideOut
{
    class XMLController
    {
        XmlTextReader reader;
        XmlWriter writer;
        PlayerController pc;
        ObstacleController oc;
        ItemController ic;
        NPCController nc;
        string read_fname;
        string write_fname;
        KeyboardState oldState;
        bool isListening;
        public XMLController(string read_fname, string write_fname, PlayerController pc, ObstacleController oc, ItemController ic, NPCController nc)
        {
            this.read_fname = read_fname;
            this.write_fname = write_fname;
            this.pc = pc;
            this.oc = oc;
            this.ic = ic;
            this.nc = nc;
            oldState = Keyboard.GetState();
            isListening = false;
        }
        public void read()
        {
            this.reader = new XmlTextReader(read_fname);
            string entity = "";
            string type = "";
            int xPos = -1;
            int yPos = -1;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        entity = reader.Name;
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
                        break;
                    case XmlNodeType.Text:
                        type = reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        switch (entity)
                        {
                            case "player":
                                this.pc.CreatePlayer(new Vector2(xPos, yPos));
                                break;
                            case "obstacle":
                                switch (type)
                                {
                                    case "Bush":
                                        this.oc.CreateObstacle(ObstacleType.Bush, new Vector2(xPos, yPos));
                                        break;
                                    case "Fountain":
                                        this.oc.CreateObstacle(ObstacleType.Fountain, new Vector2(xPos, yPos));
                                        break;
                                    case "Pond":
                                        this.oc.CreateObstacle(ObstacleType.Pond, new Vector2(xPos, yPos));
                                        break;
                                    case "Tree":
                                        this.oc.CreateObstacle(ObstacleType.Tree, new Vector2(xPos, yPos));
                                        break;
                                }
                                break;
                            case "item":
                                switch (type)
                                {
                                    case "Apple":
                                        this.ic.CreateItem(ItemType.Apple, new Vector2(xPos, yPos));
                                        break;
                                    case "CandyBar":
                                        this.ic.CreateItem(ItemType.CandyBar, new Vector2(xPos, yPos));
                                        break;
                                    case "WaterBottle":
                                        this.ic.CreateItem(ItemType.WaterBottle, new Vector2(xPos, yPos));
                                        break;
                                    case "Coin":
                                        this.ic.CreateItem(ItemType.Coin, new Vector2(xPos, yPos));
                                        break;
                                }
                                break;
                            case "npc":
                                switch (type)
                                {
                                    case "Police":
                                        this.nc.CreateNPC(NPCType.Police, new Vector2(xPos, yPos));
                                        break;
                                    case "Bird":
                                        this.nc.CreateNPC(NPCType.Bird, new Vector2(xPos, yPos));
                                        break;
                                    case "Squirrel":
                                        this.nc.CreateNPC(NPCType.Squirrel, new Vector2(xPos, yPos));
                                        break;
                                    case "Child":
                                        this.nc.CreateNPC(NPCType.Child, new Vector2(xPos, yPos));
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

                writer.WriteStartElement("player");
                writer.WriteAttributeString("X", Convert.ToString(this.pc.thePlayer.position.X));
                writer.WriteAttributeString("Y", Convert.ToString(this.pc.thePlayer.position.Y));
                writer.WriteFullEndElement();

                foreach (Obstacle o in this.oc.obstacles)
                {
                    writer.WriteStartElement("obstacle");
                    writer.WriteAttributeString("X", Convert.ToString(o.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString(o.position.Y));
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
                    }
                    writer.WriteName(type);
                    writer.WriteFullEndElement();
                }

                foreach (Item i in this.ic.activeItems)
                {
                    writer.WriteStartElement("item");
                    writer.WriteAttributeString("X", Convert.ToString(i.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString(i.position.Y));
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

                foreach (NPC n in this.nc.npcs)
                {
                    writer.WriteStartElement("npc");
                    writer.WriteAttributeString("X", Convert.ToString(n.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString(n.position.Y));
                    string type = "";
                    switch (n.tag)
                    {
                        case NPCType.Bird:
                            type = "Bird";
                            break;
                        case NPCType.Child:
                            type = "Child";
                            break;
                        case NPCType.Police:
                            type = "Police";
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
