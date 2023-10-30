using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ase_chess.Server
{
    public class WebSocketServer<T>
    {
        private HttpListener httpListener = new HttpListener();
        private Mutex signal = new Mutex();

        private List<ServerSocket<T>> sockets = new List<ServerSocket<T>>();

        public delegate void SocketConnectedHandler(ServerSocket<T> socket);
        public event SocketConnectedHandler SocketConnected;

        public delegate void SocketDisconnectedHandler(ServerSocket<T> socket);
        public event SocketDisconnectedHandler SocketDisconnected;

        public delegate void SocketMessageHandler(ServerSocket<T> socket, T message);
        public event SocketMessageHandler SocketMessage;

        public delegate void SocketErrorHandler(ServerSocket<T> socket, Exception exception);
        public event SocketErrorHandler SocketError;

        public WebSocketServer(string uri)
        {
            httpListener.Prefixes.Add(uri);
        }

        public WebSocketServer(int port)
        {
            httpListener.Prefixes.Add($"http://localhost:{port}/");
        }

        public async Task Start()
        {
            httpListener.Start();
            while (signal.WaitOne())
            {
                Listener();
            }
        }

        public void Send(WebSocket socket, T payload)
        {
            if (socket.State != WebSocketState.Open) return;
            string message = JsonSerializer.Serialize<T>(payload);
            socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void Broadcast(T message)
        {
            foreach (var socket in sockets)
            {
                Send(socket, message);
            }
        }

        private async Task Listener()
        {
            HttpListenerContext context = await httpListener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                ServerSocket<T> socket = new ServerSocket<T>(webSocketContext.WebSocket, this);
                sockets.Add(socket);
                SocketConnected?.Invoke(socket);
                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new ArraySegment<byte>(new byte[1024]);
                    WebSocketReceiveResult result;
                    var allBytes = new List<byte>();

                    do
                    {
                        result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                        for (int i = 0; i < result.Count; i++)
                        {
                            allBytes.Add(buffer.Array[i]);
                        }
                    }
                    while (!result.EndOfMessage);

                    var text = Encoding.UTF8.GetString(allBytes.ToArray(), 0, allBytes.Count);
                    try
                    {
                        var payload = JsonSerializer.Deserialize<T>(text);
                        SocketMessage?.Invoke(socket, payload);
                    }
                    catch (Exception ex)
                    {
                        SocketError?.Invoke(socket, ex);
                    }
                }
                sockets.Remove(socket);
                SocketDisconnected?.Invoke(socket);
            }
            signal.ReleaseMutex();
        }
    }
}
