
using littlenet.Connection.Implementations;
using littlenet.shared.Packets.ToClient;
using littlenet.shared.Packets.ToServer.Login;
using System;
using System.Threading;

using var game = new littlenet.gameclient.Game1();

var thread = new Thread(() =>
{
    System.Threading.Thread.Sleep(2000);

    var connection = StandardConnection.Connect("127.0.0.1", 9090);

    connection.OnReceived<WelcomeMessagePacket>((packet) =>
    {
        game.ClearColor = new Microsoft.Xna.Framework.Color(Random.Shared.Next(1, 255), Random.Shared.Next(1, 255), Random.Shared.Next(1, 255));
    });

    connection.OnReceived<WaitingRoomPacket>((packet) =>
    {
        // We are in the waiting room, display the amount of users waiting 
         
    });

    connection.Send(new LoginPacket() { Token = "" });

    while(true)
    {

    }
});

thread.Start();

game.Run();


