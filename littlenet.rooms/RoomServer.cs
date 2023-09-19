using littlenet.Connection.Interfaces;
using littlenet.Packets.Interfaces;
using littlenet.Server.Interfaces;
using littlenet.Server.Models;

namespace littlenet.rooms
{
    public interface IRoomServer
    {
        void OnConnectionConnected(Action<IConnection> connected);
        void OnConnectionDisconnected(Action<IConnection> disconnected);

        void AddRoom(IRoom room);
    }

    public class RoomServer : IRoomServer
    {
        private readonly ILittlenetServer _server;

        public RoomServer(ILittlenetServer server)
        {
            this._server = server;

            this._server.OnConnected(OnConnected);
        }

        private void OnConnected(IConnection connection)
        {

        }

        public void OnConnectionConnected(Action<IConnection> connected)
        {
            throw new NotImplementedException();
        }

        public void OnConnectionDisconnected(Action<IConnection> disconnected)
        {
            throw new NotImplementedException();
        }
    }
}
