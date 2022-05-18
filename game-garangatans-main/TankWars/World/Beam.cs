using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Beam
    {
        public int frameCount = 90;
        public bool beamDictEmpty = false;

        [JsonProperty(PropertyName = "beam")]
        public int ID { get; private set; } = 0;

        [JsonProperty(PropertyName = "org")]
        public Vector2D org { get; private set; } = null;

        [JsonProperty(PropertyName = "dir")]
        public Vector2D dir { get; private set; } = null;

        [JsonProperty(PropertyName = "owner")]
        public int owner;

        //ID counter for beams
        private static int id = 0;
        /// <summary>
        /// Default beam
        /// </summary>
        public Beam()
        {

        }

        /// <summary>
        /// Creates a beam with reference to specified tank
        /// </summary>
        /// <param name="tank"></param>
        public Beam(Tank tank)
        {
            ID = id++;
            org = tank.Location;
            dir = tank.aiming;
            owner = tank.ID;
        }

        /// <summary>
        /// Converts a beam to a JSON string used to send to clients
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }

    }
}
