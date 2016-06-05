namespace Toscana.Domain.DigitalUnits
{
    internal abstract class TerminalFileSizeExpression : FileSizeExpression
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
}