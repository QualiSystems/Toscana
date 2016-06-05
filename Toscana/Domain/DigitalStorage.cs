using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Toscana.Domain
{
    [Serializable]
    public struct DigitalStorage : ISerializable
    {
        private const string TotaBytesSerializationTag = "TotalBytes";
        private readonly long m_TotalBytes;

        public DigitalStorage(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            m_TotalBytes = serializationInfo.GetInt64(TotaBytesSerializationTag);
        }

        public DigitalStorage(string formattedMemorySize)
            : this(new FileSizeParser().Parse(formattedMemorySize))
        {
        }

        public DigitalStorage(long totalBytes)
        {
            m_TotalBytes = totalBytes;
        }

        public long TotalBytes
        {
            get { return m_TotalBytes; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(TotaBytesSerializationTag, m_TotalBytes);
        }

        public bool Equals(DigitalStorage other)
        {
            return m_TotalBytes == other.m_TotalBytes;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DigitalStorage && Equals((DigitalStorage) obj);
        }

        public override int GetHashCode()
        {
            return m_TotalBytes.GetHashCode();
        }

        public override string ToString()
        {
            return FormatBytes(m_TotalBytes);
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

    public class DigitalStorageParsingContext
    {
        private string input;
        private long output;

        public DigitalStorageParsingContext(string input)
        {
            Input = input;
        }

        public string Input { get; set; }

        public long Output { get; set; }
    }

    public abstract class FileSizeExpression
    {
        public abstract void Interpret(DigitalStorageParsingContext value);
    }

    public abstract class TerminalFileSizeExpression : FileSizeExpression
    {
        public override void Interpret(DigitalStorageParsingContext value)
        {
            if (value.Input.EndsWith(ThisPattern()))
            {
                var amount = double.Parse(value.Input.Replace(ThisPattern(), string.Empty));
                var fileSize = (long) (amount*1024);
                value.Input = string.Format("{0}{1}", fileSize, NextPattern());
                value.Output = fileSize;
            }
        }

        protected abstract string ThisPattern();
        protected abstract string NextPattern();
    }

    public class KbFileSizeExpression : TerminalFileSizeExpression
    {
        protected override string ThisPattern()
        {
            return "KB";
        }

        protected override string NextPattern()
        {
            return "bytes";
        }
    }

    public class MbFileSizeExpression : TerminalFileSizeExpression
    {
        protected override string ThisPattern()
        {
            return "MB";
        }

        protected override string NextPattern()
        {
            return "KB";
        }
    }

    public class GbFileSizeExpression : TerminalFileSizeExpression
    {
        protected override string ThisPattern()
        {
            return "GB";
        }

        protected override string NextPattern()
        {
            return "MB";
        }
    }

    public class TbFileSizeExpression : TerminalFileSizeExpression
    {
        protected override string ThisPattern()
        {
            return "TB";
        }

        protected override string NextPattern()
        {
            return "GB";
        }
    }

    public class FileSizeParser : FileSizeExpression
    {
        private readonly List<FileSizeExpression> expressionTree = new List<FileSizeExpression>
        {
            new TbFileSizeExpression(),
            new GbFileSizeExpression(),
            new MbFileSizeExpression(),
            new KbFileSizeExpression()
        };

        public override void Interpret(DigitalStorageParsingContext value)
        {
            foreach (var exp in expressionTree)
            {
                exp.Interpret(value);
            }
        }

        public long Parse(string input)
        {
            var fileSizeContext = new DigitalStorageParsingContext(input);
            Interpret(fileSizeContext);
            return fileSizeContext.Output;
        }
    }
}