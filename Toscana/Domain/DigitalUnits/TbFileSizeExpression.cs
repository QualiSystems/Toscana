namespace Toscana.Domain.DigitalUnits
{
    internal class TbFileSizeExpression : TerminalFileSizeExpression
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
}