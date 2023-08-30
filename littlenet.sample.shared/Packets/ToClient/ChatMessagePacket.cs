using littlenet.Packets.Interfaces;
using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.sample.shared.Packets.ToClient
{
    public class ChatMessagePacket : IPacket
    {
        public int PacketType => 202;

        public string Message { get; set; }

        public string Sender { get; set; }

        public void Read(IReadableStream stream)
        {
            Message = stream.ReadString();
            Sender = stream.ReadString();
        }

        public void Write(IWriteableStream stream)
        {
            stream.WriteString(this.Message);
            stream.WriteString(this.Sender);
        }
    }
}
