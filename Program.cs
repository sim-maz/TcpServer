using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTCP;

namespace TcpServer
{
    class Program
    {
        private string _serverIp;
        private string _port;
        private SimpleTcpServer _server;

        static void Main(string[] args)
        {
            var tcpServer = new Program();
            tcpServer.GetServerSettings();

        }

        private void GetServerSettings()
        {
            _server = new SimpleTcpServer();
            _server.Delimiter = 0x13;
            _server.DataReceived += Server_DataReceived;

            Console.WriteLine("Configure your server settings.");
            Console.Write("Enter your server IP: ");
            _serverIp = Console.ReadLine();

            Console.WriteLine("Enter you server port: ");
            _port = Console.ReadLine();

            Console.WriteLine("Your server: {0}", _serverIp);
            Console.Read();
        }

        private void Server_DataReceived(object sender, Message e)
        {
            _serverIp += e.MessageString;
            e.ReplyLine($"Server set to: {e.MessageString}");
        }

        private void StartConnection()
        {
            System.Net.IPAddress ip = new System.Net.IPAddress(long.Parse(_serverIp));
            _server.Start(ip, Convert.ToInt32(_port));
        }

        private void StopConnection()
        {
            if (_server.IsStarted)
                _server.Stop();
        }
    }
}
