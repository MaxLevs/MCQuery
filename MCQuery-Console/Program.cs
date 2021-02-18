using MCQuery;
using System;

namespace MCQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            string ipAddress = "localhost";
            string portString = "25565";
            
            if (args.Length != 0)
            {
                ipAddress = args[0];
                portString = args[1];
            }

            int.TryParse(portString, out int port);

            if (port != 0)
            {
                //Connection connection = new Connection(ipAddress, port); //This should give us a challenge token needed for getting data from the sever.

                //using (Query query = new Query(ipAddress, port))
                //{

                //}

                //Query serverQuery = new Query(ipAddress, port);
				//Server basicServer = serverQuery.GetBasicServerInfo();
                //serverQuery.Close();
                
                Query query = new Query(ipAddress, port);
                Console.WriteLine(query.IsConnected);
                ServerInfo serverInfo = query.GetFullServerInfo();
                Console.WriteLine(serverInfo.Address);
                Console.WriteLine(serverInfo.GameType);
                Console.WriteLine(serverInfo.Map);
                Console.WriteLine(serverInfo.MaxPlayers);
                Console.WriteLine(serverInfo.Motd);
                Console.WriteLine(serverInfo.Port);
                Console.WriteLine(serverInfo.Version);

                
                // Rcon rconServer = new Rcon(ipAddress, port, "yolo");
                // rconServer.Login();
                // string test = rconServer.Address;
                // while (true)
                // {
                //     Console.WriteLine("Wpisz komende: ");
                //     string input = Console.ReadLine();
                //     rconServer.SendCommand(input);
                // }

                //Console.WriteLine("Printing out server info: ");
                //Console.WriteLine("Server MOTD: {0}", basicServer.Motd);
                //Console.WriteLine("Server GameType: {0}", basicServer.GameType);
                //Console.WriteLine("Server Map: {0}", basicServer.Map);
                //Console.WriteLine("Server Player Count: {0}", basicServer.PlayerCount);
                //Console.WriteLine("Server Max Players: {0}", basicServer.MaxPlayers);
                //Console.WriteLine("Server Status: {0}", basicServer.IsOnline);
            }
            
            else
            {
                Console.WriteLine("Wrong port number!");
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.Read();
        }
    }
}
