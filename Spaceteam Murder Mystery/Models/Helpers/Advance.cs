namespace SMM.Models.Helpers;

public abstract record Advance
{
    public sealed record Intro       : Advance;
    public sealed record FirstMurder : Advance;
    public sealed record PreDeath    : Advance
    {
        public string? Victim { get; }
        public PreDeath(string Victim)
        {
            Validator.ValidateShortCharacterName(Victim);
            this.Victim = Victim;
        }
        public PreDeath()
        {
            Victim = null;
        }
    }
    public sealed record PostDeath : Advance;
    public sealed record PostAccusation(Vote Vote)   : Advance;
}