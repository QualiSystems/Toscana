namespace Toscana.Domain.DigitalUnits
{
    internal class GbFileSizeExpression : TerminalFileSizeExpression
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
}