namespace Toscana.Domain.DigitalUnits
{
    internal class KbFileSizeExpression : TerminalFileSizeExpression
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
}