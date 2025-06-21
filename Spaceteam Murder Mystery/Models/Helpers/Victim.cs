namespace SMM.Models.Helpers;

public abstract record Victim
{
    public sealed record Random   : Victim;
    public abstract record ByName : Victim
    {
        public string Name { get; init; }
        protected ByName(string Name)
        {
            Validator.ValidateShortCharacterName(Name);
            this.Name = Name;
        }
        public sealed record Innocent(string Name) : ByName(Name);
        public sealed record Voted(string Name)    : ByName(Name);
    }
}