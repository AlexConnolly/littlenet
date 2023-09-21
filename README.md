# littlenet
A small .net packet-based networking solution.

```
var server = new LittlenetTcpServer(9090);

var roomServer = new RoomServer(server);

roomServer.AddRoom(new Room("Default"));
roomServer.AddRoom(new Room("Chat"));

roomServer.GetRoom("Default").OnUserJoinedRoom((user) =>
{
    user.Connection.OnReceived<LoginPacket>((packet) =>
    {
        roomServer.GetRoom("Chat").JoinRoom(user);
    });
});

roomServer.GetRoom("Chat").OnUserJoinedRoom((user) =>
{
    user.Connection.OnReceived<SendChatMessagePacket>((chatMessage) =>
    {
        server.Broadcast(new ChatMessagePacket()
        {
            Message = chatMessage.Message,
            Sender = user.Id
        });
    });

    var room = roomServer.GetRoom("Chat");

    room.Broadcast(new ChatMessagePacket()
    {
        Message = $"User {user.Id} joined the room.",
        Sender = "Server"
    });
});

roomServer.GetRoom("Chat").OnUserLeftRoom((user) =>
{
    var room = roomServer.GetRoom("Chat");

    room.Broadcast(new ChatMessagePacket()
    {
        Message = $"User {user.Id} left the room.",
        Sender = "Server"
    });
});

await server.Start(new littlenet.Server.Models.LittlenetServerConfiguration());
```
