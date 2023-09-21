using littlenet.Packets.Interfaces;
using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.shared.Packets.ToServer.Login
{
    public class LoginPacket : IPacket
    {
        public int PacketType => 20001;

        public string Token { get; set; }

        public void Read(IReadableStream stream)
        {
            Token = stream.ReadString();
        }

        public void Write(IWriteableStream stream)
        {
            stream.WriteString(Token);
        }
    }
}
