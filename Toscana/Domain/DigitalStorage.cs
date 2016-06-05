using System;
using System.Runtime.Serialization;
using Toscana.Domain.DigitalUnits;

namespace Toscana.Domain
{
    [Serializable]
    public struct DigitalStorage : ISerializable
    {
        private const string TotaBytesSerializationTag = "TotalBytes";
        private readonly long totalBytes;

        public DigitalStorage(SerializationInfo serializationInfo, StreamingContext streamingContext):
            this(serializationInfo.GetInt64(TotaBytesSerializationTag))
        {
        }

        public DigitalStorage(string formattedMemorySize)
            : this(new DigitalStorageParser().Parse(formattedMemorySize))
        {
        }

        public DigitalStorage(long totalBytes)
        {
            this.totalBytes = totalBytes;
        }

        public long TotalBytes
        {
            get { return totalBytes; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(TotaBytesSerializationTag, totalBytes);
        }

        public bool Equals(DigitalStorage other)
        {
            return totalBytes == other.totalBytes;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DigitalStorage && Equals((DigitalStorage) obj);
        }

        public override int GetHashCode()
        {
            return totalBytes.GetHashCode();
        }

        public override string ToString()
        {
            return FormatBytes(totalBytes);
        }

        private string FormatBytes(long bytes)
        {
            const int scale = 1024;
            string[] orders = {"GB", "MB", "KB", "Bytes"};
            var max = (long) Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }
    }
}