using littlenet.Packets.Interfaces;
using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.shared.Packets.ToClient
{
    public class WelcomeMessagePacket : IPacket
    {
        public int PacketType => 10001;

        public void Read(IReadableStream stream)
        {

        }

        public void Write(IWriteableStream stream)
        {

        }
    }
}
