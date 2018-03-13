using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketSample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Connect("ws://localhost:5000/WebSocket/").Wait();
            Console.WriteLine("Press any key to Exit...");
            Console.ReadKey();
        }
        public static async Task Connect(string uri)
        {
            await Task.Delay(3000); //Wait until server application start
            ClientWebSocket webSocket = null;
            try
            {
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(new Uri(uri),CancellationToken.None);
                await Task.WhenAll(Receive(webSocket),Send(webSocket));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: "+ex);
            }
            finally
            {
                webSocket?.Dispose();
                Console.WriteLine("\nWebSocket closed.");
            }
        }

        private static async Task Send(ClientWebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                Console.WriteLine("Type something to send server:");
                string stringToSend = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(stringToSend);

                await webSocket.SendAsync(new ArraySegment<byte>(buffer),WebSocketMessageType.Binary,false,CancellationToken.None);
                Console.WriteLine("Sent: "+stringToSend);

                await Task.Delay(1000);
            }

        }

        private static async Task Receive(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnected Message !!", CancellationToken.None);
                }
                else
                {
                    Console.WriteLine("Received: "+Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
                }
            }
        }
    }
}
