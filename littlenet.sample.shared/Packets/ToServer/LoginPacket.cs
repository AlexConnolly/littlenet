using littlenet.Packets.Interfaces;
using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.shared.Packets.ToServer
{
    public class LoginPacket : IPacket
    {
        public int PacketType => 101;

        public string Username { get; set; }

        public void Read(IReadableStream stream)
        {
            Username = stream.ReadString();
        }

        public void Write(IWriteableStream stream)
        {
            stream.WriteString(this.Username);
        }
    }
}
