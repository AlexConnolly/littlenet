using littlenet.Packets.Interfaces;
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

        public void Read(IReadableStream stream)
        {
            // No data to read
        }

        public void Write(IWriteableStream stream)
        {
            // No data to write
        }
    }
}
