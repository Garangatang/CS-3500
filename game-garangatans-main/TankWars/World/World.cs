using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    public class World
    {
        public int Size { get; private set; }
        int leftAndTopBorder;
        int rightAndBottomBorder;
        public Dictionary<int, Tank> Tanks;
        public Dictionary<int, Powerup> Powerups;
        public Dictionary<int, Wall> Walls;
        public Dictionary<int, Projectile> Projectiles;
        public Dictionary<int, Beam> Beams;
        public Dictionary<int, ControlCommand> ctrlCmds = new Dictionary<int, ControlCommand>();
        public bool SendBeam = false;

        //Extra Game Mode
        public static bool ExtraGameMode = false;
        public int PowerupFrameCounter = 500;

        /// <summary>
        /// Creates a new world with specified world size
        /// </summary>
        /// <param name="worldSize"></param>
        public World(int worldSize)
        {
            Tanks = new Dictionary<int, Tank>();
            Powerups = new Dictionary<int, Powerup>();
            Walls = new Dictionary<int, Wall>();
            Projectiles = new Dictionary<int, Projectile>();
            Beams = new Dictionary<int, Beam>();

            Size = worldSize;
            leftAndTopBorder = -Size / 2;
            rightAndBottomBorder = Size / 2;
        }

        /// <summary>
        /// Sets the size of the world
        /// </summary>
        /// <param name="worldSize"></param>
        public void setSize(int worldSize)
        {
            Size = worldSize;
        }

        /// <summary>
        /// Updates the world according to control commands sent by clients
        /// </summary>
        public void Update()
        {
            foreach (KeyValuePair<int, ControlCommand> ctrlCmd in ctrlCmds)
            {
                Tank tank = Tanks[ctrlCmd.Key];
                if (tank.FramesPerShotCounter > 0 && !(tank.readyToFire))
                    tank.FramesPerShotCounter--;

                if (tank.FramesPerShotCounter == 0)
                {
                    if (!tank.ProjectileSpeedFaster)
                        tank.FramesPerShotCounter = tank.GetFramesPerShot();
                    if (tank.ProjectileSpeedFaster)
                        tank.FramesPerShotCounter = 3;
                    tank.readyToFire = true;
                }

                //moving cases
                switch (ctrlCmd.Value.moving)
                {
                    case "up":
                        tank.Velocity = new Vector2D(0, -1);
                        tank.orientation = new Vector2D(0, -1);
                        break;
                    case "down":
                        tank.Velocity = new Vector2D(0, 1);
                        tank.orientation = new Vector2D(0, 1);
                        break;
                    case "left":
                        tank.Velocity = new Vector2D(-1, 0);
                        tank.orientation = new Vector2D(-1, 0);
                        break;
                    case "right":
                        tank.Velocity = new Vector2D(1, 0);
                        tank.orientation = new Vector2D(1, 0);
                        break;
                    default:
                        tank.Velocity = new Vector2D(0, 0);
                        break;
                }
                //fire (none or projectile or beam) cases
                switch (ctrlCmd.Value.fire)
                {
                    case "none":
                        break;
                    case "main":
                        if (tank.readyToFire && tank.HP != 0)
                        {
                            Projectile proj = new Projectile(tank);
                            proj.Velocity = proj.dir;
                            Projectiles.Add(proj.ID, proj);
                            proj.Velocity *= Projectile.Speed;
                            tank.readyToFire = false;
                        }
                        break;
                    case "alt":
                        if (tank.hasPowerup && tank.HP != 0)
                        {
                            Beam beam = new Beam(tank);
                            Beams.Add(beam.ID, beam);
                            tank.hasPowerup = false;
                        }
                        break;
                }
                //moves the turret
                tank.aiming = ctrlCmd.Value.tdir;
                tank.Velocity *= tank.EnginePower;
            }

            ctrlCmds.Clear();

            //Tanks
            foreach (Tank tank in Tanks.Values)
            {
                //Only necessary when extra game mode is on
                if (ExtraGameMode)
                {
                    DecrementPowerupDurations(tank);
                    CheckPowerupDurations(tank);

                    if (tank.TankSpeedFaster)
                        tank.EnginePower = 6;
                }


                if (!(tank.HP == 0))
                {

                    if (tank.Velocity.Length() == 0)
                        continue;

                    Vector2D newLoc = tank.Location + tank.Velocity;
                    bool collision = false;
                    //checking collisions
                    foreach (Wall wall in Walls.Values)
                    {
                        if (wall.CollidesTank(newLoc))
                        {
                            collision = true;
                            tank.Velocity = new Vector2D(0, 0);
                            break;
                        }
                    }
                    if (!collision)
                    {
                        tank.Location = newLoc;
                    }
                    //checking if tank leaves universe
                    OffUniverse(tank);
                }
                //If tank is dead
                else
                {
                    tank.DeathFrameCounter--;
                    if (tank.DeathFrameCounter == 0)
                    {
                        tank.HP = Tank.StartingHitPoints;
                        tank.DeathFrameCounter = Tank.RespawnDelay;
                        tank.Location = FindTankLocation();
                    }
                }
            }

            //Projectiles
            foreach (Projectile proj in Projectiles.Values)
            {
                if (proj.Velocity.Length() == 0)
                    continue;

                if (proj.died)
                {
                    Projectiles.Remove(proj.ID);
                    continue;
                }

                if (ProjOffUniverse(proj))
                    proj.died = true;

                Vector2D newLoc = proj.loc + proj.Velocity;
                bool collision = false;

                //Checking collisions for each wall
                foreach (Wall wall in Walls.Values)
                {
                    if (wall.CollidesProjectile(newLoc))
                    {
                        collision = true;
                        proj.died = true;
                        break;
                    }
                }

                //Checking collisions for each tank
                foreach (Tank tank in Tanks.Values)
                {
                    if (tank.CollidesProjectile(newLoc) && tank.ID != proj.owner)
                    {
                        collision = true;
                        proj.died = true;

                        if (!tank.Invincibility)
                            tank.HP--;

                        if (tank.HP == 0)
                        {
                            Tanks[proj.owner].score++;
                        }
                        break;
                    }
                }
                if (!collision)
                {
                    proj.loc = newLoc;
                }

            }

            //Beams
            foreach (Beam beam in Beams.Values)
            {
                //Checking collisions for each tank
                foreach (Tank tank in Tanks.Values)
                {
                    if (Intersects(beam.org, beam.dir, tank.Location, (double)tank.GetSize() / 2))
                    {
                        tank.HP = 0;
                        if (tank.HP == 0)
                        {
                            Tanks[beam.owner].score++;
                        }
                    }

                }
            }

            //Powerups
            if (Powerup.PowerupDelayCounter == 0)
            {
                Powerup power = new Powerup(FindPowerupLocation());
                if (Powerup.AmountOfPowerups < Powerup.MaxPowerups)
                {
                    Powerups.Add(power.ID, power);
                    Powerup.AmountOfPowerups++;
                }
                Powerup.PowerupDelayCounter = new Random().Next(Powerup.MaxPowerupDelay);
            }

            foreach (Powerup powerup in Powerups.Values)
            {
                if (powerup.died)
                {
                    Powerups.Remove(powerup.ID);
                    Powerup.AmountOfPowerups--;
                }

                //Checking collisions for each powerup.
                foreach (Tank tank in Tanks.Values)
                {
                    if (tank.CollidesPowerup(powerup.loc) && !powerup.died)
                    {
                        powerup.died = true;
                        if (!ExtraGameMode)
                        {
                            tank.hasPowerup = true;
                        }
                        else 
                        {
                            RandomPowerup(tank);
                        }
                    }
                }
            }
            Powerup.PowerupDelayCounter--;
        }


        /// <summary>
        /// Finds a random tank location that doesn't collide with any walls
        /// </summary>
        /// <returns></returns>
        public Vector2D FindTankLocation()
        {
            bool randomLocFound = false;
            Vector2D newLoc = new Vector2D(0, 0);

            while (!randomLocFound)
            {
                bool collision = false;
                Random rdm = new Random();
                double x = (double)rdm.Next(-Size / 2, Size / 2);
                double y = (double)rdm.Next(-Size / 2, Size / 2);
                newLoc = new Vector2D(x, y);
                foreach (Wall w in Walls.Values)
                {
                    if (w.CollidesTank(newLoc))
                        collision = true;
                }
                if (collision == false)
                    randomLocFound = true;
            }
            return newLoc;
        }

        /// <summary>
        /// Finds a random powerup location that doesn't collide with any walls
        /// </summary>
        /// <returns></returns>
        private Vector2D FindPowerupLocation()
        {
            bool randomLocFound = false;
            Vector2D newLoc = new Vector2D(0, 0);

            while (!randomLocFound)
            {
                bool collision = false;
                Random rdm = new Random();
                double x = (double)rdm.Next(-Size / 2, Size / 2);
                double y = (double)rdm.Next(-Size / 2, Size / 2);
                newLoc = new Vector2D(x, y);
                foreach (Wall w in Walls.Values)
                {
                    if (w.CollidesPowerup(newLoc))
                        collision = true;
                }
                if (collision == false)
                    randomLocFound = true;
            }
            return newLoc;
        }

        /// <summary>
        /// Chooses a random powerup when extra game mode is used
        /// </summary>
        /// <param name="tank"></param>
        private void RandomPowerup(Tank tank)
        {
            Random rdm = new Random();
            int rdmInt = rdm.Next(3);
            if (rdmInt == 0)
            {
                tank.TankSpeedFaster = true;
                tank.SpeedPowerupDuration = 300;
            }
            if (rdmInt == 1)
            {
                tank.ProjectileSpeedFaster = true;
                tank.FramesPerShotCounter = 3;
                tank.ProjectilePowerupDuration = 300;
            }
            if (rdmInt == 2)
            {
                tank.Invincibility = true;
                tank.InvincibilityDuration = 300;
            }
        }

        /// <summary>
        /// Decrements the frame counter for each powerup
        /// </summary>
        /// <param name="tank"></param>
        private void DecrementPowerupDurations(Tank tank)
        {
            if (tank.SpeedPowerupDuration > 0)
                tank.SpeedPowerupDuration--;
            if (tank.ProjectilePowerupDuration > 0)
                tank.ProjectilePowerupDuration--;
            if (tank.InvincibilityDuration > 0)
                tank.InvincibilityDuration--;
        }

        /// <summary>
        /// Checks if the powerup duration is over
        /// </summary>
        /// <param name="tank"></param>
        private void CheckPowerupDurations(Tank tank)
        {
            if (tank.SpeedPowerupDuration == 0)
            {
                tank.TankSpeedFaster = false;
                tank.EnginePower = Tank.DefaultEnginePower;
            }
            if (tank.ProjectilePowerupDuration == 0)
            {
                tank.ProjectileSpeedFaster = false;
            }
            if (tank.InvincibilityDuration == 0)
                tank.Invincibility = false;
        }

        /// <summary>
        /// Checks if a tank is off the universe and teleports it to the other side.
        /// </summary>
        /// <param name="t"></param>
        private void OffUniverse(Tank t)
        {
            Vector2D loc = t.Location;
            if (t.Location.GetY() <= leftAndTopBorder)
                t.Location = new Vector2D(loc.GetX(), -loc.GetY());
            if (t.Location.GetY() >= rightAndBottomBorder)
                t.Location = new Vector2D(loc.GetX(), -loc.GetY());
            if (t.Location.GetX() <= leftAndTopBorder)
                t.Location = new Vector2D(-loc.GetX(), loc.GetY());
            if (t.Location.GetX() >= rightAndBottomBorder)
                t.Location = new Vector2D(-loc.GetX(), loc.GetY());
        }

        /// <summary>
        /// Checks if a projectils is off universe
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool ProjOffUniverse(Projectile p)
        {
            Vector2D loc = p.loc;
            if (loc.GetY() <= leftAndTopBorder)
                return true;
            else if (loc.GetY() >= rightAndBottomBorder)
                return true;
            else if (loc.GetX() <= leftAndTopBorder)
                return true;
            else if (loc.GetX() >= rightAndBottomBorder)
                return true;
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a ray interescts a circle
        /// </summary>
        /// <param name="rayOrig">The origin of the ray</param>
        /// <param name="rayDir">The direction of the ray</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="r">The radius of the circle</param>
        /// <returns></returns>
        public static bool Intersects(Vector2D rayOrig, Vector2D rayDir, Vector2D center, double r)
        {
            // ray-circle intersection test
            // P: hit point
            // ray: P = O + tV
            // circle: (P-C)dot(P-C)-r^2 = 0
            // substituting to solve for t gives a quadratic equation:
            // a = VdotV
            // b = 2(O-C)dotV
            // c = (O-C)dot(O-C)-r^2
            // if the discriminant is negative, miss (no solution for P)
            // otherwise, if both roots are positive, hit

            double a = rayDir.Dot(rayDir);
            double b = ((rayOrig - center) * 2.0).Dot(rayDir);
            double c = (rayOrig - center).Dot(rayOrig - center) - r * r;

            // discriminant
            double disc = b * b - 4.0 * a * c;

            if (disc < 0.0)
                return false;

            // find the signs of the roots
            // technically we should also divide by 2a
            // but all we care about is the sign, not the magnitude
            double root1 = -b + Math.Sqrt(disc);
            double root2 = -b - Math.Sqrt(disc);

            return (root1 > 0.0 && root2 > 0.0);
        }
    }
}
