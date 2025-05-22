namespace SMM.Models
{
    public class ResponseSet(string innocent, string guilty)
    {
        public string Innocent { get; } = innocent;
        public string Guilty { get; } = guilty;
    }
}