using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public class BinarySerializer
    {
        private int _bytesSaved;
        private int _declaredBytes;
        private byte[] _serializedBytes;

        public BinarySerializer(int requiredBytes)
        {
            _declaredBytes = requiredBytes;
            _serializedBytes = new byte[_declaredBytes];
            _bytesSaved = 0;
        }

        public byte[] GetSerialization() => _serializedBytes;

        public void SerializeInt(int value)
        {
            byte[] valueAsBytes = BitConverter.GetBytes(value);
            SerializeBytes(valueAsBytes);
        }

        public void SerializeBytes(byte[] bytes)
        {
            bytes.CopyTo(_serializedBytes, _bytesSaved);
            _bytesSaved += bytes.Length;
        }

        public void SerializeByte(byte b)
        {
            _serializedBytes[_bytesSaved] = b;
            _bytesSaved++;
        }

        public void SerializeFloat(float value)
        {
            byte[] valueAsBytes = BitConverter.GetBytes(value);
            SerializeBytes(valueAsBytes);
        }
    }

    public class BinaryDeserializer
    {
        private byte[] _sourceBytes;
        private int _cursor;
        private int _consumedBytes;

        public BinaryDeserializer(byte[] sourceBytes, int startIndex = 0)
        {
            _sourceBytes = sourceBytes;
            _cursor = startIndex;
            _consumedBytes = 0;
        }

        public int GetConsumedBytes() => _consumedBytes;

        public int DeserializeInt()
        {
            int value = BitConverter.ToInt32(_sourceBytes, _cursor);
            OnBytesConsumed(sizeof(int));

            return value;
        }

        public byte[] DeserializeBytes(int count)
        {
            byte[] bytes = new byte[count];

            for (int i = 0; i < count; i++)
            {
                bytes[i] = _sourceBytes[_cursor + i];
            }
            OnBytesConsumed(count);

            return bytes;
        }

        public byte DeserializeByte()
        {
            byte value = _sourceBytes[_cursor];
            OnBytesConsumed(1);

            return value;
        }

        public float DeserializeFloat()
        {
            float value = BitConverter.ToSingle(_sourceBytes, _cursor);
            OnBytesConsumed(sizeof(float));

            return value;
        }

        private void OnBytesConsumed(int consumedBytesCount)
        {
            _cursor += consumedBytesCount;
            _consumedBytes += consumedBytesCount;
        }
    }
}
