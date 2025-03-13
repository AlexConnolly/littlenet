using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Stream.Interfaces
{
    public interface IDataStream : IReadableStream, IWriteableStream
    {
        public void Close();
    }
}
