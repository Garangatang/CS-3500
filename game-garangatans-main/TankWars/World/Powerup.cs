using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Powerup
    {
        [JsonProperty(PropertyName = "power")]
        public int ID { get; private set; } = 0;

        [JsonProperty(PropertyName = "loc")]
        public Vector2D loc { get; private set; } = null;

        [JsonProperty(PropertyName = "died")]
        public bool died { get; internal set; } = false;

        //Max Powerups in World
        public static int MaxPowerups = 2;
        //Delay between spawning Powerups
        public static int MaxPowerupDelay = 1650;
        //Powerup ID's
        private static int id = 0;
        //Amount of powerups in world
        public static int AmountOfPowerups = 0;
        //Counts delay
        public static int PowerupDelayCounter;
        public Powerup()
        {
            ID = id++;
            loc = new Vector2D(50, 0);
            died = false;
            PowerupDelayCounter = new Random().Next(MaxPowerupDelay);
        }

        /// <summary>
        /// Creates a new powerup given a location
        /// </summary>
        /// <param name="location"></param>
        public Powerup(Vector2D location)
        {
            ID = id++;
            loc = location;
            died = false;
            PowerupDelayCounter = new Random().Next(MaxPowerupDelay);
        }

        /// <summary>
        /// Converts a powerup to a JSON string used to send to clients
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }

    }
}
