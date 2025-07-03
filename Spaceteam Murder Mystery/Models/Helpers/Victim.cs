namespace SMM.Models.Helpers;

/// <summary>
/// Represents a character that has died (or is about to die).
/// Used when killing a character, innocent or guilty.
/// </summary>
public abstract record Victim
{
    /// <summary>
    /// Represents a randomly selected victim.
    /// </summary>
    public sealed record Random : Victim;

    /// <summary>
    /// Represents a victim selected by their name.
    /// </summary>
    public abstract record ByName : Victim
    {
        /// <summary>
        /// The victim's short name.
        /// </summary>
        public string Name { get; }

        protected ByName(string Name)
        {
            Validator.ValidateShortCharacterName(Name);
            this.Name = Name;
        }

        /// <summary>
        /// Represents a victim that is innocent,
        /// but still voted by name.
        /// </summary>
        /// <param name="Name">The short character name.</param>
        public sealed record Innocent(string Name) : ByName(Name);

        /// <summary>
        /// Represents a victim that was voted by name,
        /// selected by the user via accusation.
        /// </summary>
        /// <param name="Name">The short character name.</param>
        public sealed record Voted(string Name) : ByName(Name);
    }
}