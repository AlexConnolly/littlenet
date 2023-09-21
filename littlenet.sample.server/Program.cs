using littlenet.Connection.Interfaces;
using littlenet.Packets.Implementations;
using littlenet.rooms;
using littlenet.sample.shared.Packets.ToClient;
using littlenet.sample.shared.Packets.ToServer;
using littlenet.Server.Implementations;

var server = new LittlenetTcpServer(9090);

var roomServer = new RoomServer(server);

var defaultRoom = roomServer.AddRoom(new Room("Default"));
var chatRoom = roomServer.AddRoom(new Room("Chat"));

defaultRoom.OnUserJoinedRoom((user) =>
{
    user.Connection.OnReceived<LoginPacket>((packet) =>
    {
        string username = packet.Username;

        if (string.IsNullOrEmpty(username))
        {
            username = "NewUser" + Random.Shared.Next(1000, 9999);
        }

        chatRoom.JoinRoom(user);
    });
});

chatRoom.OnUserJoinedRoom((user) =>
{
    user.Connection.OnReceived<SendChatMessagePacket>((chatMessage) =>
    {
        server.Broadcast(new ChatMessagePacket()
        {
            Message = chatMessage.Message,
            Sender = user.Id
        });
    });

    chatRoom.Broadcast(new ChatMessagePacket()
    {
        Message = $"User {user.Id} joined the room.",
        Sender = "Server"
    });
});

chatRoom.OnUserLeftRoom((user) =>
{
    chatRoom.Broadcast(new ChatMessagePacket()
    {
        Message = $"User {user.Id} left the room.",
        Sender = "Server"
    });
});

await server.Start(new littlenet.Server.Models.LittlenetServerConfiguration());