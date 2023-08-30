using littlenet.Connection.Implementations;
using littlenet.Packets.Implementations;
using littlenet.sample.shared.Packets.ToClient;
using littlenet.sample.shared.Packets.ToServer;

System.Threading.Thread.Sleep(2000);

var connection = StandardConnection.Connect("127.0.0.1", 9090);

connection.OnReceived<ChatMessagePacket>((packet) =>
{
    Console.Write(packet.Message);
});

connection.Send(new LoginPacket() { Username = System.Environment.MachineName });

Console.ReadLine();