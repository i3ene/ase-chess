using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ase_chess.Server
{
    public class ServerSocket<T> : WebSocket, IDisposable
    {
        private readonly WebSocket _socket;
        private WebSocketServer<T>? _server;

        public WebSocketServer<T>? server
        {
            set
            {
                if (_server is not null)
                {
                    _server.SocketConnected -= OnConnected;
                    _server.SocketDisconnected -= OnDisconnected;
                    _server.SocketMessage -= OnMessage;
                    _server.SocketError -= OnError;
                }

                _server = value;

                if (_server is not null)
                {
                    _server.SocketConnected += OnConnected;
                    _server.SocketDisconnected += OnDisconnected;
                    _server.SocketMessage += OnMessage;
                    _server.SocketError += OnError;
                }
            }
            get => _server;
        }

        public override WebSocketState State => _socket.State;

        public override WebSocketCloseStatus? CloseStatus => _socket.CloseStatus;
        public override string? CloseStatusDescription => _socket.CloseStatusDescription;
        public override string? SubProtocol => _socket.SubProtocol;

        public delegate void ConnectedHandler();
        public event ConnectedHandler Connected;

        public delegate void DisconnectedHandler();
        public event DisconnectedHandler Disconnected;

        public delegate void MessageHandler(T message);
        public event MessageHandler Message;

        public delegate void ErrorHandler(Exception exception);
        public event ErrorHandler Error;

        public ServerSocket(WebSocket socket)
        {
            _socket = socket;
        }

        public ServerSocket(WebSocket socket, WebSocketServer<T>? server)
        {
            _socket = socket;
            this.server = server;
        }

        private void OnConnected(ServerSocket<T> socket)
        {
            if (socket == this) Connected?.Invoke();
        }

        private void OnDisconnected(ServerSocket<T> socket)
        {
            if (socket == this) Disconnected?.Invoke();
        }

        private void OnMessage(ServerSocket<T> socket, T message)
        {
            if (socket == this) Message?.Invoke(message);
        }

        private void OnError(ServerSocket<T> socket, Exception exception)
        {
            if (socket == this) Error?.Invoke(exception);
        }

        public override void Abort()
        {
            _socket.Abort();
        }

        public override Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken)
        {
            return _socket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken)
        {
            return _socket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override void Dispose()
        {
            _socket.Dispose();
        }

        public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return _socket.ReceiveAsync(buffer, cancellationToken);
        }

        public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            return _socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        public void Send(T message)
        {
            server?.Send(this, message);
        }
    }
}
