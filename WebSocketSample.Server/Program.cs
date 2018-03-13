using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketSample.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSocketServer webSocketServer = new WebSocketServer();
            webSocketServer.Start("http://*:5000/WebSocket/");
            Console.WriteLine("Press any key to Exit");
            Console.ReadKey();
        }
    }
}
