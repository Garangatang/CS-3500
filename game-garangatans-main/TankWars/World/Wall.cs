using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        public static int Thickness = 50;
        double top, bottom, left, right;

        [JsonProperty(PropertyName = "wall")]
        public int ID { get; private set; } = 0;

        [JsonProperty(PropertyName = "p1")]
        public Vector2D p1 { get; private set; } = null;

        [JsonProperty(PropertyName = "p2")]
        public Vector2D p2 { get; private set; } = null;

        /// <summary>
        /// Default wall constructor
        /// </summary>
        public Wall()
        {

        }

        /// <summary>
        /// Wall constructor when reading from an XML
        /// </summary>
        /// <param name="wallID"></param>
        /// <param name="p1C"></param>
        /// <param name="p2C"></param>
        public Wall(int wallID, Vector2D p1C, Vector2D p2C)
        {
            ID = wallID;
            p1 = p1C;
            p2 = p2C;

            left = Math.Min(p1.GetX(), p2.GetX());
            right = Math.Max(p1.GetX(), p2.GetX());
            top = Math.Min(p1.GetY(), p2.GetY());
            bottom = Math.Max(p1.GetY(), p2.GetY());

        }

        /// <summary>
        /// Used in sending wall JSONs to clients
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this) + "\n";
        }

        /// <summary>
        /// Checks if a wall and a tank collide
        /// </summary>
        /// <param name="tankLoc"></param>
        /// <returns></returns>
        public bool CollidesTank(Vector2D tankLoc)
        {
            double expansion = Thickness / 2 + Tank.Size / 2;
            return left - expansion < tankLoc.GetX() 
                && tankLoc.GetX() < right + expansion
                && top - expansion < tankLoc.GetY() 
                && tankLoc.GetY() < bottom + expansion;
        }

        /// <summary>
        /// CHecks if a wall and projectile collide
        /// </summary>
        /// <param name="projLoc"></param>
        /// <returns></returns>
        public bool CollidesProjectile(Vector2D projLoc)
        {
            double expansion = Thickness / 2;
            return left - expansion < projLoc.GetX()
                && projLoc.GetX() < right + expansion
                && top - expansion < projLoc.GetY()
                && projLoc.GetY() < bottom + expansion;
        }

        /// <summary>
        /// CHecks if a wall and powerup collide
        /// </summary>
        /// <param name="projLoc"></param>
        /// <returns></returns>
        public bool CollidesPowerup(Vector2D projLoc)
        {
            double expansion = Thickness / 2 + 5;
            return left - expansion < projLoc.GetX()
                && projLoc.GetX() < right + expansion
                && top - expansion < projLoc.GetY()
                && projLoc.GetY() < bottom + expansion;
        }

        /// <summary>
        /// Helper method to print walls.
        /// </summary>
        public void printWall()
        {
            Console.WriteLine("Wall ID: " + ID + ", first vector: " + p1.ToString() + ", second vector: " + p2.ToString());
        }

    }
}
