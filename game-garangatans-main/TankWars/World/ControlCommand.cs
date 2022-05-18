using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ControlCommand
    {
        [JsonProperty(PropertyName = "moving")]
        public string moving { get; set; }

        [JsonProperty(PropertyName = "fire")]
        public string fire { get; set; }

        [JsonProperty(PropertyName = "tdir")]
        public Vector2D tdir { get; set; }

        /// <summary>
        /// Default control command
        /// </summary>
        /// <param name="m">moving</param>
        /// <param name="f">fire</param>
        /// <param name="t">turret direction</param>
        public ControlCommand()
        {

        }

        /// <summary>
        /// Creates a control command with specified moving, fire, and turret direction parameters
        /// </summary>
        /// <param name="m">moving</param>
        /// <param name="f">fire</param>
        /// <param name="t">turret direction</param>
        public ControlCommand(string m, string f, Vector2D t)
        {
            moving = m;
            fire = f;
            tdir = t;
        }

    }
}
