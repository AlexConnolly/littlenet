﻿using littlenet.Packets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Connection.Interfaces
{
    public interface IConnection
    {
        public void Send(IPacket packet);
        public void OnReceived<T>(Action<T> callback) where T : IPacket;

        public void OnUnsupportedPacket(Action callback);

        public string ConnectionId { get; }

        public void ClearPacketBindings();

        public void OnDisconnected(Action callback);
        public void Disconnect();
    }
}
