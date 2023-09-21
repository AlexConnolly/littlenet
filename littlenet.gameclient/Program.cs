
using littlenet.Connection.Implementations;
using littlenet.sample.shared.Packets.ToClient;
using littlenet.sample.shared.Packets.ToServer;
using System;
using System.Threading;

using var game = new littlenet.gameclient.Game1();

var thread = new Thread(() =>
{
    System.Threading.Thread.Sleep(2000);

    var connection = StandardConnection.Connect("127.0.0.1", 9090);

    connection.OnReceived<ChatMessagePacket>((packet) =>
    {
        game.ClearColor = new Microsoft.Xna.Framework.Color(Random.Shared.Next(1, 255), Random.Shared.Next(1, 255), Random.Shared.Next(1, 255));
    });

    connection.Send(new LoginPacket() { Username = System.Environment.MachineName });

    while(true)
    {

    }
});

thread.Start();

game.Run();


