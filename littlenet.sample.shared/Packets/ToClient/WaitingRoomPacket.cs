using littlenet.Packets.Interfaces;
using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.shared.Packets.ToClient
{
    public class WaitingRoomPacket : IPacket
    {
        public int PacketType => 10002;

        public int PlayerCount { get; set; }

        public void Read(IReadableStream stream)
        {
            PlayerCount = stream.ReadInt();
        }

        public void Write(IWriteableStream stream)
        {
            stream.WriteInt(PlayerCount);
        }
    }
}
