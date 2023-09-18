using littlenet.Connection.Interfaces;

namespace littlenet.rooms
{
    public interface IRoom
    {
        void OnConnectionJoinedRoom(Action<IConnection> callback);
        void OnConnectionLeftRoom(Action<IConnection> callback);

        void JoinRoom(IConnection connection);
        void LeaveRoom(IConnection connection);

        public IEnumerable<IConnection> Connections { get; }
    }

    public class Room : IRoom
    {
        private List<IConnection> _connections = new List<IConnection>();

        private List<Action<IConnection>> _leaveEvents = new List<Action<IConnection>>();
        private List<Action<IConnection>> _joinEvents = new List<Action<IConnection>>();

        public IEnumerable<littlenet.Connection.Interfaces.IConnection> Connections
        {
            get
            {
                return _connections;
            }
        }

        public void JoinRoom(IConnection connection)
        {
            _connections.Add(connection);

            ConnectionJoinedRoom(connection);
        }

        public void LeaveRoom(IConnection connection)
        {
            _connections.Remove(connection);

            ConnectionLeftRoom(connection);
        }

        private void ConnectionJoinedRoom(IConnection connection)
        {
            foreach(var joinEvent in _joinEvents)
            {
                joinEvent(connection);
            }
        }

        private void ConnectionLeftRoom(IConnection connection)
        {
            foreach(var leaveEvent in  _leaveEvents)
            {
                leaveEvent(connection);
            }
        }

        public void OnConnectionJoinedRoom(Action<IConnection> callback)
        {
            _leaveEvents.Add(callback);
        }

        public void OnConnectionLeftRoom(Action<IConnection> callback)
        {
            _joinEvents.Add(callback);
        }
    }
}