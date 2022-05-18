using System;
using Newtonsoft.Json;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        [JsonProperty(PropertyName = "tank")]
        public int ID { get; private set; }

        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { get; internal set; }

        [JsonProperty(PropertyName = "bdir")]
        public Vector2D orientation { get; internal set; }

        [JsonProperty(PropertyName = "tdir")]
        public Vector2D aiming { get; internal set; } = new Vector2D(0, -1);

        [JsonProperty(PropertyName = "name")]
        public string name { get; private set; }

        [JsonProperty(PropertyName = "hp")]
        public int HP { get; internal set; }

        [JsonProperty(PropertyName = "score")]
        public int score { get; internal set; } = 0;

        [JsonProperty(PropertyName = "died")]
        public bool died { get; private set; }

        [JsonProperty(PropertyName = "dc")]
        public bool disconnected { get; private set; } = false;

        [JsonProperty(PropertyName = "join")]
        public bool joined { get; private set; } = false;

        public Vector2D Velocity { get; internal set; }


        //Frames Per Shot
        public static int FramesPerShot;
        public int FramesPerShotCounter = FramesPerShot;

        //Boolean if tank is ready to fire projectiles
        public bool readyToFire = false;

        //Starting HP
        public static int StartingHitPoints;
        private int MaxHP = StartingHitPoints;

        //Speed of Tank
        public static double DefaultEnginePower;
        public double EnginePower = DefaultEnginePower;

        //Size of Tank
        public static int Size;

        //Used for determining collisions
        double top, bottom, left, right;

        //Used to determine if tank has a powerup
        internal bool hasPowerup = false;

        //RespawnDelay
        public static int RespawnDelay;
        public int DeathFrameCounter = RespawnDelay;

        //Used for extra game mode
        public bool TankSpeedFaster = false;
        public bool ProjectileSpeedFaster = false;
        public bool Invincibility = false;

        //Counts the powerup frames for extra game mode
        public int SpeedPowerupDuration = 0;
        public int ProjectilePowerupDuration = 0;
        public int InvincibilityDuration = 0;

        /// <summary>
        /// Default tank constructor
        /// </summary>
        public Tank()
        {
           
        }

        /// <summary>
        /// Creates a new tank
        /// </summary>
        /// <param name="id">tank id</param>
        /// <param name="name">player name</param>
        /// <param name="location">starting location of tank</param>
        public Tank(int id, string name, Vector2D location)
        {
            ID = id;
            Location = location;
            orientation = new Vector2D(0, -1);
            aiming = orientation;
            this.name = name;
            HP = MaxHP;
            score = 0;
            died = false;
            disconnected = false;
            joined = true;
            Velocity = new Vector2D(0, 0);
        }

        /// <summary>
        /// Converts to JSON of tank
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }


        /// <summary>
        /// CHecks if a tank and projectile collide
        /// </summary>
        /// <param name="projLoc"></param>
        /// <returns></returns>
        public bool CollidesProjectile(Vector2D projLoc)
        {
            if (this.HP != 0)
            {
                double expansion = Size / 2;
                left = Location.GetX() - expansion;
                right = Location.GetX() + expansion;
                top = Location.GetY() - expansion;
                bottom = Location.GetY() + expansion;

                return left < projLoc.GetX()
                    && projLoc.GetX() < right
                    && top < projLoc.GetY()
                    && projLoc.GetY() < bottom;
            }
            else
                return false;
        }

        /// <summary>
        /// Checks if a tank and powerup collide
        /// </summary>
        /// <param name="powerUpLoc"></param>
        /// <returns></returns>
        public bool CollidesPowerup(Vector2D powerUpLoc)
        {
            double expansion = Size / 2;
            left = Location.GetX() - expansion;
            right = Location.GetX() + expansion;
            top = Location.GetY() - expansion;
            bottom = Location.GetY() + expansion;

            return left < powerUpLoc.GetX()
                && powerUpLoc.GetX() < right
                && top < powerUpLoc.GetY()
                && powerUpLoc.GetY() < bottom;
        }

        /// <summary>
        /// Returns size of tank
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return Size;
        }

        /// <summary>
        /// Returns frames per shot of tank
        /// </summary>
        /// <returns></returns>
        public int GetFramesPerShot()
        {
            return FramesPerShot;
        }

        /// <summary>
        /// Sets disconnected to true if tank is disconnected
        /// </summary>
        /// <param name="b"></param>
        public void SetDisconnected(bool b)
        {
            disconnected = b;
        }
    }
}
