using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Stream.Interfaces
{
    public interface IReadableStream
    {
        public int ReadInt();
        public string ReadString();
        public float ReadFloat();
        public T ReadObject<T>() where T : IStreamableObject;
        public IEnumerable<T> ReadObjects<T>() where T : IStreamableObject;
    }
}
