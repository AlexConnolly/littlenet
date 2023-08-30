using littlenet.Connection.Interfaces;
using littlenet.Packets.Interfaces;
using littlenet.Stream.Implementations;
using littlenet.Stream.Interfaces;
using System;
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

        private Dictionary<int, MappedPacket> _packets = new Dictionary<int, MappedPacket>();

        public static StandardConnection Connect(string ipAddress, int port)
        {
            var tcpClient = new TcpClient();

            tcpClient.Connect(ipAddress, port);

            return new StandardConnection(new StreamBasedStream(tcpClient.GetStream()));
        }

        public StandardConnection(IDataStream dataStream)
        {
            this._dataStream = dataStream;

            this._readthread = new Thread(() =>
            {
                while(true)
                {

                    int packetId = this._dataStream.ReadInt();

                    if (_packets.ContainsKey(packetId))
                    {
                        IPacket instance = (IPacket)Activator.CreateInstance(_packets[packetId].Packet);

                        instance.Read(dataStream);

                        foreach (var callback in _packets[packetId].Callbacks)
                        {
                            callback(instance);
                        }
                    }
                }
            });
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
            this._dataStream.WriteInt(packet.PacketType);
            packet.Write(this._dataStream);
        }
    }
}
