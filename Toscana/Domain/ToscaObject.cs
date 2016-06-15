namespace Toscana.Domain
{
    public class ToscaObject
    {
        public string DerivedFrom { get; set; }

        public bool IsRoot()
        {
            return string.IsNullOrEmpty(DerivedFrom);
        }
    }
}