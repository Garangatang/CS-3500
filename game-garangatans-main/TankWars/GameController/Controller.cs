using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace TankWars
{
    public class Controller
    {
        private World theWorld;

        public const int worldSize = 2000;

        private bool movingPressed = false;
        private bool mousePressed = false;
        private bool mouseMoved = false;

        private bool sendAlt = false;

        public event Action UpdateArrived;

        private string moving = "none";
        private string fire = "none";
        private Vector2D tdir = new Vector2D(0, 1);

        public delegate void dataArrived(int i);
        public event dataArrived PlayerIdArrived;

        public delegate void soundEvent();
        public event soundEvent ShotArrived;
        public event soundEvent PlayerDied;
        public event soundEvent BeamArrived;

        private string playerName = "";
        private int tankID;

        private int order = 0;

        private SocketState ourSocketState;

        //add animations
        //add try catch
        //read me files
        //add in line comments


        public Controller()
        {
            theWorld = new World(worldSize);
        }

        public void Connect(string serverName, string pN)
        {
            playerName = pN;
            Networking.ConnectToServer(FirstContact, serverName, 11000);
        }

        /// <summary>
        /// Sending the player name and receiving world size and player ID
        /// </summary>
        /// <param name="state"></param>
        private void FirstContact(SocketState state)
        {
            ourSocketState = state;
            Networking.Send(state.TheSocket, playerName + "\n");
            state.OnNetworkAction = ReceivedData;
            Networking.GetData(state);
        }

        /// <summary>
        /// Receiving data from the server
        /// </summary>
        /// <param name="state"></param>
        private void ReceivedData(SocketState state)
        {
            List<string> newMessages = EnumerateMessages(state);

            // Receives information from server
            ReceivedWorldData(newMessages);

            Networking.GetData(state); //Not sure if this is correct.
        }

        /// <summary>
        /// Enumerates messages from the server and deletes it from string builder
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private List<string> EnumerateMessages(SocketState state)
        {
            string startupData = state.GetData();
            string[] parts = Regex.Split(startupData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.

            List<string> newMessages = new List<string>();

            lock (theWorld)
            {
                foreach (string p in parts)
                {
                    // Ignore empty strings added by the regex splitter
                    if (p.Length == 0)
                        continue;
                    // The regex splitter will include the last string even if it doesn't end with a '\n',
                    // So we need to ignore it if this happens. 
                    if (p[p.Length - 1] != '\n')
                        break;

                    // build a list of messages to send to the view and deserialize in ReceivedWorldData()
                    newMessages.Add(p);

                    // Then remove it from the SocketState's growable buffer
                    state.RemoveData(0, p.Length);
                }
            }

            return newMessages;
        }

        /// <summary>
        /// Helper method for receiving world size and player ID
        /// </summary>
        /// <param name="s"></param>
        private void Startup(string s)
        {
                if (!s.Equals(""))
                {
                    // second string the server sends, is the world
                    if (order == 1)
                    {
                        theWorld.setSize(int.Parse(s));
                        order++;
                    }

                    // first string the server sends, is the tank
                    if (order == 0)
                    {

                        tankID = int.Parse(s);
                        PlayerIdArrived(tankID);
                        order++;
                    }
                }
        }

        /// <summary>
        /// Receiving initial data from the server which is added to their respective dictionary
        /// so it can be drawn by the DrawingPanel.
        /// </summary>
        /// <param name="list"></param>
        private void ReceivedWorldData(IEnumerable<string> list)
        {
            lock (theWorld)
            {

                foreach (string s in list)
                {
                    //receive Tank ID and WorldSize
                    if (order < 2)
                    {
                        Startup(s);
                    }
                    //Receive Everything Else
                    else
                    {

                        JObject obj = JObject.Parse(s);

                        // Deserializing the wall objects received from the server.
                        JToken wallToken = obj["wall"];
                        if (wallToken != null)
                        {
                            Wall wall = JsonConvert.DeserializeObject<Wall>(s);
                            theWorld.Walls.Add(wall.ID, wall);
                        }

                        // Deserializing the tank object received from the server.
                        JToken tankToken = obj["tank"];
                        if (tankToken != null)
                        {
                            Tank tank = JsonConvert.DeserializeObject<Tank>(s);
                            if (!theWorld.Tanks.ContainsKey(tank.ID))
                            {
                                theWorld.Tanks.Add(tank.ID, tank);
                            }
                            else
                            {
                                theWorld.Tanks[tank.ID] = tank;

                                if (tank.died)
                                    PlayerDied.Invoke();

                                if (tank.disconnected)
                                    theWorld.Tanks.Remove(tank.ID);
                            }
                        }

                        // Deserializing the projectile object received from the server.
                        JToken projToken = obj["proj"];
                        if (projToken != null)
                        {
                            Projectile proj = JsonConvert.DeserializeObject<Projectile>(s);
                            if (!theWorld.Projectiles.ContainsKey(proj.ID))
                            {
                                theWorld.Projectiles.Add(proj.ID, proj);
                                ShotArrived.Invoke();
                            }
                            else
                            {
                                if (proj.died)
                                    theWorld.Projectiles.Remove(proj.ID);

                                else
                                    theWorld.Projectiles[proj.ID] = proj;
                            }

                        }

                        //Deserializing the powerup object received from the server.
                        JToken powerToken = obj["power"];
                        if (powerToken != null)
                        {
                            Powerup power = JsonConvert.DeserializeObject<Powerup>(s);
                            if (!theWorld.Powerups.ContainsKey(power.ID))
                            {
                                theWorld.Powerups.Add(power.ID, power);
                            }
                            else
                            {
                                if (power.died)
                                {
                                    theWorld.Powerups.Remove(power.ID);
                                    sendAlt = true;
                                }
                                else
                                    theWorld.Powerups[power.ID] = power;
                            }
                        }

                        JToken beamToken = obj["beam"];
                        if (beamToken != null)
                        {
                            Beam beam = JsonConvert.DeserializeObject<Beam>(s);
                            theWorld.Beams.Add(beam.ID, beam);
                            sendAlt = false;
                            BeamArrived.Invoke();
                        }
                    }
                }
            }
            // Process data coming from the server, and invoking updateArrived
            // since new information will have been received from the server.
            ProcessInputs();
            UpdateArrived.Invoke();

        }

        /// <summary>
        /// Checks which inputs are currently held down
        /// Normally this would send a message to the server
        /// </summary>
        private void ProcessInputs()
        {
            ControlCommand move = new ControlCommand(moving, fire, tdir);
            string controlComm = JsonConvert.SerializeObject(move) + "\n";

            if (movingPressed)
            {
                Networking.Send(ourSocketState.TheSocket, controlComm);
            }

            if (mousePressed)
            {
                Networking.Send(ourSocketState.TheSocket, controlComm);
            }
            if (mouseMoved)
            {
                Networking.Send(ourSocketState.TheSocket, controlComm);
            }
        }

        /// <summary>
        /// Returns the world from the ctonroller
        /// </summary>
        /// <returns></returns>
        public World GetWorld()
        {
            return theWorld;
        }

        /// <summary>
        /// Handling movement request
        /// </summary>
        public void HandleMoveRequest(string s)
        {
            movingPressed = true;
            moving = s;
        }

        /// <summary>
        /// Canceling a movement request
        /// </summary>
        public void CancelMoveRequest(string s)
        {
            movingPressed = false;
            moving = "none";
        }

        /// <summary>
        /// Handling mouse request
        /// </summary>
        public void HandleMouseRequest()
        {
            mousePressed = true;
            if (sendAlt)
                fire = "alt";
            else
                fire = "main";
        }

        /// <summary>
        /// Canceling mouse request
        /// </summary>
        public void CancelMouseRequest()
        {
            mousePressed = false;
            fire = "none";
        }

        /// <summary>
        /// Handling when the turret is spinning.
        /// </summary>
        /// <param name="dir"></param>
        public void turretMoved(Vector2D dir)
        {
            mouseMoved = true;
            tdir = dir;
        }

    }
}
