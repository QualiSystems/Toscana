namespace Toscana.Domain.DigitalUnits
{
    internal class DigitalStorageParsingContext
    {
        public DigitalStorageParsingContext(string input)
        {
            Input = input;
        }

        public string Input { get; set; }

        public long Output { get; set; }
    }
}