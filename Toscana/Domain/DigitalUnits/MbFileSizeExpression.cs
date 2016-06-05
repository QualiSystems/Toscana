namespace Toscana.Domain.DigitalUnits
{
    internal class MbFileSizeExpression : TerminalFileSizeExpression
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
}