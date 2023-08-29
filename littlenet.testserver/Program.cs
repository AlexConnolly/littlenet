using littlenet.Connection.Interfaces;
using littlenet.Packets.Implementations;
using littlenet.Server.Implementations;

var server = new LittlenetTcpServer(9090);

server.OnConnected((connection) =>
{
    connection.OnReceived<PingPacket>((packet) =>
    {
        connection.Send(new PongPacket() { Message = "Hello world. The time is " + DateTime.UtcNow.ToString() });
    });
});

await server.Start(new littlenet.Server.Models.LittlenetServerConfiguration());