using littlenet.Connection.Implementations;
using littlenet.Connection.Interfaces;
using littlenet.Packets.Interfaces;
using System.Diagnostics;
using System.Net.WebSockets;

namespace littlenet.rooms
{
    public interface IRoom
    {
        void OnUserJoinedRoom(Action<RoomUser> callback);
        void OnUserLeftRoom(Action<RoomUser> callback);

        void JoinRoom(RoomUser connection);
        void LeaveRoom(RoomUser connection);

        public IEnumerable<RoomUser> Users { get; }

        public RoomConfiguration Configuration { get; }

        public void Broadcast(IPacket packet);
    }

    public class Room : IRoom
    {
        private class MappedPacket
        {
            public Type Packet { get; set; }
            public List<Action<IPacket, RoomUser>> Callbacks { get; set; }
        }

        private List<RoomUser> _users = new List<RoomUser>();

        private List<Action<RoomUser>> _leaveEvents = new List<Action<RoomUser>>();
        private List<Action<RoomUser>> _joinEvents = new List<Action<RoomUser>>();

        public IEnumerable<RoomUser> Users
        {
            get
            {
                return _users;
            }
        }

        public RoomConfiguration Configuration { get; private set; }

        public Room(string name)
        {
            this.Configuration = new RoomConfiguration() { Name = name };
        }

        public void JoinRoom(RoomUser user)
        {
            _users.Add(user);

            if(user.Room != null)
            {
                // Leave current room
                user.Room.LeaveRoom(user);
            }

            user.Connection.OnDisconnected(() =>
            {
                LeaveRoom(user);
            });

            UserJoinedRoom(user);
        }

        public void LeaveRoom(RoomUser user)
        {
            _users.Remove(user);

            user.Room = null;

            // Unbinding any packet bindigns that were already there
            user.Connection.ClearPacketBindings();

            UserLeftRoom(user);
        }

        private void UserJoinedRoom(RoomUser user)
        {
            foreach(var joinEvent in _joinEvents)
            {
                joinEvent(user);
            }
        }

        private void UserLeftRoom(RoomUser user)
        {
            foreach(var leaveEvent in  _leaveEvents)
            {
                leaveEvent(user);
            }
        }

        public void OnUserJoinedRoom(Action<RoomUser> user)
        {
            _joinEvents.Add(user);
        }

        public void OnUserLeftRoom(Action<RoomUser> user)
        {
            _leaveEvents.Add(user);
        }

        private int GetPacketTypeFor(Type type)
        {
            if (!typeof(IPacket).IsAssignableFrom(type))
                throw new ArgumentException("Type must be an IPacket");

            IPacket instance = (IPacket)Activator.CreateInstance(type);

            return instance.PacketType;
        }


        private Dictionary<int, MappedPacket> _packets = new Dictionary<int, MappedPacket>();

        public IRoom OnPacketReceived<T>(Action<T, RoomUser> callback) where T : IPacket
        {
            Type typeOfT = typeof(T);

            int packetTypeId = GetPacketTypeFor(typeOfT);

            if (!_packets.TryGetValue(packetTypeId, out MappedPacket mappedPacket))
            {
                mappedPacket = new MappedPacket
                {
                    Packet = typeOfT,
                    Callbacks = new List<Action<IPacket, RoomUser>>()
                };

                _packets[packetTypeId] = mappedPacket;
            }

            // Cast the Action<T> to Action<IPacket> and add to the callback list
            Action<IPacket, RoomUser> generalCallback = (IPacket packet, RoomUser user) =>
            {
                if(user.Room == this)
                    callback((T)packet, user);
                {
                    // Todo: Cleanup this so we remove the binding and not have memory leaks
                }
            };

            mappedPacket.Callbacks.Add(generalCallback);

            return this;
        }

        public void Broadcast(IPacket packet)
        {
            foreach(var user in _users)
            {
                user.Connection.Send(packet);
            }
        }
    }
}