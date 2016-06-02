using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Toscana.Domain
{
    [Serializable]
    public struct MemorySize : ISerializable
    {
        private const string TotaBytesSerializationTag = "TotalBytes";
        private readonly long m_TotalBytes;

        public MemorySize(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            m_TotalBytes = serializationInfo.GetInt64(TotaBytesSerializationTag);
        }

        public MemorySize(string formattedMemorySize)
            : this(new FileSizeParser().Parse(formattedMemorySize))
        {
        }

        public MemorySize(long totalBytes)
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
    }

    public class FileSizeContext
    {
        private string input;
        private long output;

        public FileSizeContext(string input)
        {
            Input = input;
        }

        public string Input { get; set; }

        public long Output { get; set; }
    }

    public abstract class FileSizeExpression
    {
        public abstract void Interpret(FileSizeContext value);
    }

    public abstract class TerminalFileSizeExpression : FileSizeExpression
    {
        public override void Interpret(FileSizeContext value)
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

        public override void Interpret(FileSizeContext value)
        {
            foreach (var exp in expressionTree)
            {
                exp.Interpret(value);
            }
        }

        public long Parse(string input)
        {
            var fileSizeContext = new FileSizeContext(input);
            Interpret(fileSizeContext);
            return fileSizeContext.Output;
        }
    }
}