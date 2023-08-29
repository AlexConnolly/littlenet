using littlenet.Connection.Implementations;
using littlenet.Packets.Implementations;

System.Threading.Thread.Sleep(2000);

var connection = StandardConnection.Connect("127.0.0.1", 9090);

connection.OnReceived<PongPacket>((packet) =>
{
    Console.Write(packet.Message);
});

connection.Send(new PingPacket());

Console.ReadLine();