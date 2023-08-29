using littlenet.Connection.Implementations;
using littlenet.Connection.Interfaces;
using littlenet.Server.Interfaces;
using littlenet.Server.Models;
using littlenet.Stream.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Server.Implementations
{

    public class LittlenetTcpServer : ILittlenetServer
    {
        private readonly int _port;

        private List<Action<IConnection>> _connectionListeners = new List<Action<IConnection>>();

        private CancellationTokenSource _stopToken = new CancellationTokenSource();

        public LittlenetTcpServer(int port)
        {
            this._port = port;
        }

        public void OnConnected(Action<IConnection> callback)
        {
            this._connectionListeners.Add(callback);
        }

        public async Task Start(LittlenetServerConfiguration serverConfiguration)
        {
            var listener = new TcpListener(IPAddress.Any, this._port);

            listener.Start();

            while (!_stopToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync();

                var stream = client.GetStream();

                var connection = new StandardConnection(new StreamBasedStream(stream)); 

                foreach(var connectionCallback in this._connectionListeners)
                {
                    connectionCallback(connection);
                }
            }
        }

        public async Task Stop()
        {
            this._stopToken.Cancel();
        }
    }
}
