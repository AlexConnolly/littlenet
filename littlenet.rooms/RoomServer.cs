using littlenet.Connection.Interfaces;
using littlenet.Packets.Interfaces;
using littlenet.Server.Interfaces;
using littlenet.Server.Models;

namespace littlenet.rooms
{
    public interface IRoomServer
    {
        IRoom AddRoom(IRoom room);

        void CloseRoom(IRoom room);

        IEnumerable<IRoom> GetAllRooms();

        IRoom GetRoom(string name);
    }

    public class RoomServer : IRoomServer
    {
        private readonly ILittlenetServer _server;

        private List<IRoom> _rooms;

        public RoomServer(ILittlenetServer server)
        {
            this._server = server;
            this._rooms = new List<IRoom>();

            this._server.OnConnected(OnConnected);
        }

        public IRoom AddRoom(IRoom room)
        {
            _rooms.Add(room);

            return room;
        }

        public void CloseRoom(IRoom room)
        {
            var users = room.Users.ToList();
            var defaultRoom = GetRoom("Default");

            foreach(var user in users)
            {
                room.LeaveRoom(user);
                defaultRoom.JoinRoom(user);
            }
        }

        public IEnumerable<IRoom> GetAllRooms()
        {
            foreach(var room in _rooms)
            {
                yield return room;
            }
        }

        public IRoom GetRoom(string name)
        {
            return _rooms.FirstOrDefault(x => x.Configuration.Name.Equals(name));
        }

        private void OnConnected(IConnection connection)
        {
            var connectedUser = new RoomUser(connection);

            var defaultRoom = GetRoom("Default");

            defaultRoom.JoinRoom(connectedUser);
        }
    }
}
