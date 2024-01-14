using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        public void SerializeMatrix4x4(Matrix4x4 matrix)
        {
            for(int row = 0; row < 4; row++)
            {
                for(int col = 0; col < 4; col++)
                {
                    byte[] valueAsBytes = BitConverter.GetBytes(matrix[row, col]);
                    SerializeBytes(valueAsBytes);
                }
            }
        }

        public void SerializeVector3(Vector3 vector)
        {
            for(int i = 0; i < 3; i++)
            {
                byte[] valueAsBytes = BitConverter.GetBytes(vector[i]);
                SerializeBytes(valueAsBytes);
            }
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

        public Matrix4x4 DeserializeMatrix4x4()
        {
            Matrix4x4 matrix = new Matrix4x4();

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    float value = DeserializeFloat();
                    matrix[row, col] = value;
                }
            }

            return matrix;
        }

        public Vector3 DeserializeVector3()
        {
            Vector3 vector = new Vector3();

            for(int i = 0; i < 3; i++)
            {
                float value = DeserializeFloat();
                vector[i] = value;
            }

            return vector;
        }

        private void OnBytesConsumed(int consumedBytesCount)
        {
            _cursor += consumedBytesCount;
            _consumedBytes += consumedBytesCount;
        }
    }
}
