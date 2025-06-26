namespace SMM.Models.Helpers;

public sealed record Vote
{
    public string Voted   { get; }
    public bool   Success { get; }
    public Vote(string Voted, GameState State)
    {
        Validator.ValidateCharacter(Voted, State);
        this.Voted = Voted;
        Success = State.Characters[Voted].IsGuilty;
    }
}