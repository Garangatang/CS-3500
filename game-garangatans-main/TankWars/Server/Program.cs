using System;
using System.Collections.Generic;

namespace TankWars
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings settings = new Settings(@"../../../../Resources/settingsWithBasicData.xml");//retrieves settings from a XML file
            ServerController serverController = new ServerController(settings);//creates a server controller with settings
            serverController.Start();//starts the server thread

            Console.Read();
        }
    }
}
