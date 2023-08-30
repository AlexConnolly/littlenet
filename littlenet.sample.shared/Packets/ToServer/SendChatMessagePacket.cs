using littlenet.Packets.Interfaces;
using littlenet.Stream.Interfaces;

namespace littlenet.sample.shared.Packets.ToServer
{
    public class SendChatMessagePacket : IPacket
    {
        public int PacketType => 201;

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
