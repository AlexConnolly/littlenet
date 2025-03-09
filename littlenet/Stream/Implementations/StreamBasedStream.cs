using littlenet.Stream.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Stream.Implementations
{
    public class StreamBasedStream : IDataStream
    {
        private readonly System.IO.Stream _stream;

        public StreamBasedStream(System.IO.Stream stream)
        {
            this._stream = stream;
        }

        public float ReadFloat()
        {
            byte[] buffer = new byte[4];

            _stream.Read(buffer, 0, 4);

            return BitConverter.ToSingle(buffer, 0);
        }

        public int ReadInt()
        {
            byte[] buffer = new byte[4];

            _stream.Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }

        public string ReadString()
        {
            int length = ReadInt();

            if (length <= 0)
                return "";

            byte[] buffer = new byte[length];

            _stream.Read(buffer, 0, length);

            return Encoding.UTF8.GetString(buffer);
        }

        public void WriteFloat(float value)
        {
            byte[] floatBytes = BitConverter.GetBytes(value);

            byte[] result = floatBytes;

            _stream.Write(result, 0, result.Length);
        }

        public void WriteInt(int value)
        {
            byte[] intBytes = BitConverter.GetBytes(value);

            byte[] result = intBytes;

            _stream.Write(result, 0, result.Length);
        }

        public void WriteString(string value)
        {
            WriteInt(value.Length);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);

            _stream.Write(bytes, 0, bytes.Length);
        }
    }
}
