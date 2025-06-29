namespace SMM.Models.Helpers;

public record Vote
{
    public string Voted { get; }
    public bool Success { get; }
    public Vote(string Voted, GameState State)
    {
        Validator.ValidateCharacter(Voted, State);
        this.Voted = Voted;
        Success    = State.Characters[Voted].IsGuilty;
    }
    protected Vote()
    {
        Voted   = string.Empty;
        Success = false;
    }
    public sealed record None : Vote;
}