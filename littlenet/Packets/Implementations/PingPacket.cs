﻿using littlenet.Packets.Interfaces;
using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Packets.Implementations
{
    public class PingPacket : IPacket
    {
        public int PacketType => 2;

        public string Identitiy { get; set; }

        public void Read(IReadableStream stream)
        {
            Identitiy = stream.ReadString();
        }

        public void Write(IWriteableStream stream)
        {
            stream.WriteString(this.Identitiy);
        }
    }
}
