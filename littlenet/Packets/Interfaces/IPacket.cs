using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Packets.Interfaces
{
    public interface IPacket
    {
        public int PacketType { get; }

        public void Write(IWriteableStream stream);
        public void Read(IReadableStream stream);
    }
}
