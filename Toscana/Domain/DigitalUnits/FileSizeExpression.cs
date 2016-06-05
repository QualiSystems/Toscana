namespace Toscana.Domain.DigitalUnits
{
    internal abstract class FileSizeExpression
    {
        public abstract void Interpret(DigitalStorageParsingContext value);
    }
}