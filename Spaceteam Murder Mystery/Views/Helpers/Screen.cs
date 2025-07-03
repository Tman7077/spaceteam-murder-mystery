namespace SMM.Views.Helpers;

/// <summary>
/// A reference to a UserControl type to display,
/// allowing for simplified and unified instantiation.
/// </summary>
public abstract record Screen
{
    /// <summary>
    /// Creates a screen to display the crime scene of a victim.
    /// </summary>
    public sealed record CrimeScene : Screen
    {
        /// <summary>
        /// The name of the victim whose crime scene to display.
        /// </summary>
        public string VictimName { get; }

        /// <summary>
        /// Creates a screen to display the crime scene of a victim.
        /// </summary>
        /// <param name="VictimName">The name of the victim whose crime scene to display.</param>
        public CrimeScene(string VictimName)
        {
            Validator.ValidateShortCharacterName(VictimName);
            this.VictimName = VictimName;
        }
    }

    /// <summary>
    /// Creates a screen to introduce a character by name and index.
    /// </summary>
    public sealed record CharacterIntro : Screen
    {
        /// <summary>
        /// The name of the character to introduce.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The index of the character in the introduction sequence.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Creates a screen to introduce a character by name and index.
        /// </summary>
        /// <param name="Name">The name of the character to introduce.</param>
        /// <param name="Index">The index of the character in the introduction sequence.</param>
        public CharacterIntro(string Name, int Index)
        {
            Validator.ValidateShortCharacterName(Name);
            this.Name = Name;
            this.Index = Index;
        }
    }

    /// <summary>
    /// Creates a difficulty selection screen.
    /// </summary>
    public sealed record Difficulty : Screen;

    /// <summary>
    /// Creates an end screen indicating whether the player won or lost the game.
    /// </summary>
    /// <param name="Victory">Whether the player won or lost the game.</param>
    public sealed record End(bool Victory) : Screen;

    /// <summary>
    /// Creates a screen to display a character's interview or accusation response.
    /// </summary>
    public sealed record InspectChar : Screen
    {
        /// <summary>
        /// The type of interview being conducted.
        /// </summary>
        public InterviewType Type { get; }

        /// <summary>
        /// The name of the character being interviewed.
        /// </summary>
        public string Interviewee { get; }

        /// <summary>
        /// The name of the victim related to the interview.
        /// </summary>
        public string Victim { get; }

        /// <summary>
        /// Creates a screen to display a character's interview or accusation response.
        /// </summary>
        /// <param name="Type">The type of interview being conducted.</param>
        /// <param name="Interviewee">The name of the character being interviewed.</param>
        /// <param name="Victim">The name of the victim related to the interview.</param>
        public InspectChar(InterviewType Type, string Interviewee, string Victim)
        {
            Validator.ValidateShortCharacterName(Interviewee);
            Validator.ValidateShortCharacterName(Victim);
            this.Type = Type;
            this.Interviewee = Interviewee;
            this.Victim = Victim;
        }
    }

    /// <summary>
    /// Creates an inspection screen for a givenclue.
    /// </summary>
    /// <param name="Clue">The clue to inspect.</param>
    public sealed record InspectClue(Clue Clue) : Screen;

    /// <summary>
    /// Creates a screen to display the pause menu.
    /// </summary>
    public sealed record Pause : Screen;

    /// <summary>
    /// Creates a screen to display all characters and allow for interviews or accusations.
    /// </summary>
    /// <param name="Type">The type of interview to conduct.</param>
    public sealed record Selection(InterviewType Type) : Screen;

    /// <summary>
    /// Creates a screen to display the settings menu.
    /// </summary>
    public sealed record Settings : Screen;

    /// <summary>
    /// Creates a screen to display a portion of the story progression.
    /// </summary>
    /// <param name="Advance">The type of advance being made in the story.</param>
    public sealed record Story(Advance Advance) : Screen;

    /// <summary>
    /// Creates a screen to display the main menu of the game.
    /// </summary>
    public sealed record Title : Screen;
}