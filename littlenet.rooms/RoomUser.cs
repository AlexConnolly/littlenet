using littlenet.Connection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.rooms
{
    public class RoomUser
    {
        private readonly IConnection _connection;

        public string Id { get; private set; }

        public IConnection Connection { get { return _connection;  } }

        public RoomUser(IConnection connection)
        {
            this.Id = Guid.NewGuid().ToString();
            this._connection = connection;
        }

        private IRoom room;
        public IRoom Room
        {
            get
            {
                return room;
            }

            internal set
            {
                room = value;
            }
        }
    }
}
