using littlenet.Connection.Interfaces;
using littlenet.Packets.Interfaces;
using littlenet.Stream.Implementations;
using littlenet.Stream.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Connection.Implementations
{

    public class StandardConnection : IConnection
    {
        private class MappedPacket
        {
            public Type Packet { get; set; }
            public List<Action<IPacket>> Callbacks { get; set; }
        }

        private IDataStream _dataStream;
        private Thread _readthread;
        private Thread _writeThread;

        private Dictionary<int, MappedPacket> _packets = new Dictionary<int, MappedPacket>();
        private Action _unsupportedPacketCallback;

        private string connectionId = Guid.NewGuid().ToString();
        public string ConnectionId => connectionId;

        private bool read = false;

        // Packets to be written
        private BlockingCollection<IPacket> _writeQueue = new BlockingCollection<IPacket>();

        public static StandardConnection Connect(string ipAddress, int port)
        {
            var tcpClient = new TcpClient();

            tcpClient.Connect(ipAddress, port);

            return new StandardConnection(new StreamBasedStream(tcpClient.GetStream()));
        }

        public StandardConnection(IDataStream dataStream)
        {
            this._dataStream = dataStream;

            this._writeThread = new Thread(() =>
            {
                while(read)
                {
                    try
                    {
                        foreach (var packet in _writeQueue.GetConsumingEnumerable()) // Blocks when empty
                        {
                            this._dataStream.WriteInt(packet.PacketType);
                            packet.Write(this._dataStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        foreach (var disconnect in _onDisconnectedEvents)
                        {
                            disconnect();
                        }
                    }
                }
            });

            this._readthread = new Thread(() =>
            {
                while(read)
                {
                    try
                    {

                        int packetId = this._dataStream.ReadInt();

                        Console.WriteLine("Received packet ID " + packetId);

                        if (_packets.ContainsKey(packetId))
                        {
                            IPacket instance = (IPacket)Activator.CreateInstance(_packets[packetId].Packet);

                            instance.Read(dataStream);

                            foreach (var callback in _packets[packetId].Callbacks)
                            {
                                callback(instance);
                            }
                        }
                        else
                        {
                            if (_unsupportedPacketCallback != null)
                                _unsupportedPacketCallback();
                        }
                    } catch (Exception ex)
                    {
                        // Disconnected
                        foreach(var disconnect in _onDisconnectedEvents)
                        {
                            disconnect();
                        }

                        read = false;
                    }
                }
            });

            read = true;

            this._writeThread.Start();
        }

        public void OnReceived<T>(Action<T> callback) where T : IPacket
        {
            Type typeOfT = typeof(T);

            int packetTypeId = GetPacketTypeFor(typeOfT);

            if (!_packets.TryGetValue(packetTypeId, out MappedPacket mappedPacket))
            {
                mappedPacket = new MappedPacket
                {
                    Packet = typeOfT,
                    Callbacks = new List<Action<IPacket>>()
                };

                _packets[packetTypeId] = mappedPacket;
            }

            // Cast the Action<T> to Action<IPacket> and add to the callback list
            Action<IPacket> generalCallback = packet => callback((T)packet);

            mappedPacket.Callbacks.Add(generalCallback);

            if(this._readthread.ThreadState != ThreadState.Running)
            {
                // Only start reading when we have bound a packet
                this._readthread.Start();
            }
        }

        private int GetPacketTypeFor(Type type)
        {
            if (!typeof(IPacket).IsAssignableFrom(type))
                throw new ArgumentException("Type must be an IPacket");

            IPacket instance = (IPacket)Activator.CreateInstance(type);

            return instance.PacketType;
        }

        public void Send(IPacket packet)
        {
            // Cheesy but yeah you can't send a packet if you aren't connected
            if(read)
                _writeQueue.Add(packet);
        }

        public void OnUnsupportedPacket(Action callback)
        {
            this._unsupportedPacketCallback = callback;
        }

        public void ClearPacketBindings()
        {
            this._packets.Clear();
        }

        private List<Action> _onDisconnectedEvents = new List<Action>();

        public void OnDisconnected(Action callback)
        {
            _onDisconnectedEvents.Add(callback);
        }

        public void Disconnect()
        {
            read = false;

            _dataStream.Close();

            _writeQueue.CompleteAdding();

            foreach (var disconnect in _onDisconnectedEvents)
            {
                disconnect();
            }
        }
    }
}
