using littlenet.Packets.Interfaces;
using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Packets.Implementations
{
    public class PongPacket : IPacket
    {
        public int PacketType => 2;

        public string Message { get; set; }

        public void Read(IReadableStream stream)
        {
            Message = stream.ReadString();
        }

        public void Write(IWriteableStream stream)
        {
            stream.WriteString(Message);
        }
    }
}
