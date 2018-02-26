using System;
using System.Net;
using System.Text;
using SimpleTCP;

namespace TcpServer
{
    internal class Server
    {
        public string ServerIp { get; set; }
        public string Port { get; set; }
        private SimpleTcpServer _server;

        public void GetServerSettings()
        {
            Console.WriteLine("Configure your server settings.");

            Console.Write("Enter your server IP: ");
            ServerIp = Console.ReadLine();
            Console.WriteLine("Server: {0}", ServerIp);

            Console.Write("Enter you server port: ");
            Port = Console.ReadLine();
            Console.WriteLine("Port: {0}", Port);
        }

        public void StartConnection()
        {
            _server = new SimpleTcpServer
            {
                Delimiter = 0x13,
                StringEncoder = Encoding.UTF8
            };
            _server.DataReceived += Server_DataReceived;

            Console.WriteLine("Server starting...");
            try
            {
                var ip = IPAddress.Parse(ServerIp);
                _server.Start(ip, Convert.ToInt32(Port));
                Console.WriteLine("Server started...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Server could not start. Please check configuration.");
            }
        }

        public void StopConnection()
        {
            if (_server.IsStarted)
                _server.Stop();
        }

        private void Server_DataReceived(object sender, Message e)
        {
            var fileExplorer = new FileExplorer(Encoding.UTF8.GetString(e.Data));
            Console.WriteLine(e.MessageString);
            Console.WriteLine(e.TcpClient.Client.ToString());
            e.Reply(fileExplorer.FilesSerialized);
        }
    }
}