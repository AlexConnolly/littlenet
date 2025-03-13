using littlenet.Stream.Interfaces;
using System.Text;

namespace littlenet.Stream.Implementations
{
    public class StreamBasedStream : IDataStream
    {
        private readonly System.IO.Stream _stream;

        public StreamBasedStream(System.IO.Stream stream)
        {
            this._stream = stream;
        }

        public void Close()
        {
            _stream.Close();
        }

        public float ReadFloat()
        {
            byte[] buffer = new byte[4];

            _stream.Read(buffer, 0, 4);

            if(BitConverter.IsLittleEndian)
            {
                return BitConverter.ToSingle(buffer.Reverse().ToArray(), 0);
            }

            return BitConverter.ToSingle(buffer, 0);
        }

        public int ReadInt()
        {
            byte[] buffer = new byte[4];

            if(BitConverter.IsLittleEndian)
            {
                _stream.Read(buffer, 0, 4);

                return BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);
            }

            _stream.Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }

        public T ReadObject<T>() where T : IStreamableObject
        {
            T obj = Activator.CreateInstance<T>();

            obj.ReadFromStream(this);

            return obj;
        }

        public IEnumerable<T> ReadObjects<T>() where T : IStreamableObject
        {
            int amount = ReadInt();

            List<T> result = new List<T>();

            for (int i = 0; i < amount; i++)
            {
                T obj = ReadObject<T>();

                result.Add(obj);
            }

            return result;
        }

public string ReadString()
{
    int length = ReadInt();
    if (length <= 0)
        return "";

    byte[] buffer = new byte[length];
    int totalRead = 0;

    while (totalRead < length)
    {
        int bytesRead = _stream.Read(buffer, totalRead, length - totalRead);
        if (bytesRead == 0)
            throw new IOException("Stream closed before reading enough data.");
        
        totalRead += bytesRead;
    }

    return Encoding.UTF8.GetString(buffer);
}

        public void WriteFloat(float value)
        {
            if(BitConverter.IsLittleEndian)
            {
                value = BitConverter.ToSingle(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
            }

            byte[] floatBytes = BitConverter.GetBytes(value);

            byte[] result = floatBytes;

            _stream.Write(result, 0, result.Length);
        }

        public void WriteInt(int value)
        {
            // Do we need to check for endianness here?
            if(BitConverter.IsLittleEndian)
            {
                value = System.Net.IPAddress.HostToNetworkOrder(value);
            }

            byte[] intBytes = BitConverter.GetBytes(value);

            byte[] result = intBytes;

            _stream.Write(result, 0, result.Length);
        }

        public void WriteObject<T>(T obj) where T : IStreamableObject
        {
            obj.WriteToStream(this);
        }

        public void WriteObjects<T>(IEnumerable<T> objs) where T : IStreamableObject
        {
            var objts = objs.ToList();

            WriteInt(objts.Count);

            foreach(var obj in objts)
            {
                WriteObject(obj);
            }
        }

        public void WriteString(string value)
        {
            WriteInt(value.Length);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);

            _stream.Write(bytes, 0, bytes.Length);
        }
    }
}
