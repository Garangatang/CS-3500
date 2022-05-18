using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty(PropertyName = "proj")]
        public int ID { get; private set; } = 0;

        [JsonProperty(PropertyName = "loc")]
        public Vector2D loc { get; internal set; } = null;

        [JsonProperty(PropertyName = "dir")]
        public Vector2D dir { get; private set; } = null;

        [JsonProperty(PropertyName = "died")]
        public bool died { get; internal set; } = false;

        [JsonProperty(PropertyName = "owner")]
        public int owner;

        //for sounds
        public bool soundPlayed = false;

        //frames per shot of projectile
        public static int ConstFramesPerShot;
        public int FramesPerShot = 0;

        //speed of projectile
        public static double Speed;
        public Vector2D Velocity { get; internal set; }

        //projectile id counter
        private static int id = 0;

        /// <summary>
        /// Default projectile constructor
        /// </summary>
        public Projectile()
        {

        }
 
        /// <summary>
        /// Creates a projectile with reference to the tank
        /// </summary>
        /// <param name="tank"></param>
        public Projectile(Tank tank)
        {
            ID = id++;
            loc = tank.Location;
            dir = tank.aiming;
            died = false;
            owner = tank.ID;
        }

        /// <summary>
        /// Used when sending JSONs to clients.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }
    }
}
