using littlenet.Connection.Interfaces;
using littlenet.Packets.Implementations;
using littlenet.sample.shared.Packets.ToClient;
using littlenet.sample.shared.Packets.ToServer;
using littlenet.Server.Implementations;

var server = new LittlenetTcpServer(9090);

server.OnConnected((connection) =>
{
    connection.OnReceived<LoginPacket>((packet) =>
    {
        string username = packet.Username;

        if(string.IsNullOrEmpty(username))
        {
            username = "NewUser" + Random.Shared.Next(1000, 9999);
        }

        connection.OnReceived<SendChatMessagePacket>((chatMessage) =>
        {
            server.Broadcast(new ChatMessagePacket()
            {
                Message = chatMessage.Message,
                Sender = username
            });
        });

        server.Broadcast(new ChatMessagePacket()
        {
            Message = "User " + username + " has joined the chat!",
            Sender = ""
        });
    });
});

await server.Start(new littlenet.Server.Models.LittlenetServerConfiguration());