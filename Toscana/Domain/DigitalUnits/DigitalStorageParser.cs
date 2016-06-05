using System.Collections.Generic;

namespace Toscana.Domain.DigitalUnits
{
    internal class DigitalStorageParser : FileSizeExpression
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