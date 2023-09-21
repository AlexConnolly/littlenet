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
        public int PacketType => 1002;

        public void Read(IReadableStream stream)
        {
            throw new NotImplementedException();
        }

        public void Write(IWriteableStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
