using littlenet.rooms;
using littlenet.Server.Implementations;
using littlenet.shared.Packets.ToClient;
using littlenet.shared.Packets.ToServer.Login;

var server = new LittlenetTcpServer(9090);

var roomServer = new RoomServer(server);

var defaultRoom = roomServer.AddRoom(new Room("Default"));
var gameRoom = roomServer.AddRoom(new Room("Game"));
var waitingRoom = roomServer.AddRoom(new Room("Waiting"));

defaultRoom.OnUserJoinedRoom((user) =>
{
    user.Connection.OnReceived<LoginPacket>((packet) =>
    {
        bool gameFull = gameRoom.Users.Count() == 2;

        if(gameFull)
        {
            waitingRoom.JoinRoom(user);
        } else
        {
            gameRoom.JoinRoom(user);
        }
    });
});

waitingRoom.OnUserJoinedRoom((user) =>
{
    var usersInRoomPacket = new WaitingRoomPacket() { PlayerCount = waitingRoom.Users.Count() };

    foreach(var con in waitingRoom.Users)
    {
        con.Connection.Send(usersInRoomPacket);
    }
});

waitingRoom.OnUserLeftRoom((user) =>
{
    var usersInRoomPacket = new WaitingRoomPacket() { PlayerCount = waitingRoom.Users.Count() };

    foreach (var con in waitingRoom.Users)
    {
        if (con == user)
            continue;

        con.Connection.Send(usersInRoomPacket);
    }
});

gameRoom.OnUserJoinedRoom((user) =>
{
    user.Connection.Send(new WelcomeMessagePacket());
});

gameRoom.OnUserLeftRoom((user) =>
{
    bool playersInWaitingRoom = waitingRoom.Users.Count() > 0;

    if(playersInWaitingRoom)
    {
        var firstPlayers = waitingRoom.Users.First();

        gameRoom.JoinRoom(firstPlayers);
    }
});

await server.Start(new littlenet.Server.Models.LittlenetServerConfiguration());