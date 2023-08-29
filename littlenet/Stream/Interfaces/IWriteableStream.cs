using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Stream.Interfaces
{
    public interface IWriteableStream
    {
        public void WriteInt(int value);
        public void WriteString(string value);
    }
}
