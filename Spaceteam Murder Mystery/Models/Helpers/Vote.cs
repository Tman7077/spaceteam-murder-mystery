namespace SMM.Models.Helpers;

/// <summary>
/// Represents a vote cast by a player in the game.
/// Upon creation, whether or not the vote was correct
/// is established.
/// </summary>
public record Vote
{
    /// <summary>
    /// The short name of the character that was voted for.
    /// </summary>
    public string Voted { get; }

    /// <summary>
    /// Indicates whether or not the vote was successful.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Creates a new vote for the given character,
    /// and determines whether or not the vote was successful
    /// based on the character's guilt in the given game state.
    /// </summary>
    /// <param name="Voted">The character's short name.</param>
    /// <param name="State">The current GameState by which to check the character's guilt.</param>
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

    /// <summary>
    /// Represents a vote for no specific character,
    /// which is unsuccessful. Used when the player
    /// skips voting.
    /// </summary>
    public sealed record None : Vote;
}