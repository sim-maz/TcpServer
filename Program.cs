using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SimpleTCP;

namespace TcpServer
{
    class Program
    {
        private string _serverIp;
        private string _port;
        private static bool _keepRunning;
        private SimpleTcpServer _server; 

        static void Main(string[] args)
        {
            var tcpServer = new Program();
            tcpServer.GetServerSettings();
            _keepRunning = true;

            while (_keepRunning)
            {
                tcpServer.GetUserCommand();
            }
            tcpServer.StopConnection();
        }

        private void GetUserCommand()
        {
            
            Console.WriteLine("To get a list of the possible commands type 'help'. ");
            Console.WriteLine("Enter command: ");
            var userCommand = Console.ReadLine();
            switch (userCommand)
            {
                case "start":
                {
                    StartConnection();
                    break;
                }
                case "stop":
                {
                    StopConnection();
                    break;
                }
                case "configure":
                {
                    GetServerSettings();
                    break;
                }
                case "clients":
                {
                    GetConnectedClients();
                    break;
                }
                case "quit":
                {
                    _keepRunning = false;
                    break;
                }
                case "help":
                {
                    PrintHelp();
                    break;
                }
                default:
                {
                    Console.WriteLine("Command '{0}' not found. Please try again...", userCommand);
                    break;
                }
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("You can use the following commands:");
            Console.WriteLine("'start' - start the server.");
            Console.WriteLine("'stop' - stop the server");
            Console.WriteLine("'configure' - configure the IP and port.");
            Console.WriteLine("'quit' - close the application");
            GetUserCommand();
        }

        private void GetServerSettings()
        {
            Console.WriteLine("Configure your server settings.");
            Console.Write("Enter your server IP: ");
            _serverIp = Console.ReadLine();

            Console.WriteLine("Enter you server port: ");
            _port = Console.ReadLine();
        }

        private void Server_DataReceived(object sender, Message e)
        {
            string userMessage = null;
            userMessage += e.MessageString;
            Console.WriteLine(userMessage);
            e.ReplyLine($"You said: {e.MessageString}");
        }

        private void GetConnectedClients()
        {
            if (_server.IsStarted)
            {
                var listeningIps = _server.GetListeningIPs();
                Console.WriteLine("Currently connected {0} clients:", listeningIps.Count);
                foreach (var ip in listeningIps)
                {
                    Console.WriteLine(ip.MapToIPv4().ToString());
                }
            }
        }

        private void StartConnection()
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
                System.Net.IPAddress ip = System.Net.IPAddress.Parse(_serverIp);
                _server.Start(ip, Convert.ToInt32(_port));
                Console.WriteLine("Server started...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Server could not start. Please check configuration.");
            }
        }

        private void StopConnection()
        {
            if (_server.IsStarted)
                _server.Stop();
        }
    }
}
