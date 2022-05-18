using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TankWars
{
    class Settings
    {
        //Needed data in settings set to default values
        public int UniverseSize { get; private set; } = 2000;
        public int MSPerFrame { get; private set; } = 17;
        public int FramesPerShot { get; private set; } = 80;
        public int RespawnRate { get; private set; } = 300;

        //BasicData set to default values
        public int StartingHitPoints { get; private set; } = 3;
        public double ProjectileSpeed { get; private set; } = 25;
        public double EngineSpeed { get; private set; } = 3.0;
        public int TankSize { get; private set; } = 60;
        public int WallSize { get; private set; } = 50;
        public int MaxPowerups { get; private set; } = 2;
        public int MaxPowerupDelay { get; private set; } = 1650;

        //GameMode set to default values
        public bool ExtraMode { get; private set; } = false;
        //ID of walls
        private int WallOrder = 0;
        //WallVector stack used in XML reading when creating a new wall
        private Stack<int> WallVector;
        //Dictionary of walls
        public Dictionary<int, Wall> Walls { get; private set; }

        /// <summary>
        /// Retrieves settings from XML file
        /// </summary>
        /// <param name="filepath">filepath of the xml file</param>
        public Settings(string filepath)
        {
            WallVector = new Stack<int>();
            Walls = new Dictionary<int, Wall>();
            ParseXML(filepath);
        }

        /// <summary>
        /// Assigns the instance variables to their values in the XML settings files
        /// </summary>
        /// <param name="filepath"></param>
        private void ParseXML(string filepath)
        {
            using (XmlReader reader = XmlReader.Create(filepath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "UniverseSize":
                                reader.Read();
                                UniverseSize = int.Parse(reader.Value);
                                break;
                            case "MSPerFrame":
                                reader.Read();
                                MSPerFrame = int.Parse(reader.Value);
                                break; 

                            case "FramesPerShot":
                                reader.Read();
                                FramesPerShot = int.Parse(reader.Value);
                                break;

                            case "RespawnRate":
                                reader.Read();
                                RespawnRate = int.Parse(reader.Value);
                                break;

                            case "StartingHitPoints":
                                reader.Read();
                                StartingHitPoints = int.Parse(reader.Value);
                                break;

                            case "ProjectileSpeed":
                                reader.Read();
                                ProjectileSpeed = double.Parse(reader.Value);
                                break;

                            case "EngineSpeed":
                                reader.Read();
                                EngineSpeed = double.Parse(reader.Value);
                                break;

                            case "TankSize":
                                reader.Read();
                                TankSize = int.Parse(reader.Value);
                                break;

                            case "WallSize":
                                reader.Read();
                                WallSize = int.Parse(reader.Value);
                                break;

                            case "MaxPowerups":
                                reader.Read();
                                MaxPowerups = int.Parse(reader.Value);
                                break;

                            case "MaxPowerupDelay":
                                reader.Read();
                                MaxPowerupDelay = int.Parse(reader.Value);
                                break;

                            case "Mode":
                                reader.Read();
                                string value = (reader.Value).ToLower();
                                if (value == "basic")
                                    ExtraMode = false;
                                if (value == "extra")
                                    ExtraMode = true;
                                break;

                            case "Wall":
                                break;

                            case "p1":
                                break;

                            case "x":
                                reader.Read();
                                WallVector.Push(int.Parse(reader.Value));
                                break;

                            case "y":
                                reader.Read();
                                WallVector.Push(int.Parse(reader.Value));
                                break;

                            case "p2":
                                break;


                        }
                    }
                    else // End element
                    {

                        if (reader.Name == "Wall")
                        {
                            Vector2D p1 = new Vector2D((double) WallVector.Pop(), (double) WallVector.Pop());
                            Vector2D p2 = new Vector2D((double)WallVector.Pop(), (double)WallVector.Pop());
                            Walls.Add(WallOrder, new Wall(WallOrder, p1, p2));
                            WallOrder++;
                        }
                            
                    }
                }
            }

        }
    }
}
