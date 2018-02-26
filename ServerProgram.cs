using System;

namespace TcpServer
{
    internal class ServerProgram
    {
        private static bool _keepRunning;
        public static Server TcpServer = new Server();

        private static void Main(string[] args)
        {
            RunApp();
        }

        private static void RunApp()
        {
            TcpServer.GetServerSettings();
            _keepRunning = true;

            while (_keepRunning)
            {
                var app = new ServerProgram();
                app.GetUserCommand();
            }
            TcpServer.StopConnection();
        }

        private void GetUserCommand()
        {
            
            Console.WriteLine("To get a list of the possible commands type 'help'. ");
            Console.Write("Enter command: ");
            var userCommand = Console.ReadLine();
            switch (userCommand)
            {
                case "start":
                {
                    TcpServer.StartConnection();
                    break;
                }
                case "stop":
                {
                    TcpServer.StopConnection();
                    break;
                }
                case "configure":
                {
                    TcpServer.GetServerSettings();
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

        private static void PrintHelp()
        {
            Console.WriteLine("You can use the following commands:");
            Console.WriteLine("'start' - start the server.");
            Console.WriteLine("'stop' - stop the server");
            Console.WriteLine("'configure' - configure the IP and port.");
            Console.WriteLine("'quit' - close the application");
        }
    }
}
