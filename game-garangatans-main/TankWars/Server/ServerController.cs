using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TankWars
{
    class ServerController
    {
        private Settings settings;
        private World theWorld;
        private Dictionary<int, SocketState> clients = new Dictionary<int, SocketState>();
        private string startupInfo;//startupInfo is sent to clients when they first join

        /// <summary>
        /// Creates a server based on settings file
        /// </summary>
        /// <param name="settings"></param>
        public ServerController(Settings settings)
        {
            this.settings = settings;
            StartupFromSettings(); //retrieves info from settings
            foreach (Wall wall in settings.Walls.Values)
            {
                theWorld.Walls.Add(wall.ID, wall);//adding walls to the dictionary
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(theWorld.Size);//ading world size to startup info
            sb.Append("\n");
            foreach (Wall wall in theWorld.Walls.Values)
            {
                sb.Append(wall.ToString());//adding walls to startupInfo
            }
            startupInfo = sb.ToString();
        }

        /// <summary>
        /// Assigns static variables from the settings file.
        /// </summary>
        private void StartupFromSettings()
        {
            World.ExtraGameMode = settings.ExtraMode;
            theWorld = new World(settings.UniverseSize);
            Tank.FramesPerShot = settings.FramesPerShot;
            Tank.RespawnDelay = settings.RespawnRate;
            Tank.StartingHitPoints = settings.StartingHitPoints;
            Projectile.Speed = settings.ProjectileSpeed;
            Tank.DefaultEnginePower = settings.EngineSpeed;
            Tank.Size = settings.TankSize;
            Wall.Thickness = settings.WallSize;
            Powerup.MaxPowerups = settings.MaxPowerups;
            Powerup.MaxPowerupDelay = settings.MaxPowerupDelay;
        }

        /// <summary>
        /// Starts the server with given port
        /// </summary>
        internal void Start()
        {
            Networking.StartServer(NewClient, 11000);
            Thread t = new Thread(Update);
            t.Start();
            Console.WriteLine("Server is running. Accepting clients.");
        }

        /// <summary>
        /// Updates the world and send JSONs to all clients
        /// </summary>
        private void Update()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //begins a thread that updates the world according to the MSPerFrame
            while (true)
            {
                while (watch.ElapsedMilliseconds < settings.MSPerFrame) ;

                watch.Restart();

                StringBuilder sb = new StringBuilder();
                lock (theWorld)
                {
                    theWorld.Update();
                    //adding tanks
                    foreach (Tank tank in theWorld.Tanks.Values)
                    {
                        sb.Append(tank.ToString());
                        if (tank.disconnected)
                            theWorld.Tanks.Remove(tank.ID);
                    }
                    //adding projectiles
                    foreach(Projectile proj in theWorld.Projectiles.Values)
                    {
                        sb.Append(proj.ToString());
                    }
                    //adding powerups
                    foreach(Powerup powerup in theWorld.Powerups.Values)
                    {
                        sb.Append(powerup.ToString());
                    }
                    //adding beams
                    foreach(Beam beam in theWorld.Beams.Values)
                    {
                        sb.Append(beam.ToString());
                        theWorld.Beams.Remove(beam.ID);//beam is only sent once
                    }
                }
                string frame = sb.ToString();
                //Send JSON to all clients
                lock (clients)
                {
                    foreach (SocketState client in clients.Values)
                    {
                        Networking.Send(client.TheSocket, frame);
                    }
                }
            }
        }

        /// <summary>
        /// When a new client joins
        /// </summary>
        /// <param name="client"></param>
        private void NewClient(SocketState client)
        {
            client.OnNetworkAction = ReceivePlayerName;
            Networking.GetData(client);
        }

        /// <summary>
        /// Receiving the player name from a client
        /// </summary>
        /// <param name="client"></param>
        private void ReceivePlayerName(SocketState client)
        {
            string name = client.GetData();
            if (!name.EndsWith("\n"))
            {
                client.GetData();
                return;
            }
            client.RemoveData(0, name.Length);
            name = name.Trim();
            Console.WriteLine("Accepted new connection. \n" + "Player(" + client.ID + ") " + name + " joined.");
            Networking.Send(client.TheSocket, client.ID + "\n");
            Networking.Send(client.TheSocket, startupInfo);
            //Creating a new tank with new player ID
            lock (theWorld)
            {
                theWorld.Tanks[(int)client.ID] = new Tank((int)client.ID, name, theWorld.FindTankLocation());
            }
            //Adding new client to list of clients
            lock (clients)
            {
                clients.Add((int) client.ID, client);
            }

            client.OnNetworkAction = ReceiveControlCommand;
            Networking.GetData(client);
        }

        /// <summary>
        /// Received control commands from the client
        /// </summary>
        /// <param name="client"></param>
        private void ReceiveControlCommand(SocketState client)
        {
            try
            {
                string totalData = client.GetData();
                string[] parts = Regex.Split(totalData, @"(?<=[\n])");
                foreach (string p in parts)
                {
                    if (p.Length == 0)
                        continue;

                    if (p[p.Length - 1] != '\n')
                        break;

                    JObject obj = JObject.Parse(p);
                    JToken moveToken = obj["moving"];
                    if (moveToken != null)
                    {
                        ControlCommand move = JsonConvert.DeserializeObject<ControlCommand>(p);
                        lock (theWorld)
                        {
                            theWorld.ctrlCmds[(int)client.ID] = move;//adds control command to the dictionary
                        }
                    }
                    client.RemoveData(0, p.Length);
                }
                Networking.GetData(client);
            }
            //Handles if a client disconnects
            catch 
            {
                Console.WriteLine("Client(" + client.ID + ") disconnected");
                clients.Remove((int) client.ID);
                theWorld.Tanks[(int)client.ID].SetDisconnected(true);
            }
        }
    }
}
