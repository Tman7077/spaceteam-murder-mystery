namespace SMM.Models.Helpers;

/// <summary>
/// A record representing the different
/// types of story screen to display.
/// </summary>
public abstract record Advance
{
    /// <summary>
    /// Marks that the story screen should display the intro screen.
    /// </summary>
    public sealed record Intro : Advance;

    /// <summary>
    /// Marks that the story screen should display the first murder intro screen.
    /// </summary>
    public sealed record FirstMurder : Advance;

    /// <summary>
    /// Marks that the story screen should display
    /// a character's pre-death story.
    /// </summary>
    public sealed record PreDeath : Advance
    {
        /// <summary>
        /// The short name of the character that died.
        /// </summary>
        public string? Victim { get; }

        /// <summary>
        /// Marks that the story screen should display
        /// a pre-death screen for a predefined victim.
        /// </summary>
        /// <param name="Victim">The name of the character that should be displayed.</param>
        public PreDeath(string Victim)
        {
            Validator.ValidateShortCharacterName(Victim);
            this.Victim = Victim;
        }

        /// <summary>
        /// Marks that the story screen should display
        /// a pre-death screen for a predefined victim.
        /// </summary>
        public PreDeath()
        {
            Victim = null;
        }
    }

    /// <summary>
    /// Marks that the story screen should display
    /// a character's post-death story.
    /// </summary>
    public sealed record PostDeath : Advance;

    /// <summary>
    /// Marks that the story screen should display
    /// a character's post-accusation screen,
    /// which includes the result of the accusation.
    /// </summary>
    /// <param name="Vote">The Vote regarding the character.</param>
    public sealed record PostAccusation(Vote Vote) : Advance;
}