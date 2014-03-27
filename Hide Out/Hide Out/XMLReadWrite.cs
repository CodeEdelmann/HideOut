using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using HideOut.Controllers;
using HideOut.Entities;

namespace HideOut
{
    class XMLReadWrite
    {
        private static XmlTextReader reader;
        private static XmlWriter writer;
        public static void read(String fname, PlayerController pc, ObstacleController oc, ItemController ic, NPCController nc)
        {
            reader = new XmlTextReader(fname);
            String entity = "";
            String type = "";
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
                                pc.CreatePlayer(new Vector2(xPos, yPos));
                                break;
                            case "obstacle":
                                switch (type)
                                {
                                    case "Bush":
                                        oc.CreateObstacle(ObstacleType.Bush, new Vector2(xPos, yPos));
                                        break;
                                    case "Fountain":
                                        oc.CreateObstacle(ObstacleType.Fountain, new Vector2(xPos, yPos));
                                        break;
                                    case "Pond":
                                        oc.CreateObstacle(ObstacleType.Pond, new Vector2(xPos, yPos));
                                        break;
                                    case "Tree":
                                        oc.CreateObstacle(ObstacleType.Tree, new Vector2(xPos, yPos));
                                        break;
                                }
                                break;
                            case "item":
                                switch (type)
                                {
                                    case "Apple":
                                        ic.CreateItem(ItemType.Apple, new Vector2(xPos, yPos));
                                        break;
                                    case "CandyBar":
                                        ic.CreateItem(ItemType.CandyBar, new Vector2(xPos, yPos));
                                        break;
                                    case "WaterBottle":
                                        ic.CreateItem(ItemType.WaterBottle, new Vector2(xPos, yPos));
                                        break;
                                }
                                break;
                            case "npc":
                                switch (type)
                                {
                                    case "Police":
                                        nc.CreateNPC(NPCType.Police, new Vector2(xPos, yPos));
                                        break;
                                    case "Bird":
                                        nc.CreateNPC(NPCType.Bird, new Vector2(xPos, yPos));
                                        break;
                                    case "Squirrel":
                                        nc.CreateNPC(NPCType.Squirrel, new Vector2(xPos, yPos));
                                        break;
                                    case "Child":
                                        nc.CreateNPC(NPCType.Child, new Vector2(xPos, yPos));
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
        }
        public static void write(String fname, PlayerController pc, ObstacleController oc, ItemController ic, NPCController nc)
        {
            using (writer = XmlWriter.Create(fname))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("world");

                writer.WriteStartElement("player");
                writer.WriteAttributeString("X", Convert.ToString(pc.thePlayer.position.X));
                writer.WriteAttributeString("Y", Convert.ToString(pc.thePlayer.position.Y));
                writer.WriteEndElement();

                foreach (Obstacle o in oc.obstacles)
                {
                    writer.WriteStartElement("obstacle");
                    writer.WriteAttributeString("X", Convert.ToString(o.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString(o.position.Y));
                    String type = "";
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
                    writer.WriteEndElement();
                }

                foreach (Item i in ic.activeItems)
                {
                    writer.WriteStartElement("item");
                    writer.WriteAttributeString("X", Convert.ToString(i.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString(i.position.Y));
                    String type = "";
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
                    }
                    writer.WriteName(type);
                    writer.WriteEndElement();
                }

                foreach (NPC n in nc.npcs)
                {
                    writer.WriteStartElement("npc");
                    writer.WriteAttributeString("X", Convert.ToString(n.position.X));
                    writer.WriteAttributeString("Y", Convert.ToString(n.position.Y));
                    String type = "";
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
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }
    }
}
