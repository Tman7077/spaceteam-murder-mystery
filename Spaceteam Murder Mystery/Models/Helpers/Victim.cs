namespace SMM.Models.Helpers;

public abstract record Victim
{
    public sealed record Random : Victim;
    public abstract record ByName : Victim
    {
        protected ByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            { throw new ArgumentException("CharacterName is required.", nameof(name)); }
            if (name.Contains(' '))
            { throw new ArgumentException("Provide the short character name.", nameof(name)); }
        }
        public sealed record Innocent(string Name) : ByName(Name);
        public sealed record Voted(string Name) : ByName(Name);
    }
}